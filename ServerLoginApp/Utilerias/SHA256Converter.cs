using System;
using System.Security.Cryptography;
using System.Text;

namespace ServerLoginApp.Utilerias
{
    public class SHA256Converter
    {
        public string calculateAuthorizationHeaderValue(string clientSecret, string bindIdAccessToken) {

            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(bindIdAccessToken));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);


            return "BindIdBackend AccessToken " + bindIdAccessToken + "; " + encodedSignature;
        }

    }
}