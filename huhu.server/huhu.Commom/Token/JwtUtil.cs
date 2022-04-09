using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System.Configuration;

namespace huhu.Commom.Token
{
    public class JwtUtil
    {
        //私钥
        private static string secret = ConfigurationManager.AppSettings["TokenPrivateKey"].ToString();

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string SetJwtEncode(model model)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(model, secret);
        }

        /// <summary>
        /// 根据Token获取实体
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public static T GetJwtDecode<T>(string token) where T : new()
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            var algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            return decoder.DecodeToObject<T>(token, secret, verify: true);
        }

    }
}
