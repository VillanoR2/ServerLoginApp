using System.Text.Json.Serialization;

namespace ServerLoginApp.Modelos
{
    public class BindID
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

    }
}
