using Microsoft.Net.Http.Headers;
using ServerLoginApp.Modelos;
using ServerLoginApp.Utilerias;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ServerLoginApp.Controllers
{
    public class BindIdManager
    {
        private readonly HttpClient _httpClient;

        private const string SECRET = "275577f6-fec4-459f-8e74-4598cad97294";
        private const string CLIENTID = "fb355573.417b53dd.tid_414530e0.bindid.io";


        public BindIdManager()
        {

            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpRequestsSample");

        }

        public async Task<BindID> PostRequestGetUserToken(string authCode)
        {
            TokenBindId tokenBindId = new TokenBindId
            {
                Code = authCode
     ,
                RedirectUri = "https://loginapp-three.vercel.app/user"
     ,
                ClientId = CLIENTID
     ,
                ClientSecret = SECRET
            };

            var data = new[]
            {
                new KeyValuePair<string, string>("grant_type", tokenBindId.GrantType),
                new KeyValuePair<string, string>("code", tokenBindId.Code),
                new KeyValuePair<string, string>("redirect_uri", tokenBindId.RedirectUri),
                new KeyValuePair<string, string>("client_id", tokenBindId.ClientId),
                new KeyValuePair<string, string>("client_secret", tokenBindId.ClientSecret),
            };


            _httpClient.BaseAddress = new Uri("https://signin.bindid-sandbox.io/");

            var req = new HttpRequestMessage(HttpMethod.Post, "/token") { Content = new FormUrlEncodedContent(data) };
            var httpResponseMessage = await _httpClient.SendAsync(req);

            string jsonData = await httpResponseMessage.Content.ReadAsStringAsync();

            BindID bindID = JsonSerializer.Deserialize<BindID>(jsonData);

            return bindID;
        }

        public async Task<HttpResponseMessage> PostSendBindAliasIsNewUser(string bindIdAccesToken, string bindIdAlias)
        {
            var httpClient = new HttpClient();
            SHA256Converter sHA256Converter = new SHA256Converter();
            Feedback feedback = new Feedback
            {
                SubjectSessionAt = bindIdAccesToken,
                Reports = new List<Report>()
                {
                    new Report
                    {
                        Alias = bindIdAlias
                    }
                }
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var stringPayload = JsonSerializer.Serialize<Feedback>(feedback);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            _httpClient.BaseAddress = new Uri("https://api.bindid-sandbox.io/");

            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, sHA256Converter.calculateAuthorizationHeaderValue(SECRET, bindIdAccesToken));
            using var httpResponseMessage = await _httpClient.PostAsync("/session-feedback", httpContent);

            return httpResponseMessage;
        }

    }
}
