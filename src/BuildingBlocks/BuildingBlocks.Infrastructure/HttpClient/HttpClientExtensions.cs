using System.Net.Http.Headers;
using System.Text;

namespace BuildingBlocks.Infrastructure.HttpClient;

public static class HttpClientExtensions
{
    public static void UseBasicAuthentication(this System.Net.Http.HttpClient client, string userName, string password)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(userName + ":" + password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    public static void UseBearerToken(this System.Net.Http.HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
