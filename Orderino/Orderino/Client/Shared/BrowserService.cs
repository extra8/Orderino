using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Orderino.Client.Shared
{
    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<WindowDimension> GetDimensions()
        {
            return await _js.InvokeAsync<WindowDimension>("getDimensions");
        }

    }
}
