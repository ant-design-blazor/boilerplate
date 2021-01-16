using System.Threading.Tasks;

namespace AntDesign.Boilerplate.Shared
{
    public interface IIdentityContext
    {
        long? UserId { get; }

        string UserName { get; }

        string NickName { get; }

        string Avatar { get; }

        Task GetState();
    }
}