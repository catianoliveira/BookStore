using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookStore.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imagefile, string folder);
    }
}
