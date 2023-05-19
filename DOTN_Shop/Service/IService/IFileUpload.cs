using Microsoft.AspNetCore.Components.Forms;

namespace DOTN_Shop.Service.IService
{
    public interface IFileUpload
    {
        Task<string> UploadFile(IBrowserFile file);
        bool DeleteFile(string filePath);

    }
}
