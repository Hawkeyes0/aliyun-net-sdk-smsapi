using System;
using System.Collections.Generic;
using System.Text;
using Aliyuncs.Auth;
using Aliyuncs.Http;
using Aliyuncs.Profile;
using Aliyuncs.Regions;
using Aliyuncs.Exceptions;
using System.IO;
using Aliyuncs.Reader;
using Aliyuncs.Transform;
using System.Reflection;

namespace Aliyuncs
{
    public class DefaultAcsClient : IAcsClient
    {
        private int maxRetryNumber = 3;
        private bool autoRetry = true;
        private IClientProfile clientProfile = null;
        private bool urlTestFlag = false;

        public DefaultAcsClient()
        {
            clientProfile = DefaultProfile.GetProfile();
        }

        public DefaultAcsClient(IClientProfile profile)
        {
            clientProfile = profile;
        }

        public HttpResponse DoAction<T>(AcsRequest<T> request) where T : AcsResponse
        {
            return DoAction(request, autoRetry, maxRetryNumber, clientProfile);
        }

        public HttpResponse DoAction<T>(AcsRequest<T> request, bool autoRetry, int maxRetryCounts) where T : AcsResponse
        {
            return DoAction(request, autoRetry, maxRetryCounts, clientProfile);
        }

        public HttpResponse DoAction<T>(AcsRequest<T> request, IClientProfile profile) where T : AcsResponse
        {
            return DoAction(request, autoRetry, maxRetryNumber, profile);
        }

        public HttpResponse DoAction<T>(AcsRequest<T> request, string regionId, Credential credential) where T : AcsResponse
        {
            bool retry = autoRetry;
            int retryNumber = maxRetryNumber;
            ISigner signer = null;
            FormatType format = null;
            List<Endpoint> endpoints = null;
            if (null == request.RegionId)
            {
                request.RegionId = regionId;
            }
            if (null != clientProfile)
            {
                signer = clientProfile.Signer;
                format = clientProfile.Format;
                try
                {
                    endpoints = clientProfile.GetEndpoints(request.Product, request.RegionId,
                            request.LocationProduct,
                            request.EndpointType);
                }
                catch
                {
                    endpoints = clientProfile.GetEndpoints(request.RegionId, request.Product);
                }
            }

            return DoAction(request, retry, retryNumber, request.RegionId, credential, signer, format, endpoints);
        }

        public HttpResponse DoAction<T>(AcsRequest<T> request, bool autoRetry, int maxRetryCounts, IClientProfile profile) where T : AcsResponse
        {
            if (null == profile)
            {
                throw new ClientException("SDK.InvalidProfile", "No active profile found.");
            }
            bool retry = autoRetry;
            int retryNumber = maxRetryCounts;
            String region = profile.RegionId;
            if (null == request.RegionId)
            {
                request.RegionId = region;
            }
            Credential credential = profile.Credential;
            ISigner signer = profile.Signer;
            FormatType format = profile.Format;
            List<Endpoint> endpoints;
            try
            {
                endpoints = clientProfile.GetEndpoints(request.Product, request.RegionId,
                        request.LocationProduct,
                        request.EndpointType);
            }
            catch
            {
                endpoints = clientProfile.GetEndpoints(request.RegionId, request.Product);
            }
            return DoAction(request, retry, retryNumber, request.RegionId, credential, signer, format, endpoints);
        }

        public HttpResponse DoAction<T>(AcsRequest<T> request, bool autoRetry, int maxRetryNumber, string regionId, Credential credential, ISigner signer, FormatType format, List<Endpoint> endpoints) where T : AcsResponse
        {
            try
            {
                FormatType requestFormatType = request.AcceptFormat;
                if (null != requestFormatType)
                {
                    format = requestFormatType;
                }
                ProductDomain domain = Endpoint.FindProductDomain(regionId, request.Product, endpoints);
                if (null == domain)
                {
                    throw new ClientException("SDK.InvalidRegionId", "Can not find endpoint to access.");
                }
                HttpRequest httpRequest = request.SignRequest(signer, credential, format, domain);

                if (urlTestFlag)
                {
                    throw new ClientException("URLTestFlagIsSet", httpRequest.Url);
                }

                int retryTimes = 1;
                HttpResponse response = HttpResponse.GetResponse(httpRequest);
                while (500 <= response.Status && autoRetry && retryTimes < maxRetryNumber)
                {
                    httpRequest = request.SignRequest(signer, credential, format, domain);
                    response = HttpResponse.GetResponse(httpRequest);
                    retryTimes++;
                }
                return response;
            }
            catch (InvalidKeyException)
            {
                throw new ClientException("SDK.InvalidAccessSecret", "Speicified access secret is not valid.");
            }
            catch (SocketTimeoutException)
            {
                throw new ClientException("SDK.ServerUnreachable", "SocketTimeoutException has occurred on a socket read or accept.");
            }
            catch (IOException exp)
            {
                throw new ClientException("SDK.ServerUnreachable", "Server unreachable: " + exp.ToString());
            }
            catch (NoSuchAlgorithmException)
            {
                throw new ClientException("SDK.InvalidMD5Algorithm", "MD5 hash is not supported by client side.");
            }
        }

