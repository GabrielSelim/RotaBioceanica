import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./style.css";
import avatar from '../../assets/avatar.jpeg';
import csharpLogo from '../../assets/csharp.png';
import dockerLogo from '../../assets/docker.png';
import htmlLogo from '../../assets/HTML5.png';
import mysqlLogo from '../../assets/mysql.png';
import sqlserverLogo from '../../assets/sqlservericon.png';
import reactLogo from '../../assets/reactjs.png';
import angularLogo from '../../assets/angular.png';
import swaggerLogo from '../../assets/swagger.png';
import restfulLogo from '../../assets/restful.png';
import digitaloceanLogo from '../../assets/digitalocean.png';
import rbacLogo from '../../assets/RBAC.png';
import javascriptLogo from '../../assets/javascript.png';
import razorLogo from '../../assets/razor.png';
import metodologiaAgilLogo from '../../assets/metodologiaagil.png';
import githubLogo from '../../assets/github.png';
import cartasThumb from '../../assets/ProjetoCartas/Cartas_Distribuidas.png';
import pokemonThumb from '../../assets/ProjetoPokemon/SistemaDeControlePokemon.png';
import livrosThumb from '../../assets/ProjetoLivros/SistemaListarLivros.png';
import painelFinanceiroThumb from '../../assets/ProjetoFinanceiro/PainelFinanceiro_1.png';

// Importe todas as imagens dos projetos
const projetoCartasImgs = [
  require('../../assets/ProjetoCartas/Cartas_Distribuidas.png'),
  require('../../assets/ProjetoCartas/Inicio.png'),
  require('../../assets/ProjetoCartas/Resultado_Comparacao_Cartas.png'),
  require('../../assets/ProjetoCartas/Tela_Distribuir.png'),
  require('../../assets/ProjetoCartas/Validação.png'),
];
const projetoPokemonImgs = [
  require('../../assets/ProjetoPokemon/SistemaDeControlePokemon.png'),
  require('../../assets/ProjetoPokemon/SistemaDeControlePokemonAPI.png'),
  require('../../assets/ProjetoPokemon/SistemaDeControlePokemonInativas.png'),
  require('../../assets/ProjetoPokemon/IncluirNovaCartaPokemon.png'),
  require('../../assets/ProjetoPokemon/SistemaDeControlePokemonDados.png'),
];
const projetoLivrosImgs = [
  require('../../assets/ProjetoLivros/SistemaCadastroLivros.png'),
  require('../../assets/ProjetoLivros/SistemaListarLivros.png'),
  require('../../assets/ProjetoLivros/SistemLivrosAPI.png'),
];
const projetoFinanceiroImgs = [
  require('../../assets/ProjetoFinanceiro/PainelFinanceiro_1.png'),
  require('../../assets/ProjetoFinanceiro/Cadastrar_Cartao_2.png'),
  require('../../assets/ProjetoFinanceiro/Cadastrar_Pessoa_3.png'),
  require('../../assets/ProjetoFinanceiro/Cadastrar_Categoria_4.png'),
  require('../../assets/ProjetoFinanceiro/Lancamento_Financeiro_5.png'),
  require('../../assets/ProjetoFinanceiro/Adicionar_Lancamento_Financeiro_6.png'),
  require('../../assets/ProjetoFinanceiro/Parcelamentos_7.png'),
  require('../../assets/ProjetoFinanceiro/Cadastrar_Parcelamento_8.png'),
  require('../../assets/ProjetoFinanceiro/Parcelas_Do_Parcelamento.png'),
];

