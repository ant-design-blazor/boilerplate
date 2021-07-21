using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Pro.Layout;
using AntDesign.Boilerplate.Client.Models;
using Microsoft.AspNetCore.Components;
using AntDesign.Boilerplate.Shared;
using AntDesign.Boilerplate.Client.Services;

namespace AntDesign.Boilerplate.Client.Components
{
    public partial class RightContent
    {
        private CurrentUser _currentUser = new CurrentUser();
        private NoticeIconData[] _notifications = { };
        private NoticeIconData[] _messages = { };
        private NoticeIconData[] _events = { };
        private int _count = 0;

        private List<AutoCompleteDataItem<string>> DefaultOptions { get; set; } = new List<AutoCompleteDataItem<string>>
        {
            new AutoCompleteDataItem<string>
            {
                Label = "umi ui",
                Value = "umi ui"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Table",
                Value = "Pro Table"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Layout",
                Value = "Pro Layout"
            }
        };

        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Inject] protected IIdentityContext IdentityContext { get; set; }

        [Inject] protected MessageService MessageService { get; set; }

        [Inject] protected IAccountService AccountService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SetClassMap();
            await IdentityContext.GetState();

            _currentUser = new CurrentUser
            {
                Avatar = IdentityContext.Avatar ?? "https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png",
                Name = IdentityContext.UserName,
            };
        }

        protected void SetClassMap()
        {
            ClassMapper
                .Clear()
                .Add("right");
        }

        public async Task HandleSelectUser(MenuItem item)
        {
            switch (item.Key)
            {
                case "center":
                    NavigationManager.NavigateTo("/account/center");
                    break;

                case "setting":
                    NavigationManager.NavigateTo("/account/settings");
                    break;

                case "logout":
                    await AccountService.LogoutAsync();
                    NavigationManager.NavigateTo("/user/login");
                    break;
            }
        }

        public void HandleSelectLang(MenuItem item)
        {
        }

        public async Task HandleClear(string key)
        {
            switch (key)
            {
                case "notification":
                    _notifications = new NoticeIconData[] { };
                    break;

                case "message":
                    _messages = new NoticeIconData[] { };
                    break;

                case "event":
                    _events = new NoticeIconData[] { };
                    break;
            }
            await MessageService.Success($"清空了{key}");
        }

        public async Task HandleViewMore(string key)
        {
            await MessageService.Info("Click on view more");
        }
    }
}