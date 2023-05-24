namespace VKR_BlazorWASM
{
    public class Utils
    {
        public static string Base64ToString(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            string decodedString = System.Text.Encoding.UTF8.GetString(bytes);
            return decodedString;
        }
    }
}