// Projeto de Controle Financeiro destacado
const projetos = [
  {
    nome: "Projeto de Controle Financeiro",
    nomeEn: "Financial Control Project",
    thumb: painelFinanceiroThumb,
    imagens: projetoFinanceiroImgs,
    destaque: true,
    texto: {
      pt: (
        <>
          <b>O Projeto de Controle Financeiro</b> é o maior e mais completo sistema do meu portfólio até o momento.<br /><br />
          Ele foi desenvolvido para permitir o controle total das finanças pessoais ou familiares, com recursos como:
          <ul style={{ marginTop: 8, marginBottom: 8 }}>
            <li>Cadastro e gerenciamento de cartões, contas e pessoas</li>
            <li>Criação de categorias financeiras personalizadas</li>
            <li>Lançamento de receitas e despesas, com controle de situação (pago, a pagar, recebido, a receber)</li>
            <li>Gestão de parcelamentos, acompanhamento de parcelas e status</li>
            <li>Painel intuitivo, visual moderno e responsivo</li>
            <li>API RESTful robusta, autenticação JWT, RBAC e deploy em Docker</li>
          </ul>
          <b>Destaque:</b> Este projeto demonstra integração completa entre front-end React, back-end .NET 8, banco de dados MySQL, autenticação segura e arquitetura escalável.<br />
          <span style={{ color: "#00c6ff" }}>É o projeto ideal para mostrar minha experiência em sistemas reais e complexos!</span>
        </>
      ),
      en: (
        <>
          <b>The Financial Control Project</b> is the largest and most complete system in my portfolio so far.<br /><br />
          It was developed to allow full control of personal or family finances, with features such as:
          <ul style={{ marginTop: 8, marginBottom: 8 }}>
            <li>Registration and management of cards, accounts, and people</li>
            <li>Creation of custom financial categories</li>
            <li>Recording income and expenses, with status control (paid, to pay, received, to receive)</li>
            <li>Installment management, tracking installments and status</li>
            <li>Intuitive dashboard, modern and responsive design</li>
            <li>Robust RESTful API, JWT authentication, RBAC, and Docker deployment</li>
          </ul>
          <b>Highlight:</b> This project demonstrates full integration between React front-end, .NET 8 back-end, MySQL database, secure authentication, and scalable architecture.<br />
          <span style={{ color: "#00c6ff" }}>It is the ideal project to showcase my experience with real and complex systems!</span>
        </>
      )
    },
    github: "https://github.com/GabrielSelim/Projeto_Gabriel"
  },
  {
    nome: "Projeto Jogo de Cartas",
    nomeEn: "Card Game Project",
    thumb: cartasThumb,
    imagens: projetoCartasImgs,
    texto: {
      pt: "Este projeto foi criado para testar meus conhecimentos utilizando .NET 8 com Angular, integrando API com Front-end. Trata-se de um jogo de cartas em que minha API consome dados de terceiros e o objetivo é comparar cartas para ver quem possui a de maior valor.",
      en: "This project was created to test my skills using .NET 8 with Angular, integrating API with Front-end. It is a card game where my API consumes data from third-party APIs and the goal is to compare cards to see who has the highest value card."
    },
    github: "https://github.com/GabrielSelim/Jogo_Cartas"
  },
  {
    nome: "Projeto Controle de Cartas Pokémon",
    nomeEn: "Pokémon Card Control Project",
    thumb: pokemonThumb,
    imagens: projetoPokemonImgs,
    texto: {
      pt: "Este projeto foi criado para suprir uma necessidade pessoal em um jogo de cartas: visualizar quais cartas eu ainda não tinha e, ao obtê-las, poder inativá-las para que sumissem da lista. Permite um controle prático e visual das cartas colecionadas.",
      en: "This project was created to fulfill a personal need in a card game: to visualize which cards I didn't have yet and, upon obtaining them, to deactivate them so they disappear from the list. It provides a practical and visual control of collected cards."
    },
    github: "https://github.com/GabrielSelim/Projeto_Gabriel"
  },
  {
    nome: "Sistema de Cadastro e Listagem de Livros",
    nomeEn: "Book Registration and Listing System",
    thumb: livrosThumb,
    imagens: projetoLivrosImgs,
    texto: {
      pt: "Este sistema de cadastro e listagem de livros foi desenvolvido para aprofundar meus conhecimentos em APIs RESTful, permitindo o gerenciamento eficiente de livros com operações de cadastro, listagem e atualização.",
      en: "This book registration and listing system was developed to deepen my knowledge in RESTful APIs, enabling efficient management of books with registration, listing, and update operations."
    },
    github: "https://github.com/GabrielSelim/Projeto_Gabriel"
  }
];

