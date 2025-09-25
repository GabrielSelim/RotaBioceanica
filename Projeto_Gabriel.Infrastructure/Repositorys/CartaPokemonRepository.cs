using Microsoft.AspNetCore.Http;
using Projeto_Gabriel.Domain.Entity;
using Projeto_Gabriel.Domain.RepositoryInterface;
using Projeto_Gabriel.Infrastructure.Services.Validation;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Infrastructure.Repositorys
{
    public class CartaPokemonRepository : GenericRepository<CartaPokemon>, ICartaPokemonRepository
    {

        public CartaPokemonRepository(MySQLContext context) : base(context)
        {
        }

        public List<CartaPokemon> FiltrarPorCriterios(string? nome = null, string? tipo = null, string? raridade = null, string? estagio = null, string? versao = null, string? booster = null, string? sortField = null, string? sortDirection = "asc")
        {
            var query = _context.CartasPokemon.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(c => c.NomePokemon.Contains(nome));

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(c => c.Tipo == tipo);

            if (!string.IsNullOrEmpty(raridade))
                query = query.Where(c => c.Raridade == raridade);

            if (!string.IsNullOrEmpty(estagio))
                query = query.Where(c => c.Estagio == estagio);

            if (!string.IsNullOrEmpty(versao))
                query = query.Where(c => c.Versao == versao);

            if (!string.IsNullOrEmpty(booster))
                query = query.Where(c => c.Booster == booster);

            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortDirection?.ToLower() == "desc")
                {
                    query = sortField.ToLower() switch
                    {
                        "nomepokemon" => query.OrderByDescending(c => c.NomePokemon),
                        "tipo" => query.OrderByDescending(c => c.Tipo),
                        "raridade" => query.OrderByDescending(c => c.Raridade),
                        "estagio" => query.OrderByDescending(c => c.Estagio),
                        "versao" => query.OrderByDescending(c => c.Versao),
                        "booster" => query.OrderByDescending(c => c.Booster),
                        _ => query.OrderByDescending(c => c.Id)
                    };
                }
                else
                {
                    query = sortField.ToLower() switch
                    {
                        "nomepokemon" => query.OrderBy(c => c.NomePokemon),
                        "tipo" => query.OrderBy(c => c.Tipo),
                        "raridade" => query.OrderBy(c => c.Raridade),
                        "estagio" => query.OrderBy(c => c.Estagio),
                        "versao" => query.OrderBy(c => c.Versao),
                        "booster" => query.OrderBy(c => c.Booster),
                        _ => query.OrderBy(c => c.Id)
                    };
                }
            }

            return query.ToList();
        }

        public async Task<List<CartaPokemon>> SaveImagesToDatabase(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("Nenhum arquivo enviado.");
            }

            var validationService = new CartaPokemonValidationService();
            List<CartaPokemon> cartasSalvas = new List<CartaPokemon>();

            foreach (var file in files)
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var parts = fileName.Split('_');
                    if (parts.Length < 8)
                    {
                        throw new Exception($"Nome do arquivo inválido: {fileName}. Certifique-se de que segue o formato esperado.");
                    }

                    var nomeVersao = parts[0];
                    int index = 1;
                    while (index < parts.Length - 8)
                    {
                        nomeVersao += $" {parts[index]}";
                        index++;
                    }

                    var versao = parts[index++];
                    var numeroCarta = int.Parse(parts[index++]);
                    var nomePokemon = parts[index++];
                    var raridade = parts[index++];
                    var tipo = parts[index++];
                    var hp = int.TryParse(parts[index++], out var parsedHp) ? parsedHp : (int?)null;
                    var estagio = parts[index++];
                    var booster = parts[index++];

                    var cartaPokemon = new CartaPokemon
                    {
                        NomeVersao = nomeVersao,
                        Versao = versao,
                        NumeroCarta = numeroCarta,
                        NomePokemon = nomePokemon,
                        Raridade = raridade,
                        Tipo = tipo,
                        HP = hp,
                        Estagio = estagio,
                        Booster = booster
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        cartaPokemon.Imagem = memoryStream.ToArray();
                    }

                    // Valida a entidade antes de salvar
                    validationService.Validate(cartaPokemon);

                    _context.CartasPokemon.Add(cartaPokemon);
                    await _context.SaveChangesAsync();

                    cartasSalvas.Add(cartaPokemon);
                }
                catch (ValidationException ex)
                {
                    throw new ArgumentException($"Erro de validação: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao processar o arquivo {file.FileName}: {ex.Message}");
                }
            }

            return cartasSalvas;
        }

        public void AtivarCarta(long id)
        {
            var carta = _context.CartasPokemon.FirstOrDefault(c => c.Id == id);

            if (carta == null)
            {
                throw new KeyNotFoundException($"Carta com ID {id} não encontrada.");
            }

            if (carta.Situacao == "A")
            {
                throw new InvalidOperationException($"A carta com ID {id} já está ativa.");
            }

            carta.Situacao = "A";
            _context.CartasPokemon.Update(carta);
            _context.SaveChanges();
        }

        public void InativarCarta(long id)
        {
            var carta = _context.CartasPokemon.FirstOrDefault(c => c.Id == id);

            if (carta == null)
            {
                throw new KeyNotFoundException($"Carta com ID {id} não encontrada.");
            }

            if (carta.Situacao == "I")
            {
                throw new InvalidOperationException($"A carta com ID {id} já está inativa.");
            }

            carta.Situacao = "I";
            _context.CartasPokemon.Update(carta);
            _context.SaveChanges();
        }
    }
}