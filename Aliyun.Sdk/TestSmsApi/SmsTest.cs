using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Aliyuncs.Profile;
using Aliyuncs;
using Aliyun.SmsApi.Model;

namespace TestSmsApi
{
    [TestClass]
    public class SmsTest
    {
        // 产品名称:云通信短信API产品,开发者无需替换
        const String product = "Dysmsapi";
        // 产品域名,开发者无需替换
        const String domain = "dysmsapi.aliyuncs.com";

        // TODO 此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
        const String accessKeyId = "LTAI8rj7YUCeeF3s";
        const String accessKeySecret = "fWcP5RDzj2mMDK1N51adcHdfLsJJJY";

        [TestMethod]
        public void TestSendSms()
        {
            // 初始化acsClient,暂不支持region化
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);

            // 组装请求对象-具体描述见控制台-文档部分内容
            SendSmsRequest request = new SendSmsRequest();
            // 必填:待发送手机号
            request.PhoneNumbers = "18611671896";
            // 必填:短信签名-可在短信控制台中找到
            request.SignName = "金色北京";
            // 必填:短信模板-可在短信控制台中找到
            request.TemplateCode = "SMS_76495028";
            // 可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
            // request.setTemplateParam("{\"name\":\"Tom\", \"code\":\"123\"}");
            // 可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
            // request.setOutId("yourOutId");

            // hint 此处可能会抛出异常，注意catch
            SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);

            Debug.WriteLine("短信接口返回的数据----------------");
            Debug.WriteLine("Code=" + sendSmsResponse.Code);
            Debug.WriteLine("Message=" + sendSmsResponse.Message);
            Debug.WriteLine("RequestId=" + sendSmsResponse.RequestId);
            Debug.WriteLine("BizId=" + sendSmsResponse.BizId);
        }
    }
}
