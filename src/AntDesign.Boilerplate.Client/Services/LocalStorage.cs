using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Text.Json;

namespace AntDesign.Boilerplate.Client.Services
{
    public class LocalStorage
    {
        private IJSRuntime _js;

        public LocalStorage(IJSRuntime js)
        {
            _js = js;
        }

        public async ValueTask SetAsync<TValue>(string key, TValue value) where TValue : class
        {
            var json = value is null ? null : JsonSerializer.Serialize(value);
            await _js.InvokeVoidAsync("window.localStorage.setItem", key, json);
        }

        public async ValueTask<TValue> GetAsync<TValue>(string key)
        {
            var json = await _js.InvokeAsync<string>("window.localStorage.getItem", key);
            if (json == null)
                return default;

            return JsonSerializer.Deserialize<TValue>(json);
        }

        public async ValueTask Clear(string key)
        {
            await _js.InvokeVoidAsync("window.localStorage.setItem", key, null);
        }
    }
}