using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Application.Converter.Implementacao;
using Projeto_Gabriel.Application.Dto;
using Projeto_Gabriel.Application.Exceptions;
using Projeto_Gabriel.Domain.RepositoryInterface;

namespace Projeto_Gabriel.Bussines.Implementacoes
{
    public class PokemonFileBusinessImplementacao : IPokemonFileBusiness
    {
        private readonly ICartaPokemonRepository _cartaPokemonRepository;
        private readonly CartaPokemonConverter _converter;

        public PokemonFileBusinessImplementacao(ICartaPokemonRepository cartaPokemonRepository)
        {
            _cartaPokemonRepository = cartaPokemonRepository;
            _converter = new CartaPokemonConverter();
        }

        public async Task<List<CartaPokemonDbo>> SavePokemonImagesToDatabase(List<IFormFile> files)
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

                var cartas = await _cartaPokemonRepository.SaveImagesToDatabase(files);
                if (cartas == null || cartas.Count == 0)
                {
                    throw new Exception("Nenhuma carta foi salva no banco de dados.");
                }

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

        public List<CartaPokemonDbo> FiltrarPorCriterios(string? nome = null, string? tipo = null, string? raridade = null, string? estagio = null, string? versao = null, string? booster = null, string? sortField = null, string? sortDirection = "asc")
        {
            try
            {
                var cartas = _cartaPokemonRepository.FiltrarPorCriterios(nome, tipo, raridade, estagio, versao, booster, sortField, sortDirection);

                if (cartas == null || !cartas.Any())
                {
                    throw new Exception("Nenhuma carta encontrada com os critérios fornecidos.");
                }

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
                    Imagem = carta.Imagem,
                    Situacao = carta.Situacao
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao filtrar cartas: {ex.Message}", ex);
            }
        }

        public void Deletar(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID inválido.");
                }

                _cartaPokemonRepository.Deletar(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new NotFoundException($"Erro ao Deletar carta Id inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao Deletar carta: {ex.Message}", ex);
            }
        }

        public List<CartaPokemonDbo> ObterTodosSemImagem()
        {
            try
            {
                var cartas = _cartaPokemonRepository.ObterTodos();

                if (cartas == null || !cartas.Any())
                {
                    throw new NotFoundException("Nenhuma carta encontrada.");
                }

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
                    Situacao = carta.Situacao
                }).ToList();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException("Nenhuma carta encontrada.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter cartas: {ex.Message}", ex);
            }
        }

        public void AtivarCarta(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID inválido.");
                }

                _cartaPokemonRepository.AtivarCarta(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Erro ao ativar a carta: {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Erro ao ativar a carta: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao ativar a carta: {ex.Message}", ex);
            }
        }

        public void InativarCarta(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID inválido.");
                }

                _cartaPokemonRepository.InativarCarta(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Erro ao inativar a carta: {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Erro ao inativar a carta: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inativar a carta: {ex.Message}", ex);
            }
        }

        public CartaPokemonDbo ObterPorId(long id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID inválido.");
                }

                var carta = _cartaPokemonRepository.ObterPorId(id);

                if (carta == null)
                {
                    throw new KeyNotFoundException($"Carta com ID {id} não encontrada.");
                }

                return new CartaPokemonDbo
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
                    Imagem = carta.Imagem,
                    Situacao = carta.Situacao
                };
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Erro ao buscar carta: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar carta: {ex.Message}", ex);
            }
        }

        public CartaPokemonDbo Atualizar(CartaPokemonDbo carta)
        {
            try
            {
                if (carta == null)
                {
                    throw new ArgumentNullException(nameof(carta), "Carta não pode ser nula.");
                }
                if (carta.Id <= 0)
                {
                    throw new ArgumentException("ID inválido.");
                }
                if (string.IsNullOrEmpty(carta.NomePokemon))
                {
                    throw new ArgumentException("Nome do Pokémon não pode ser nulo ou vazio.");
                }
                if (string.IsNullOrEmpty(carta.Tipo))
                {
                    throw new ArgumentException("Tipo não pode ser nulo ou vazio.");
                }
                if (string.IsNullOrEmpty(carta.Raridade))
                {
                    throw new ArgumentException("Raridade não pode ser nula ou vazia.");
                }
                if (string.IsNullOrEmpty(carta.Estagio))
                {
                    throw new ArgumentException("Estágio não pode ser nulo ou vazio.");
                }
                if (string.IsNullOrEmpty(carta.Booster))
                {
                    throw new ArgumentException("Booster não pode ser nulo ou vazio.");
                }
                if (string.IsNullOrEmpty(carta.NomeVersao))
                {
                    throw new ArgumentException("Nome da versão não pode ser nulo ou vazio.");
                }
                if (string.IsNullOrEmpty(carta.Versao))
                {
                    throw new ArgumentException("Versão não pode ser nula ou vazia.");
                }
                if (string.IsNullOrEmpty(carta.Situacao))
                {
                    throw new ArgumentException("Situação não pode ser nula ou vazia.");
                }
                if (carta.NumeroCarta <= 0)
                {
                    throw new ArgumentException("Número da carta deve ser maior que zero.");
                }
                if (carta.HP < 0)
                {
                    throw new ArgumentException("HP não pode ser negativo.");
                }

                var pokemonEntity = _converter.Parse(carta);

                var cartaAtualizada = _cartaPokemonRepository.Atualizar(pokemonEntity);

                if (cartaAtualizada == null)
                {
                    throw new Exception("Erro ao atualizar a carta.");
                }
                return new CartaPokemonDbo
                {
                    Id = cartaAtualizada.Id,
                    NomeVersao = cartaAtualizada.NomeVersao,
                    Versao = cartaAtualizada.Versao,
                    NumeroCarta = cartaAtualizada.NumeroCarta,
                    NomePokemon = cartaAtualizada.NomePokemon,
                    Raridade = cartaAtualizada.Raridade,
                    Tipo = cartaAtualizada.Tipo,
                    HP = cartaAtualizada.HP,
                    Estagio = cartaAtualizada.Estagio,
                    Booster = cartaAtualizada.Booster,
                    Imagem = cartaAtualizada.Imagem,
                    Situacao = cartaAtualizada.Situacao
                };
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar a carta: {ex.Message}", ex);
            }
        }
    }
}
