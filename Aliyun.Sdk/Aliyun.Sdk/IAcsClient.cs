using Aliyuncs.Auth;
using Aliyuncs.Http;
using Aliyuncs.Profile;
using Aliyuncs.Regions;
using System;
using System.Collections.Generic;

namespace Aliyuncs
{
    public interface IAcsClient
    {
        HttpResponse DoAction<T>(AcsRequest<T> request) where T : AcsResponse;

        HttpResponse DoAction<T>(AcsRequest<T> request, bool autoRetry, int maxRetryCounts) where T : AcsResponse;

        HttpResponse DoAction<T>(AcsRequest<T> request, IClientProfile profile) where T : AcsResponse;

        HttpResponse DoAction<T>(AcsRequest<T> request, string regionId, Credential credential) where T : AcsResponse;

        HttpResponse DoAction<T>(AcsRequest<T> request, bool autoRetry, int maxRetryCounts, IClientProfile profile) where T : AcsResponse;

        HttpResponse DoAction<T>(AcsRequest<T> request,
            bool autoRetry, int maxRetryNumber,
            String regionId, Credential credential,
            ISigner signer, FormatType format,
            List<Endpoint> endpoints) where T : AcsResponse;

        T GetAcsResponse<T>(AcsRequest<T> request) where T : AcsResponse;

        T GetAcsResponse<T>(AcsRequest<T> request, bool autoRetry, int maxRetryCounts) where T : AcsResponse;

        T GetAcsResponse<T>(AcsRequest<T> request, IClientProfile profile) where T : AcsResponse;

        T GetAcsResponse<T>(AcsRequest<T> request, string regionId, Credential credential) where T : AcsResponse;
    }
}
