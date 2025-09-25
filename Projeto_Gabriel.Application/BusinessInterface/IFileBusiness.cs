using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Application.Dto;

namespace Projeto_Gabriel.Bussines
{
    public interface IFileBusiness
    {
        public byte[] GetFile(string fileName);

        public Task<FIleDetailDbo> SaveFileToDisk(IFormFile file);

        public Task<List<FIleDetailDbo>> SaveFilesToDisk(List<IFormFile> file);

        public string GetContentType(string fileName);

        public List<FIleDetailDbo> ListFiles();

        Task<List<CartaPokemonDbo>> SaveImagesToDatabase(List<IFormFile> files);
    }
}
