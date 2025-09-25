import React, { useState } from "react";
import "./style.css";
import "bootstrap/dist/css/bootstrap.min.css";
import api from "../../services/api"; // Adicione este import para usar o mesmo padrão de requisição

const socialLinks = [
  {
    name: "LinkedIn",
    url: "https://www.linkedin.com/in/gabriel-sanz/",
    icon: "bi-linkedin"
  },
  {
    name: "GitHub",
    url: "https://github.com/GabrielSelim",
    icon: "bi-github"
  }
];

const textos = {
  titulo: {
    pt: "Entre em Contato!",
    en: "Get in Touch!"
  },
  intro: {
    pt: "Tem alguma pergunta, projeto em mente ou apenas quer bater um papo? Envie-me uma mensagem!",
    en: "Have a question, a project in mind, or just want to chat? Send me a message!"
  },
  nome: {
    pt: "Seu Nome",
    en: "Your Name"
  },
  nomePlaceholder: {
    pt: "Seu nome completo",
    en: "Your full name"
  },
  email: {
    pt: "Seu Email",
    en: "Your Email"
  },
  emailPlaceholder: {
    pt: "email@exemplo.com",
    en: "email@example.com"
  },
  mensagem: {
    pt: "Mensagem",
    en: "Message"
  },
  mensagemPlaceholder: {
    pt: "Digite sua mensagem aqui...",
    en: "Type your message here..."
  },
  enviar: {
    pt: "Enviar Mensagem",
    en: "Send Message"
  },
  campoObrigatorio: {
    pt: "Campo obrigatório",
    en: "Required field"
  },
  emailInvalido: {
    pt: "Formato de e-mail inválido",
    en: "Invalid email format"
  },
  sucesso: {
    pt: "Mensagem enviada com sucesso! Em breve entrarei em contato.",
    en: "Message sent successfully! I will contact you soon."
  },
  erro: {
    pt: "Ocorreu um erro ao enviar sua mensagem. Por favor, tente novamente.",
    en: "An error occurred while sending your message. Please try again."
  },
  emailDireto: {
    pt: "E-mail direto:",
    en: "Direct email:"
  }
};

export default function Contato({ language = "pt" }) {
  const [form, setForm] = useState({ nome: "", email: "", mensagem: "" });
  const [errors, setErrors] = useState({});
  const [status, setStatus] = useState("idle"); // idle | sending | success | error

  // Validação em tempo real
  const validate = (field, value) => {
    let error = "";
    if (!value) {
      error = textos.campoObrigatorio[language];
    } else if (field === "email") {
      // Regex simples para e-mail
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(value)) {
        error = textos.emailInvalido[language];
      }
    }
    return error;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    setErrors((prev) => ({ ...prev, [name]: validate(name, value) }));
  };

  const handleBlur = (e) => {
    const { name, value } = e.target;
    setErrors((prev) => ({ ...prev, [name]: validate(name, value) }));
  };

  const validateAll = () => {
    const newErrors = {};
    Object.keys(form).forEach((field) => {
      const error = validate(field, form[field]);
      if (error) newErrors[field] = error;
    });
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Altere aqui para usar o mesmo padrão de requisição do projeto (api.post)
  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateAll()) return;
    setStatus("sending");
    try {
      await api.post("/v1/api/contato", form);
      setStatus("success");
      setForm({ nome: "", email: "", mensagem: "" });
      setErrors({});
    } catch {
      setStatus("error");
    }
  };

  return (
    <div className="contato-bg">
      <div className="contato-card shadow-lg">
        <h2 className="contato-title mb-2">{textos.titulo[language]}</h2>
        <p className="contato-intro mb-4">{textos.intro[language]}</p>
        <form className="contato-form" onSubmit={handleSubmit} noValidate>
          <div className="mb-3">
            <label htmlFor="nome" className="form-label">{textos.nome[language]}</label>
            <input
              type="text"
              className={`form-control ${errors.nome ? "is-invalid" : ""}`}
              id="nome"
              name="nome"
              placeholder={textos.nomePlaceholder[language]}
              value={form.nome}
              onChange={handleChange}
              onBlur={handleBlur}
              required
              autoComplete="off"
            />
            {errors.nome && <div className="invalid-feedback">{errors.nome}</div>}
          </div>
          <div className="mb-3">
            <label htmlFor="email" className="form-label">{textos.email[language]}</label>
            <input
              type="email"
              className={`form-control ${errors.email ? "is-invalid" : ""}`}
              id="email"
              name="email"
              placeholder={textos.emailPlaceholder[language]}
              value={form.email}
              onChange={handleChange}
              onBlur={handleBlur}
              required
              autoComplete="off"
            />
            {errors.email && <div className="invalid-feedback">{errors.email}</div>}
          </div>
          <div className="mb-3">
            <label htmlFor="mensagem" className="form-label">{textos.mensagem[language]}</label>
            <textarea
              className={`form-control ${errors.mensagem ? "is-invalid" : ""}`}
              id="mensagem"
              name="mensagem"
              placeholder={textos.mensagemPlaceholder[language]}
              rows={5}
              value={form.mensagem}
              onChange={handleChange}
              onBlur={handleBlur}
              required
              style={{ resize: "vertical", minHeight: 100, maxHeight: 300 }}
            />
            {errors.mensagem && <div className="invalid-feedback">{errors.mensagem}</div>}
          </div>
          <button
            type="submit"
            className="btn btn-gradient btn-lg w-100"
            disabled={status === "sending"}
          >
            {status === "sending" ? (
              <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
            ) : null}
            {textos.enviar[language]}
          </button>
        </form>
        {status === "success" && (
          <div className="alert alert-success mt-4" role="alert">
            {textos.sucesso[language]}
          </div>
        )}
        {status === "error" && (
          <div className="alert alert-danger mt-4" role="alert">
            {textos.erro[language]}
          </div>
        )}
        <div className="contato-extra mt-4">
          <div className="contato-social mb-2">
            {socialLinks.map((s) => (
              <a
                key={s.name}
                href={s.url}
                target="_blank"
                rel="noopener noreferrer"
                className="contato-social-link"
                aria-label={s.name}
              >
                <i className={`bi ${s.icon}`}></i>
              </a>
            ))}
          </div>
          <div className="contato-email-info">
            <span className="contato-email-label">{textos.emailDireto[language]} </span>
            <span className="contato-email-value">eng.gabrielsanz@hotmail.com</span>
          </div>
        </div>
      </div>
    </div>
  );
}