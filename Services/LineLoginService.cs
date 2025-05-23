using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using erp_server.Dtos;
using erp_server.Providers;

namespace erp_server.Services
{
    public class LineLoginService(IConfiguration configuration)
    {

        private static readonly HttpClient client = new();
        private readonly JsonProvider _jsonProvider = new();


        // key 
        private readonly string clientId = configuration["LineLogin:ClientId"] ?? "";
        private readonly string clientSecret = configuration["LineLogin:ClientSecret"] ?? "";
        private readonly string redirectUrl = (configuration["BaseUrl"] ?? "") + "/api/line/callback";


        // 回傳 line authorization url
        private readonly string loginUrl = "https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope={4}";
        public string GetLoginUrl()
        {
            // 根據想要得到的資訊填寫 scope
            var scope = "profile&openid";
            // 這個 state 是隨便打的
            var state = Guid.NewGuid().ToString("N");
            var uri = string.Format(loginUrl, "code", clientId, HttpUtility.UrlEncode(redirectUrl), state, scope);
            return uri;
        }

        private readonly string tokenUrl = "https://api.line.me/oauth2/v2.1/token";
        // 取得 access token 等資料
        public async Task<TokensResponseDto> GetTokensByAuthToken(string authToken, string callbackUri)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("grant_type", "authorization_code"),
        new KeyValuePair<string, string>("code", authToken),
        new KeyValuePair<string, string>("redirect_uri",callbackUri),
        new KeyValuePair<string, string>("client_id", clientId),
        new KeyValuePair<string, string>("client_secret", clientSecret),
    });

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //添加 accept header
            var response = await client.PostAsync(tokenUrl, formContent); // 送出 post request
            var dto = _jsonProvider.Deserialize<TokensResponseDto>(await response.Content.ReadAsStringAsync()); //將 json response 轉成 dto

            return dto;
        }

        private readonly string profileUrl = "https://api.line.me/v2/profile";


        public async Task<UserProfileDto> GetUserProfileByAccessToken(string accessToken)
        {
            //取得 UserProfile
            var request = new HttpRequestMessage(HttpMethod.Get, profileUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);
            var profile = _jsonProvider.Deserialize<UserProfileDto>(await response.Content.ReadAsStringAsync());

            return profile;
        }

    }
}
