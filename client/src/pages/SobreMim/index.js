import React, { useState, useEffect, useRef } from "react";
import "./style.css";
import avatar from "../../assets/avatar.jpeg";
import devCoding from "../../assets/dev_coding.png";

const textos = {
  pt: {
    titulo: "Sobre Mim",
    cargo: "Desenvolvedor Full Stack",
    intro: [
      "Olá! Sou Gabriel Sanz, desenvolvedor de software full-stack dedicado a criar soluções eficientes e focadas no usuário. Embora minha jornada no desenvolvimento seja mais recente, com 4 anos de experiência, encontrei aqui minha verdadeira paixão, buscando aprimoramento contínuo.",
      "Sou proficiente em backend com .NET (C#) e ASP.NET Core, e tenho experiência com MySQL e SQL Server, além de APIs RESTful e Docker. No front-end, domino React e Angular para interfaces funcionais e responsivas. Minha experiência inclui manutenção de sistemas de planejamento e finanças, e desenvolvimento de scripts SQL e relatórios (Jasper, Report Builder).",
      "Meu compromisso com o aprendizado contínuo se reflete em pós-graduações em Engenharia de Software e cursos em Arquitetura Cloud Computing, Azure Data (DP-900) e Azure AI (AI-900). O \"Projeto_Gabriel\" — uma API de gerenciamento financeiro pessoal (api.gabrielsanztech.com.br) — é prova disso, com autenticação JWT, modelagem de dados complexa, interface responsiva, Flyway e Serilog.",
      "Minha experiência anterior como engenheiro autônomo e estagiário (Prefeitura de Campo Grande, FUNASA) me deu uma base sólida em análise, organização e resolução de problemas. Sou adepto de metodologias ágeis (Scrum e Kanban).",
      "Acredito no poder transformador da tecnologia, buscando sempre unir performance, design e usabilidade. Se quiser trocar ideias sobre tecnologia, projetos ou carreira, sinta-se à vontade para me contatar!"
    ],
    destaque: "Experiência em .NET, APIs RESTful, Docker, React, Angular, bancos de dados, cloud, automação e metodologias ágeis.",
    skills: [
      ".NET", "C#", "ASP.NET Core", "React", "Angular", "SQL Server", "MySQL", "REST API", "Docker", "Cloud", "JWT", "Flyway", "Serilog", "Jasper", "Report Builder", "Scrum", "Kanban"
    ],
    contato: "Fale comigo"
  },
  en: {
    titulo: "About Me",
    cargo: "Full Stack Developer",
    intro: [
      "Hi! I'm Gabriel Sanz, a full-stack software developer dedicated to creating efficient, user-focused solutions. Although my journey in development is more recent, with 4 years of experience, this is where I found my true passion, always seeking continuous improvement.",
      "I am proficient in backend with .NET (C#) and ASP.NET Core, and have experience with MySQL and SQL Server, as well as RESTful APIs and Docker. On the front-end, I master React and Angular for functional and responsive interfaces. My experience includes maintaining planning and finance systems, and developing SQL scripts and reports (Jasper, Report Builder).",
      "My commitment to continuous learning is reflected in post-graduate studies in Software Engineering and courses in Cloud Systems Architecture, Azure Data (DP-900), and Azure AI (AI-900). \"Projeto_Gabriel\" — a personal finance management API (api.gabrielsanztech.com.br) — is proof of this, featuring JWT authentication, complex data modeling, responsive interface, Flyway, and Serilog.",
      "My previous experience as a freelance engineer and intern (City Hall of Campo Grande, FUNASA) gave me a solid foundation in analysis, organization, and problem-solving. I am an advocate of agile methodologies (Scrum and Kanban).",
      "I believe in the transformative power of technology, always seeking to combine performance, design, and usability. If you'd like to chat about technology, projects, or career, feel free to contact me!"
    ],
    destaque: "Experience in .NET, RESTful APIs, Docker, React, Angular, databases, cloud, automation, and agile methodologies.",
    skills: [
      ".NET", "C#", "ASP.NET Core", "React", "Angular", "SQL Server", "MySQL", "REST API", "Docker", "Cloud", "JWT", "Flyway", "Serilog", "Jasper", "Report Builder", "Scrum", "Kanban"
    ],
    contato: "Contact me"
  }
};