        public T GetAcsResponse<T>(AcsRequest<T> request) where T : AcsResponse
        {
            HttpResponse baseResponse = DoAction(request);
            return ParseAcsResponse<T>(request.GetResponseClass(), baseResponse);
        }

        public T GetAcsResponse<T>(AcsRequest<T> request, bool autoRetry, int maxRetryCounts) where T : AcsResponse
        {
            HttpResponse baseResponse = DoAction(request, autoRetry, maxRetryCounts);
            return ParseAcsResponse<T>(request.GetResponseClass(), baseResponse);
        }

        public T GetAcsResponse<T>(AcsRequest<T> request, IClientProfile profile) where T : AcsResponse
        {
            HttpResponse baseResponse = DoAction(request, profile);
            return ParseAcsResponse<T>(request.GetResponseClass(), baseResponse);
        }

        public T GetAcsResponse<T>(AcsRequest<T> request, string regionId, Credential credential) where T : AcsResponse
        {
            HttpResponse baseResponse = DoAction(request, regionId, credential);
            return ParseAcsResponse<T>(request.GetResponseClass(), baseResponse);
        }

        private T ParseAcsResponse<T>(Type clasz, HttpResponse baseResponse) where T : AcsResponse
        {

            FormatType format = baseResponse.ContentType;

            if (baseResponse.IsSuccess)
            {
                return ReadResponse<T>(clasz, baseResponse, format);
            }
            else
            {
                AcsError error = ReadError(baseResponse, format);
                if (500 <= baseResponse.Status)
                {
                    throw new ServerException(error.ErrorCode, error.ErrorMessage, error.RequestId);
                }
                else
                {
                    throw new ClientException(error.ErrorCode, error.ErrorMessage, error.RequestId);
                }
            }
        }

        private T ReadResponse<T>(Type clasz, HttpResponse httpResponse, FormatType format) where T : AcsResponse
        {
            Assembly assembly = Assembly.Load(new AssemblyName(clasz.AssemblyQualifiedName));
            IReader reader = ReaderFactory.CreateInstance(format);
            UnmarshallerContext context = new UnmarshallerContext();
            T response = null;
            String stringContent = GetResponseContent(httpResponse);
            try
            {
                response = (T)assembly.CreateInstance(clasz.FullName);
            }
            catch (Exception e)
            {
                throw new ClientException("SDK.InvalidResponseClass", "Unable to allocate " + clasz.FullName + " class");
            }
            String responseEndpoint = clasz.Name.Substring(clasz.FullName.LastIndexOf(".") + 1);
            context.ResponseMap = reader.Read(stringContent, responseEndpoint);
            context.HttpResponse=httpResponse;
            response.GetInstance(context);
            return response;
        }

        private string GetResponseContent(HttpResponse httpResponse)
        {
            String stringContent = null;
            try
            {
                if (null == httpResponse.Encoding)
                {
                    stringContent = Encoding.Default.GetString(httpResponse.Content);
                }
                else
                {
                    stringContent = httpResponse.Encoding.GetString(httpResponse.Content);
                }
            }
            catch
            {
                throw new ClientException("SDK.UnsupportedEncoding", "Can not parse response due to un supported encoding.");
            }
            return stringContent;
        }

        private AcsError ReadError(HttpResponse httpResponse, FormatType format)
        {
            AcsError error = new AcsError();
            String responseEndpoint = "Error";
            IReader reader = ReaderFactory.CreateInstance(format);
            UnmarshallerContext context = new UnmarshallerContext();
            String stringContent = GetResponseContent(httpResponse);
            context.ResponseMap=reader.Read(stringContent, responseEndpoint);
            return error.GetInstance(context) as AcsError;
        }
    }
}