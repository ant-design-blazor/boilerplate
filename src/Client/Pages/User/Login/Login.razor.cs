using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AntDesign.Boilerplate.Client.Models;
using AntDesign.Boilerplate.Client.Services;

namespace AntDesign.Pro.Template.Pages.User
{
    public partial class Login : ComponentBase
    {
        private readonly LoginParamsType _model = new LoginParamsType() { UserName = "alice", Password = "alice" };

        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public IAccountService AccountService { get; set; }

        [Inject] public MessageService Message { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (await AccountService.IsLoginAsync())
            {
                NavigationManager.NavigateTo("/");
            }
        }

        public async Task HandleSubmit()
        {
            if (await AccountService.LoginAsync(_model))
            {
                NavigationManager.NavigateTo("/");
            }
        }

        public async Task GetCaptcha()
        {
            var captcha = await AccountService.GetCaptchaAsync(_model.Mobile);
            await Message.Success($"获取验证码成功！验证码为：{captcha}");
        }
    }
}