// Typewriter para múltiplos parágrafos, mostrando todos os anteriores
function TypewriterParagraph({ text, onDone, instant }) {
  const [displayed, setDisplayed] = useState(instant ? text : "");
  const i = useRef(0);

  useEffect(() => {
    setDisplayed(instant ? text : "");
    i.current = 0;
    if (!text) return;
    if (instant) {
      if (onDone) onDone();
      return;
    }
    // Corrigido: usar função para garantir que sempre pega o texto correto
    const interval = setInterval(() => {
      setDisplayed((prev) => {
        // Corrige bug de letras pulando: usa prev.length como índice
        const nextChar = text.charAt(prev.length);
        if (nextChar) {
          return prev + nextChar;
        }
        return prev;
      });
      i.current++;
      if (i.current >= text.length) {
        clearInterval(interval);
        if (onDone) setTimeout(onDone, 400);
      }
    }, 18);
    return () => clearInterval(interval);
  }, [text, onDone, instant]);

  return (
    <span className="sobre-mim-typewriter">
      {displayed}
      {!instant && <span className="type-cursor" />}
    </span>
  );
}

export default function SobreMim({ language = "pt" }) {
  const t = textos[language];
  const [step, setStep] = useState(0);
  const [done, setDone] = useState(false);
  const [forceInstant, setForceInstant] = useState(false);

  // Avança para o próximo parágrafo
  const handleNext = () => {
    if (!done) {
      setForceInstant(true); // Completa o parágrafo atual instantaneamente
    } else if (step < t.intro.length - 1) {
      setStep(step + 1);
      setDone(false);
      setForceInstant(false);
    }
  };

  // Volta para o início ao clicar com o botão direito
  const handlePrev = (e) => {
    e.preventDefault();
    setStep(0);
    setDone(false);
    setForceInstant(false);
  };

  return (
    <div className="sobre-mim-bg">
      <div className="sobre-mim-card">
        <div className="sobre-mim-profile">
          <img src={avatar} alt="Gabriel Sanz" className="sobre-mim-avatar" />
          <div>
            <h1 className="sobre-mim-nome">
              <span className="sobre-mim-logo">{"<"}</span>
              Gabriel Sanz
              <span className="sobre-mim-logo">{"/>"}</span>
            </h1>
            <h2 className="sobre-mim-cargo">{t.cargo}</h2>
          </div>
        </div>
        <div
          className="sobre-mim-intro-interativa"
          tabIndex={0}
          onClick={handleNext}
          onContextMenu={handlePrev}
          title={
            done
              ? step < t.intro.length - 1
                ? language === "pt"
                  ? "Clique para avançar, botão direito para voltar ao início"
                  : "Click to advance, right-click to go back to start"
                : language === "pt"
                  ? "Clique com o botão direito para voltar ao início"
                  : "Right-click to go back to start"
              : language === "pt"
                ? "Clique para completar o parágrafo"
                : "Click to complete the paragraph"
          }
        >
          {t.intro.slice(0, step).map((par, idx) => (
            <p className="sobre-mim-typewriter" key={idx}>{par}</p>
          ))}
          <p>
            <TypewriterParagraph
              text={t.intro[step]}
              onDone={() => setDone(true)}
              instant={forceInstant}
            />
          </p>
          <span className="sobre-mim-intro-hint">
            {done
              ? step < t.intro.length - 1
                ? language === "pt"
                  ? "Clique para continuar"
                  : "Click to continue"
                : language === "pt"
                  ? "Fim do texto. Clique com o botão direito para voltar ao início"
                  : "End of text. Right-click to go back to start"
              : language === "pt"
                ? "Digitando... (clique para completar)"
                : "Typing... (click to complete)"}
          </span>
        </div>
        <div className="sobre-mim-destaque">
          <span>{t.destaque}</span>
        </div>
        <div className="sobre-mim-skills">
          {t.skills.map(skill => (
            <span className="sobre-mim-skill-badge" key={skill}>{skill}</span>
          ))}
        </div>
        <div className="sobre-mim-contato">
          <a
            href="mailto:eng.gabrielsanz@hotmail.com"
            className="sobre-mim-btn"
            title={t.contato}
          >
            <span className="sobre-mim-btn-icon" role="img" aria-label="email">✉️</span>
            <span>{t.contato}</span>
          </a>
        </div>
        <div className="sobre-mim-ilustracao">
          <img
            src={devCoding}
            alt={language === "pt" ? "Ilustração de desenvolvedor programando" : "Developer coding illustration"}
            className="sobre-mim-ilustra-img"
          />
        </div>
      </div>
    </div>
  );
}