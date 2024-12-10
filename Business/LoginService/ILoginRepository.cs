using System.Threading.Tasks;

namespace CampingApplication.Business.LoginService
{
    public interface ILoginRepository
    {
        Task<bool> ValidateUserAsync(string username, string password);
    }
}
