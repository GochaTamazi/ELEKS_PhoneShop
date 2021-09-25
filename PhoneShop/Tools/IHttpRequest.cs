using System.Threading.Tasks;

namespace PhoneShop.Tools
{
    public interface IHttpRequest
    {
        Task<string> GetAsync(string url);
    }
}