const technologies = [
  {
    name: ".NET C#",
    img: csharpLogo,
    color: "#178600",
    desc: {
      pt: "Tenho experiência prática em .NET, incluindo versões 4.5 e 8.0, e domínio em C# (5.0 a 12.0). Neste portfólio, desenvolvi uma API RESTful moderna utilizando .NET 8, C# 12, Entity Framework Core, MySQL, JWT para autenticação, Swagger para documentação, e padrões como Dependency Injection e Clean Architecture. Também aplico versionamento de API, controle de acesso por roles, testes automatizados com xUnit, e boas práticas de segurança e escalabilidade.",
      en: "I have practical experience in .NET, including versions 4.5 and 8.0, and mastery of C# (5.0 to 12.0). In this portfolio, I developed a modern RESTful API using .NET 8, C# 12, Entity Framework Core, MySQL, JWT for authentication, Swagger for documentation, and patterns such as Dependency Injection and Clean Architecture. I also apply API versioning, role-based access control, automated testing with xUnit, and best practices for security and scalability."
    }
  },
  {
    name: "RESTful APIs",
    img: restfulLogo,
    color: "#00b8d9",
    desc: {
      pt: "Desenvolvimento e consumo de APIs RESTful robustas, seguras e bem estruturadas. A API deste portfólio segue esse padrão, destacando boas práticas de arquitetura e documentação.",
      en: "Development and consumption of robust, secure, and well-structured RESTful APIs. The API for this portfolio follows this pattern, highlighting best practices in architecture and documentation."
    }
  },
  {
    name: "RBAC (Role-Based Access Control)",
    img: rbacLogo,
    color: "#f39c12",
    desc: {
      pt: "Implementação de controle de acesso baseado em funções para garantir segurança e flexibilidade em sistemas multiusuário. Esta funcionalidade está presente na API deste portfólio, demonstrando sua aplicação prática em ambientes reais.",
      en: "Implementation of role-based access control to ensure security and flexibility in multi-user systems. This feature is present in the API of this portfolio, demonstrating its practical application in real environments."
    }
  },
  {
    name: "Swagger",
    img: swaggerLogo,
    color: "#85ea2d",
    desc: {
      pt: "Documentação de APIs utilizando Swagger, incluindo suporte à autenticação via accessToken. A API deste portfólio conta com essa documentação interativa, facilitando a integração e os testes por parte dos desenvolvedores.",
      en: "API documentation using Swagger, including support for authentication via accessToken. The API of this portfolio features this interactive documentation, making integration and testing easier for developers."
    }
  },
  {
    name: "MySQL",
    img: mysqlLogo,
    color: "#00758f",
    desc: {
      pt: "Utilização do MySQL 8.0.3 como banco de dados relacional na API deste portfólio, garantindo desempenho, estabilidade e estrutura sólida para o armazenamento das informações.",
      en: "Use of MySQL 8.0.3 as the relational database in the API of this portfolio, ensuring performance, stability, and a solid structure for information storage."
    }
  },
  {
    name: "SQL Server",
    img: sqlserverLogo,
    color: "#a91d22",
    desc: {
      pt: "Experiência com SQL Server no desenvolvimento de um sistema de controle financeiro de grande porte. Utilização de funções, views, stored procedures e demais recursos avançados para garantir desempenho, integridade dos dados e organização eficiente da lógica de negócio.",
      en: "Experience with SQL Server in the development of a large-scale financial control system. Use of functions, views, stored procedures, and other advanced features to ensure performance, data integrity, and efficient organization of business logic."
    }
  },
  {
    name: "Docker",
    img: dockerLogo,
    color: "#2496ed",
    desc: {
      pt: "Criação e gerenciamento de containers para ambientes escaláveis, portáveis e consistentes. Este portfólio utiliza Docker para garantir facilidade de deploy, isolamento de ambientes e padronização no desenvolvimento.",
      en: "Creation and management of containers for scalable, portable, and consistent environments. This portfolio uses Docker to ensure easy deployment, environment isolation, and standardization in development."
    }
  },
  {
    name: "DigitalOcean",
    img: digitaloceanLogo,
    color: "#0080ff",
    desc: {
      pt: "Hospedagem de containers Docker em servidores na nuvem, com gerenciamento de DNS e deploy automatizado. Este site e sua API estão rodando em containers hospedados em um servidor dedicado na DigitalOcean, garantindo alta disponibilidade e escalabilidade.",
      en: "Hosting Docker containers on cloud servers, with DNS management and automated deployment. This site and its API run in containers hosted on a dedicated DigitalOcean server, ensuring high availability and scalability."
    }
  },
  {
    name: "React",
    img: reactLogo,
    color: "#61dafb",
    desc: {
      pt: "Tenho experiência com React na versão 19.1.0. Este site foi desenvolvido com essa tecnologia, aproveitando seus recursos modernos para criar uma interface dinâmica, responsiva e de alta performance.",
      en: "I have experience with React version 19.1.0. This site was developed with this technology, leveraging its modern features to create a dynamic, responsive, and high-performance interface."
    }
  },
  {
    name: "Angular",
    img: angularLogo,
    color: "#dd0031",
    desc: {
      pt: (
        <>
          Possuo conhecimento em Angular 19.2.4, utilizado para desenvolver o front-end de um jogo de cartas criado para aprimorar minhas habilidades.
          O projeto está disponível no GitHub:&nbsp;
          <a
            href="https://github.com/GabrielSelim/Jogo_Cartas"
            target="_blank"
            rel="noopener noreferrer"
            style={{ color: "#61dafb", textDecoration: "underline" }}
          >
            github.com/GabrielSelim/Jogo_Cartas
          </a>.
        </>
      ),
      en: (
        <>
          I have knowledge of Angular 19.2.4, used to develop the front-end of a card game created to improve my skills.
          The project is available on GitHub:&nbsp;
          <a
            href="https://github.com/GabrielSelim/Jogo_Cartas"
            target="_blank"
            rel="noopener noreferrer"
            style={{ color: "#61dafb", textDecoration: "underline" }}
          >
            github.com/GabrielSelim/Jogo_Cartas
          </a>.
        </>
      )
    }
  },
  {
    name: "HTML5 / CSS3",
    img: htmlLogo,
    color: "#e44d26",
    desc: {
      pt: "Domínio de HTML5 e CSS3 para construção de interfaces web modernas, responsivas e acessíveis, garantindo uma experiência consistente em diferentes dispositivos e navegadores.",
      en: "Proficiency in HTML5 and CSS3 for building modern, responsive, and accessible web interfaces, ensuring a consistent experience across different devices and browsers."
    }
  },
  {
    name: "JavaScript",
    img: javascriptLogo,
    color: "#f7df1e",
    desc: {
      pt: "Conhecimento em JavaScript, incluindo o uso de bibliotecas como jQuery, jqGrid e Bootstrap, para criação de interfaces interativas, grids dinâmicos e design responsivo.",
      en: "Knowledge of JavaScript, including the use of libraries such as jQuery, jqGrid, and Bootstrap, for creating interactive interfaces, dynamic grids, and responsive design."
    }
  },
  {
    name: "Razor",
    img: razorLogo,
    color: "#512bd4",
    desc: {
      pt: "Utilizo Razor no projeto de controle financeiro para integrar HTML com C# de forma dinâmica e eficiente, facilitando a criação de páginas web com conteúdo gerado no servidor e melhorando a separação entre lógica e apresentação.",
      en: "I use Razor in the financial control project to integrate HTML with C# dynamically and efficiently, making it easier to create web pages with server-generated content and improving the separation between logic and presentation."
    }
  },
  {
    name: "Metodologias Ágeis",
    img: metodologiaAgilLogo,
    color: "#ff9800",
    desc: {
      pt: "No projeto de controle financeiro, eu e minha equipe utilizamos metodologias ágeis como Scrum e Kanban para organizar tarefas, acompanhar o progresso e garantir entregas contínuas e bem planejadas.",
      en: "In the financial control project, my team and I used agile methodologies such as Scrum and Kanban to organize tasks, track progress, and ensure continuous and well-planned deliveries."
    }
  }
];

