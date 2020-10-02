using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookStore.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsyc(IFormFile imageFile, string folder);
    }
}
