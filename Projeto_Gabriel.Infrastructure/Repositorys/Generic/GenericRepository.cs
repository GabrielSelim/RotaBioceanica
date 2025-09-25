using Microsoft.EntityFrameworkCore;
using Projeto_Gabriel.Domain.Entity.Base;
using Projeto_Gabriel.Domain.Entity.Validations;
using Projeto_Gabriel.Domain.ServiceInterface;
using Projeto_Gabriel.Infrastructure.Services.Validation;
using Projeto_Gabriel.Model.Context;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected MySQLContext _context;
        private DbSet<T> dataset;
        private readonly IEntityValidationService<T> _validationService;

        public GenericRepository(MySQLContext context)
        {
            _validationService = ValidationServiceFactory.GetValidationService<T>();
            _context = context;
            dataset = _context.Set<T>();
        }

        public T ObterPorId(long id)
        {
            return dataset.FirstOrDefault(p => p.Id.Equals(id));
        }

        public List<T> ObterTodos()
        {
            return dataset.ToList();
        }

        public virtual T Criar(T item)
        {
            try
            {
                if (item is IEntityValidator validator)
                {
                    validator.Validate();
                }
                else
                {
                    _validationService.Validate(item);
                }

                dataset.Add(item);
                _context.SaveChanges();

                return item;
            }
            catch (ValidationException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar a entidade no banco de dados.", ex);
            }
        }

        public T Atualizar(T item)
        {
            if (!Existe(item.Id))
                throw new ArgumentException("A entidade com o ID especificado não existe.");

            var result = dataset.FirstOrDefault(p => p.Id.Equals(item.Id));

            if (result == null)
                throw new ArgumentException("A entidade com o ID especificado não foi encontrada.");

            try
            {
                _validationService.Validate(item);

                _context.Entry(result).CurrentValues.SetValues(item);

                _context.SaveChanges();

                return item;
            }
            catch (ValidationException ex)
            {
                throw new ArgumentException($"Erro de validação: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a entidade no banco de dados.", ex);
            }
        }

        public void Deletar(long id)
        {
            var result = dataset.FirstOrDefault(p => p.Id.Equals(id));

            if (result == null)
            {
                throw new KeyNotFoundException("Entidade não encontrada para o ID especificado.");
            }

            try
            {
                dataset.Remove(result);
                _context.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Existe(long id)
        {
            return dataset.Any(p => p.Id.Equals(id));
        }

        public List<T> ObterComPaginacao(int pageNumber, int pageSize, string sortDirection)
        {
            var query = dataset.AsQueryable();

            if (sortDirection == "asc")
            {
                query = query.OrderBy(e => e.Id);
            }
            else if (sortDirection == "desc")
            {
                query = query.OrderByDescending(e => e.Id);
            }            

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetCount()
        {
            return dataset.Count();
        }
    }
}