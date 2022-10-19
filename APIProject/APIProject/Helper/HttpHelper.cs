using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Security;

namespace APIProject.Helper
{
    public class HttpHelper
    {
        private string url;
        private delegate string Method<H, J>(H h, J j);
        private delegate string Method<H, J, F>(H h, J j, F f);

        public HttpHelper(string domain, string url)
        {
            this.url = domain + url;
        }
        public string get(Dictionary<string, string> jsonBody)
        {
            return perform(_get, jsonBody);
        }
        public string post(Dictionary<string, string> jsonBody)
        {
            return perform(_post, jsonBody);
        }
        public string post(Dictionary<string, string> jsonBody, byte[] fileBytes)
        {
            return performMultipart(_postFileWithBody, jsonBody, fileBytes);
        }

        private HttpClientHandler getHandler()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            return httpClientHandler;
        }

        private string perform(Method<HttpClientHandler, Dictionary<string, string>> method, Dictionary<string, string> jsonBody)
        {
            using (var httpClientHandler = getHandler())
            {
                return method(getHandler(), jsonBody);
            }
        }
        private string performMultipart(Method<HttpClientHandler, Dictionary<string, string>, byte[]> method, Dictionary<string, string> jsonBody, byte[] fileBytes)
        {
            using (var httpClientHandler = getHandler())
            {
                return method(httpClientHandler, jsonBody, fileBytes);
            }
        }

        private string _get(HttpClientHandler handler, Dictionary<string, string> jsonBody)
        {
            using (var client = new HttpClient(handler))
            {
                var uri = QueryHelpers.AddQueryString(url, jsonBody);
                var result = client.GetAsync(uri).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }

        private string _post(HttpClientHandler handler, Dictionary<string, string> body)
        {
            using (var client = new HttpClient(handler))
            {
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri(url);
                    content.Add(new StringContent(body["accesstoken"]), "accesstoken");
                    content.Add(new StringContent(body["mac"]), "mac");
                    content.Add(new StringContent(body["data"]), "data");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    var response = client.PostAsync(url, content).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
        }

        private string _postFileWithBody(HttpClientHandler handler, Dictionary<string, string> body, byte[] fileBytes)
        {
            using (var client = new HttpClient(handler))
            {
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri(url);
                    content.Add(new ByteArrayContent(fileBytes), "uploadFile", "api-demo.docx");
                    content.Add(new StringContent(body["accesstoken"]), "accesstoken");
                    content.Add(new StringContent(body["mac"]), "mac");
                    content.Add(new StringContent(body["data"]), "data");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    var response = client.PostAsync(url, content).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
        }
    }
}
