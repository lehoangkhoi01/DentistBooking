using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DentistBookingWebApp.Utils.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
