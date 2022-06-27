using ConvertData.Domain.Response;
using Microsoft.AspNetCore.Http;

namespace ConvertData.Domain.Interfaces
{
    public interface IGenerateFile
    {
        Task<StateResponse> FileSplitWriter(IFormFile file);
    }
}
