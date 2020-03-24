using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EM.AppServices
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, 
                JsonConvert.SerializeObject(value, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                }));
        }
        public static T GetJson<T>(this ISession session, string key) {
            var sessionData = session.GetString(key); return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData); }
    }
}
