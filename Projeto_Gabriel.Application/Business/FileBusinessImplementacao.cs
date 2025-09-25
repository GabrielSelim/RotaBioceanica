using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Domain.RepositoryInterface;

namespace Projeto_Gabriel.Bussines.Implementacoes
{
    public class FileBusinessImplementacao : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;
        private readonly ICartaPokemonRepository _cartaPokemonRepository;

        public FileBusinessImplementacao(IHttpContextAccessor context, ICartaPokemonRepository cartaPokemonRepository)
        {
            _context = context;
            _cartaPokemonRepository = cartaPokemonRepository;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadDir");


            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public byte[] GetFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Nome do arquivo inválido");
            }

            var filePath = Path.Combine(_basePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Arquivo não encontrado");
            }

            return File.ReadAllBytes(filePath);
        }

        public Task<List<FIleDetailDbo>> SaveFilesToDisk(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

            List<FIleDetailDbo> fileDetails = new List<FIleDetailDbo>();
            foreach (var item in files)
            {
                var fileDetail = SaveFileToDisk(item).Result;
                fileDetails.Add(fileDetail);
            }

            return Task.FromResult(fileDetails);
        }

        public async Task<FIleDetailDbo> SaveFileToDisk(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Arquivo não enviado ou está vazio.");
            }

            FIleDetailDbo fileDetail = new FIleDetailDbo();
            var fileType = Path.GetExtension(file.FileName);
            var baseUrl = _context?.HttpContext?.Request?.Host.Value;

            var validFileTypes = new[] { ".jpg", ".png", ".jpeg", ".pdf" };
            if (!validFileTypes.Contains(fileType, StringComparer.OrdinalIgnoreCase))
            {
                throw new Exception("Tipo de arquivo não suportado");
            }

            var docName = Path.GetFileName(file.FileName);
            var destination = Path.Combine(_basePath, docName);
            fileDetail.DocumentName = docName;
            fileDetail.DocType = fileType.Equals(".pdf", StringComparison.OrdinalIgnoreCase) ? "pdf" : "image";
            fileDetail.DocUrl = $"{baseUrl}/v1/api/file/{fileDetail.DocumentName}";

            using (var stream = new FileStream(destination, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileDetail;
        }

        public string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream",
            };
        }

        public List<FIleDetailDbo> ListFiles()
        {
            if (!Directory.Exists(_basePath))
            {
                throw new DirectoryNotFoundException("Diretório não encontrado");
            }

            var files = Directory.GetFiles(_basePath)
                .Select(filePath => new FIleDetailDbo
                {
                    DocumentName = Path.GetFileName(filePath),
                    DocType = GetContentType(filePath).Split('/')[0],
                    SizeInMb = (new FileInfo(filePath).Length / (1024.0 * 1024.0)).ToString("F2") + " MB"
                })
                .ToList();

            return files;
        }

        public async Task<List<CartaPokemonDbo>> SaveImagesToDatabase(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

            try
            {
                var validFileTypes = new[] { ".jpg", ".png", ".jpeg" };
                foreach (var file in files)
                {
                    var fileType = Path.GetExtension(file.FileName);
                    if (!validFileTypes.Contains(fileType, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new Exception($"Tipo de arquivo não suportado: {fileType}. Apenas imagens (.jpg, .png, .jpeg) são permitidas.");
                    }

                    if (file.Length == 0)
                    {
                        throw new ArgumentException($"O arquivo {file.FileName} está vazio.");
                    }
                }

                // Chama o repositório para salvar as imagens
                var cartas = await _cartaPokemonRepository.SaveImagesToDatabase(files);

                // Converte de CartaPokemon para CartaPokemonDbo
                return cartas.Select(carta => new CartaPokemonDbo
                {
                    Id = carta.Id,
                    NomeVersao = carta.NomeVersao,
                    Versao = carta.Versao,
                    NumeroCarta = carta.NumeroCarta,
                    NomePokemon = carta.NomePokemon,
                    Raridade = carta.Raridade,
                    Tipo = carta.Tipo,
                    HP = carta.HP,
                    Estagio = carta.Estagio,
                    Booster = carta.Booster,
                    Imagem = carta.Imagem
                }).ToList();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar imagens no banco de dados: {ex.Message}", ex);
            }
        }
    }
}
