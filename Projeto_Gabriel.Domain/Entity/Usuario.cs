using Projeto_Gabriel.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Gabriel.Domain.Entity
{
    [Table("usuarios")]
    public class Usuario : BaseEntity
    {
        #region Informações Pessoais e Contato

        [Column("full_name")]
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(200, MinimumLength = 3)]
        public string NomeCompleto { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(150)]
        public string Email { get; set; }

        [Column("document")]
        [StringLength(20)]
        public string? Documento { get; set; }

        [Column("country")]
        [Required(ErrorMessage = "O País é obrigatório.")]
        [StringLength(100)]
        public string Pais { get; set; }

        [Column("city_state")]
        [Required(ErrorMessage = "A Cidade/Estado é obrigatória.")]
        [StringLength(150)]
        public string CidadeEstado { get; set; }

        [Column("phone_country_code")]
        [Required(ErrorMessage = "O DDI é obrigatório.")]
        [StringLength(5)] // Ex: "+55"
        public string DDI { get; set; }

        [Column("phone_number")]
        [Required(ErrorMessage = "O número de telefone é obrigatório.")]
        [StringLength(20)] // Ex: "(67) 99999-9999"
        public string TelefoneNumero { get; set; }

        #endregion

        #region Informações Profissionais

        [Column("company_institution")]
        [StringLength(150)]
        public string? EmpresaInstituicao { get; set; }

        [Column("job_title")]
        [StringLength(100)]
        public string? Cargo { get; set; }

        [Column("sector")]
        [StringLength(100)]
        public string? SetorAtuacao { get; set; }

        [Column("company_website")]
        [StringLength(200)]
        public string? WebsiteEmpresa { get; set; }

        #endregion

        #region Credenciais e Segurança

        [Column("password_hash")]
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string SenhaHash { get; set; }

        [Column("role")]
        [StringLength(30)]
        public string Role { get; set; } = "Participante";

        [Column("refresh_token")]
        [StringLength(500)]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenTempoExpiracao { get; set; }

        #endregion

        #region Consentimento e Preferências (LGPD)

        [Column("terms_accepted_at")]
        [Required]
        public DateTime TermosAceitosEm { get; set; }

        [Column("consents_to_whatsapp_group")]
        public bool ConsentiuEntrarGrupoWhatsApp { get; set; }

        [Column("authorizes_image_use")]
        public bool AutorizaUsoImagem { get; set; } = false;

        [Column("wishes_to_receive_communications")]
        public bool DesejaReceberComunicados { get; set; } = false;

        #endregion

        [Column("password")]
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }
    }
}