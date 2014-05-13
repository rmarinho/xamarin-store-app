using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Xamarin.SSO.Client
{
    public class XamarinSSOClient
    {
        const int TimeoutSeconds = 30;
        const string user_agent = "XamarinSSO .NET v1.0";
        string auth_api_url;
        string apikey;
        HttpClient httpClient;

        public XamarinSSOClient(string apikey)
            : this("https://auth.xamarin.com", apikey)
        {
        }

        public XamarinSSOClient(string base_url, string apikey)
        {
            auth_api_url = base_url + "/api/v1/auth";
            this.apikey = apikey;
            this.httpClient = new HttpClient(new HttpClientHandler
            {
                PreAuthenticate = true,
                Credentials = new NetworkCredential(apikey, "")
            });
            this.httpClient.BaseAddress = new Uri(auth_api_url);
        }
        
        protected virtual HttpRequestMessage SetupHttpRequest(HttpMethod method, string url)
        {
            var httpRequestMessage = new HttpRequestMessage(method, url);

            httpRequestMessage.Headers.Add(System.Net.HttpRequestHeader.Authorization.ToString(), string.Format("Basic {0}", Convert.ToBase64String(Encoding.UTF8.GetBytes(apikey + ":"))));
            httpRequestMessage.Headers.Add(HttpRequestHeader.UserAgent.ToString(), user_agent);

            return httpRequestMessage;
        }

        protected async virtual Task<string> DoRequest(string endpoint, string method = "GET", string body = null)
        {
            string result = null;
            var httpmethod = HttpMethod.Get;
            switch (method)
            {
                case "POST":
                    httpmethod = HttpMethod.Post;
                    break;
                default:
                    break;
            }
            var req = SetupHttpRequest(httpmethod, endpoint);
            if (body != null)
            {
                req.Content = new StringContent(body);
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            }
            try
            {
                var response = await httpClient.SendAsync(req);
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                //Todo : log this
            }



            return result;
        }

        public async Task<AccountResponse> CreateToken(string email, string password)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email");
            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("password");

            var str = String.Format("email={0}&password={1}", UrlEncode(email), UrlEncode(password));
            string json = await DoRequest(auth_api_url, "POST", str);
            return JsonConvert.DeserializeObject<AccountResponse>(json);
        }

        static string UrlEncode(string src)
        {
            if (src == null)
                return null;

            return WebUtility.UrlEncode(src);
        }
    }
}

