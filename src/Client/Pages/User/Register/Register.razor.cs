using AntDesign.Boilerplate.Server.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Pro.Template.Pages.User
{
    public partial class Register
    {
        private readonly RegisterModel _user = new RegisterModel();

        [Inject] private HttpClient HttpClient { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        public async Task SubmitRegister()
        {
            var response = await HttpClient.PostAsJsonAsync("api/account/register", _user);
            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("user/login");
            }
        }
    }
}