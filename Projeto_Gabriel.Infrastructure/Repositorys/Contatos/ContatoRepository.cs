using Projeto_Gabriel.Domain.Entity.Contatos;
using Projeto_Gabriel.Domain.RepositoryInterface.Contatos;
using Projeto_Gabriel.Domain.ServiceInterface;
using Projeto_Gabriel.Infrastructure.Services.Validation;
using Projeto_Gabriel.Model.Context;
using Projeto_Gabriel.Repository.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Gabriel.Infrastructure.Repositorys.Contatos
{
    public class ContatoRepository : GenericRepository<Contato>, IContatoRepository
    {
        private readonly IEntityValidationService<Contato> _validationService;

        public ContatoRepository(MySQLContext context) : base(context)
        {
            _validationService = ValidationServiceFactory.GetValidationService<Contato>();
        }

        public async Task SalvarContatoAsync(Contato contato)
        {
            try
            {
                _validationService.Validate(contato);

                await _context.Contatos.AddAsync(contato);
                await _context.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException(ex.Message);
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
    }
}