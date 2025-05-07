using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace WebApp.Extensions
{
    public static class HttpClientExtension
    {
        public async static Task<TResult> PostGetResponseAsync<TResult, TValue>(this HttpClient client, string url, TValue value)
        {
            var httpRes = await client.PostAsJsonAsync(url, value);

            return httpRes.IsSuccessStatusCode ? await httpRes.Content.ReadFromJsonAsync<TResult>() : default;

        }

        public async static Task PostAsync<TValue>(this HttpClient client, string url, TValue value)
        {
            await client.PostAsJsonAsync(url, value);
        }

        public async static Task<T> GetReponseAsync<T>(this HttpClient client, string url)
        {
            return await client.GetFromJsonAsync<T>(url);
        }
    }
}
