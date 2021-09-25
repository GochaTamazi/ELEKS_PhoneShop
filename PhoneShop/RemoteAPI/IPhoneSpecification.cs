using System.Threading.Tasks;

namespace PhoneShop.RemoteAPI
{
    /// <summary>
    /// Rest Api for Phone specifications
    /// https://github.com/azharimm/phone-specs-api
    /// </summary>
    public interface IPhoneSpecification
    {
        Task<string> ListBrandsAsync();
        Task<string> ListPhonesAsync(string brandSlug, int page = 1);
        Task<string> PhoneSpecificationsAsync(string phoneSlug);
        Task<string> SearchAsync(string query);
        Task<string> LatestAsync();
        Task<string> TopByInterestAsync();
        Task<string> TopByFansAsync();
    }
}