import React from "react";
import "./style.css";
import esposa from "../../assets/Agradecimentos/Esposa.jpg";
import mae from "../../assets/Agradecimentos/Lilian.jpg";
import mauricio from "../../assets/Agradecimentos/Mauricio Benites.jpeg";
import walace from "../../assets/Agradecimentos/Walace.jpg";
import carlos from "../../assets/Agradecimentos/Carlos Moura.jpeg";

const textos = {
  pt: {
    titulo: "Agradecimentos",
    intro:
      "Ninguém chega longe sozinho. Aqui estão pessoas especiais que fizeram e fazem parte da minha trajetória e contribuíram para o meu crescimento pessoal e profissional:",
    principais: [
      {
        nome: "Stephanie Alves",
        imagem: esposa,
        texto:
          "Minha parceira de vida, que sempre acreditou em mim e esteve ao meu lado nos momentos mais importantes. Seu apoio, carinho e compreensão tornam cada conquista ainda mais especial. Obrigado por ser meu alicerce e minha maior motivação."
      },
      {
        nome: "Lilian Sanz",
        imagem: mae,
        texto:
          "Mãe, obrigado por sempre acreditar em mim e estar ao meu lado em todos os momentos. Seu apoio e amor foram essenciais para que eu chegasse até aqui. Sou muito grato por tudo que fez e faz por mim."
      }
    ],
    outros: [
      {
        nome: "Mauricio Benites",
        imagem: mauricio,
        texto:
          "Sou muito grato ao Mauricio por ter acreditado no meu sonho e me dado a primeira oportunidade na área de desenvolvimento de software. Seu apoio e confiança foram fundamentais para que eu pudesse crescer e aprender. Obrigado por abrir as portas e incentivar meu desenvolvimento profissional."
      },
      {
        nome: "Carlos Moura",
        imagem: carlos,
        texto:
          "Carlos foi meu primeiro amigo na área de desenvolvimento de software e, com o tempo, se tornou um grande amigo pessoal. Compartilhamos muitos aprendizados, desafios e conquistas juntos. Sou muito grato pela parceria, pelas conversas e pela amizade verdadeira que construímos."
      },
      {
        nome: "Walace Silva",
        imagem: walace,
        texto:
          "Amigo que sempre acreditou no meu potencial e me incentivou a crescer. Foi ele quem me motivou a criar este portfólio quando compartilhei a ideia. Sou grato pela amizade, pelo apoio e por estar presente nos momentos importantes da minha trajetória."
      }
    ]
  },
  en: {
    titulo: "Acknowledgements",
    intro:
        "No one gets far alone. Here are some special people who have been part of my journey and contributed to my personal and professional growth:",
    principais: [
      {
        nome: "Stephanie Alves",
        imagem: esposa,
        texto:
          "My life partner, who has always believed in me and stood by my side during the most important moments. Your support, affection, and understanding make every achievement even more special. Thank you for being my foundation and my greatest motivation."
      },
      {
        nome: "Lilian Sanz",
        imagem: mae,
        texto:
            "Mom, thank you for always believing in me and being by my side in every moment. Your support and love were essential for me to get here. I am very grateful for everything you have done and continue to do for me."
      }
    ],
    outros: [
      {
        nome: "Mauricio Benites",
        imagem: mauricio,
        texto:
            "I am very grateful to Mauricio for believing in my dream and giving me my first opportunity in software development. His support and trust were fundamental for my growth and learning. Thank you for opening doors and encouraging my professional development."
      },
      {
        nome: "Carlos Moura",
        imagem: carlos,
        texto:
            "Carlos was my first friend in the software development field and, over time, became a great personal friend. We have shared many learnings, challenges, and achievements together. I am very grateful for the partnership, the conversations, and the true friendship we have built."
      },
      {
        nome: "Walace Silva",
        imagem: walace,
        texto:
          "Friend who always believed in my potential and encouraged me to grow. He was the one who motivated me to create this portfolio when I shared the idea. I am grateful for the friendship, support, and for being present in the important moments of my journey."
      }
    ]
  }
};

export default function Agradecimento({ language = "pt" }) {
  const t = textos[language];

  return (
    <div className="agradecimento-bg">
      <div className="agradecimento-card">
        <h1 className="agradecimento-titulo">{t.titulo}</h1>
        <p className="agradecimento-intro">{t.intro}</p>
        <div className="agradecimento-linhas">
          <div className="agradecimento-grid agradecimento-grid-principal">
            {t.principais.map((pessoa) => (
              <div className="agradecimento-quadro" key={pessoa.nome}>
                <div className="agradecimento-img-wrapper">
                  <img src={pessoa.imagem} alt={pessoa.nome} className="agradecimento-img" />
                </div>
                <div className="agradecimento-nome">{pessoa.nome}</div>
                <div className="agradecimento-texto">{pessoa.texto}</div>
              </div>
            ))}
          </div>
          <div className="agradecimento-grid agradecimento-grid-outros">
            {t.outros.map((pessoa) => (
              <div className="agradecimento-quadro" key={pessoa.nome}>
                <div className="agradecimento-img-wrapper">
                  <img src={pessoa.imagem} alt={pessoa.nome} className="agradecimento-img" />
                </div>
                <div className="agradecimento-nome">{pessoa.nome}</div>
                <div className="agradecimento-texto">{pessoa.texto}</div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}