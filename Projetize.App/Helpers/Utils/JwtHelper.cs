using System.Text.Json;

namespace Projetize.App.Helpers.Utils
{
    public class JwtHelper
    {
        public static DateTime? GetExpiration(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var parts = token.Split('.');

            if (parts.Length != 3)
                return null;

            string payload = parts[1];

            string paddedPayload = PadBase64(payload);

            byte[] data = Convert.FromBase64String(paddedPayload);

            string jsonPayload = System.Text.Encoding.UTF8.GetString(data);

            var payloadObj = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonPayload);

            if (payloadObj != null && payloadObj.TryGetValue("exp", out var expElement))
            {
                long expSeconds = expElement.GetInt64();
                return DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
            }

            return null;
        }

        private static string PadBase64(string input)
        {
            return input.PadRight(input.Length + (4 - input.Length % 4) % 4, '=');
        }
    }
}