export default function Home({ language }) {
  const [modalTech, setModalTech] = useState(null);
  const [modalProjeto, setModalProjeto] = useState(null);
  const [carouselIdx, setCarouselIdx] = useState(0);
  const [zoom, setZoom] = useState(false);

  const cvFile =
    language === "pt"
      ? "/CV_Gabriel_Sanz_DEV.pdf"
      : "/CV_Gabriel_Sanz_DEV_English.pdf";

  // Função para abrir modal de projeto e resetar o índice do carrossel
  const openProjetoModal = (idx) => {
    setModalProjeto(idx);
    setCarouselIdx(0);
    setZoom(false);
  };

  return (
    <div className="main-bg">
      {/* Avatar */}
      <img
        src={avatar}
        alt="Gabriel Sanz Avatar"
        className="rounded-circle shadow-lg mb-4"
        style={{ width: 160, height: 160, objectFit: "cover", border: "5px solid #fff" }}
      />

      {/* Nome e Título */}
      <h1 className="fw-bold text-white text-shadow mb-1">Gabriel Sanz</h1>
      <h4 className="text-light mb-3">
        {language === "pt" ? "Desenvolvedor Full Stack" : "Full Stack Developer"}
      </h4>

      {/* Botão de Download do Currículo */}
      <a
        href={cvFile}
        download
        className="btn btn-lg btn-gradient mb-4"
        style={{ minWidth: 220 }}
      >
        {language === "pt" ? "Baixar Currículo" : "Download Resume"}
      </a>

      {/* Tecnologias */}
      <div className="home-container">
        <h5 className="text-white mb-4 text-center">
          {language === "pt" ? "Tecnologias e Experiências:" : "Technologies & Experience:"}
        </h5>
        <div className="row g-3 justify-content-center">
          {technologies.map((tech, idx) => (
            <div key={tech.name} className="col-12 col-sm-6 col-lg-4">
              <div
                className="card tech-card h-100 text-white shadow-sm"
                style={{ cursor: "pointer" }}
                onClick={() => setModalTech(idx)}
              >
                <div className="card-body d-flex align-items-center">
                  <img
                    src={tech.img}
                    alt={tech.name}
                    className="me-3"
                    style={{
                      width: 48,
                      height: 48,
                      filter: `drop-shadow(0 0 8px ${tech.color})`
                    }}
                  />
                  <div>
                    <h6 className="mb-1">{tech.name}</h6>
                    <small className="text-secondary">{language === "pt" ? "Clique para saber mais" : "Click to know more"}</small>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Projetos Pessoais */}
      <div className="home-container mt-5">
        <h5 className="text-white mb-4 text-center">
          {language === "pt" ? "Projetos Pessoais:" : "Personal Projects:"}
        </h5>
        <div className="row g-4 justify-content-center">
          {projetos.map((proj, idx) => (
            <div
              key={proj.nome}
              className={`col-12 col-md-6 col-lg-4${proj.destaque ? " destaque-financeiro" : ""}`}
              style={proj.destaque ? { order: -1 } : {}}
            >
              <div
                className={`card h-100 shadow-lg project-card${proj.destaque ? " project-card-destaque" : ""}`}
                style={{
                  cursor: "pointer",
                  border: "none",
                  borderRadius: 18,
                  background: "#181c24",
                  color: "#fff",
                  position: "relative",
                  overflow: "hidden",
                  ...(proj.destaque ? { boxShadow: "0 0 0 4px #00c6ff, 0 8px 32px rgba(0,0,0,0.35)" } : {})
                }}
                onClick={() => openProjetoModal(idx)}
              >
                <img
                  src={proj.thumb}
                  alt={proj.nome}
                  style={{
                    width: "100%",
                    height: proj.destaque ? 220 : 180,
                    objectFit: "cover",
                    borderTopLeftRadius: 18,
                    borderTopRightRadius: 18,
                    borderBottom: "2px solid #00c6ff"
                  }}
                />
                <div className="card-body d-flex flex-column align-items-center justify-content-center" style={{ minHeight: proj.destaque ? 140 : 120 }}>
                  <h6 className="fw-bold text-center" style={{ fontSize: proj.destaque ? 24 : 20, color: proj.destaque ? "#00c6ff" : "#fff" }}>
                    {language === "pt" ? proj.nome : proj.nomeEn}
                  </h6>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Modal de Tecnologia */}
      {modalTech !== null && (
        <div className="modal fade show d-block" tabIndex="-1" style={{ background: "rgba(0,0,0,0.7)" }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content bg-dark text-white">
              <div className="modal-header border-0">
                <h5 className="modal-title d-flex align-items-center">
                  <img
                    src={technologies[modalTech].img}
                    alt={technologies[modalTech].name}
                    style={{
                      width: 36,
                      height: 36,
                      marginRight: 10,
                      filter: `drop-shadow(0 0 8px ${technologies[modalTech].color})`
                    }}
                  />
                  {technologies[modalTech].name}
                </h5>
                <button type="button" className="btn-close btn-close-white" onClick={() => setModalTech(null)}></button>
              </div>
              <div className="modal-body">
                {/* Permite React node para Angular */}
                {typeof technologies[modalTech].desc[language] === "string"
                  ? <p>{technologies[modalTech].desc[language]}</p>
                  : technologies[modalTech].desc[language]}
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Modal de Projeto */}
      {modalProjeto !== null && (
        <div className="modal fade show d-block" tabIndex="-1" style={{ background: "rgba(0,0,0,0.7)" }}>
          <div className="modal-dialog modal-lg modal-dialog-centered">
            <div className="modal-content bg-dark text-white" style={{ position: "relative" }}>
              <div className="modal-header border-0">
                <h5 className="modal-title d-flex align-items-center">
                  <img
                    src={projetos[modalProjeto].thumb}
                    alt={language === "pt" ? projetos[modalProjeto].nome : projetos[modalProjeto].nomeEn}
                    style={{
                      width: 36,
                      height: 36,
                      marginRight: 10,
                      borderRadius: 8
                    }}
                  />
                  {language === "pt" ? projetos[modalProjeto].nome : projetos[modalProjeto].nomeEn}
                </h5>
                <button type="button" className="btn-close btn-close-white" onClick={() => setModalProjeto(null)}></button>
              </div>
              <div className="modal-body">
                {/* Carrossel de imagens */}
                <div className="d-flex flex-column align-items-center mb-3">
                  <img
                    src={projetos[modalProjeto].imagens[carouselIdx]}
                    alt={`Projeto ${carouselIdx + 1}`}
                    className={zoom ? "pokemon-zoom active" : "pokemon-zoom"}
                    style={{
                      width: "100%",
                      maxWidth: 420,
                      height: 240,
                      objectFit: "contain",
                      borderRadius: 12,
                      background: "#222",
                      zIndex: 1,
                      cursor: "zoom-in"
                    }}
                    onClick={() => setZoom(z => !z)}
                  />
                  <div className="mt-2 d-flex gap-2">
                    <button
                      className="btn btn-sm btn-secondary"
                      disabled={carouselIdx === 0}
                      onClick={e => {
                        e.stopPropagation();
                        setCarouselIdx(idx => Math.max(0, idx - 1));
                        setZoom(false);
                      }}
                    >
                      {"<"}
                    </button>
                    <span style={{ color: "#fff", minWidth: 40, textAlign: "center" }}>
                      {carouselIdx + 1} / {projetos[modalProjeto].imagens.length}
                    </span>
                    <button
                      className="btn btn-sm btn-secondary"
                      disabled={carouselIdx === projetos[modalProjeto].imagens.length - 1}
                      onClick={e => {
                        e.stopPropagation();
                        setCarouselIdx(idx => Math.min(projetos[modalProjeto].imagens.length - 1, idx + 1));
                        setZoom(false);
                      }}
                    >
                      {">"}
                    </button>
                  </div>
                </div>
                <p style={{ fontSize: 16 }}>
                  {projetos[modalProjeto].texto[language]}
                </p>
                <a
                  href={projetos[modalProjeto].github}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="github-btn"
                  title="Ver no GitHub"
                  onClick={e => e.stopPropagation()}
                >
                  <img src={githubLogo} alt="GitHub" />
                </a>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}