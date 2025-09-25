import React, { useEffect, useRef, useState } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";
import "./amor.css";

const steps = [
  { type: 'text', content: 'Bem-vinda ao nosso momento especial!' },
  { type: 'text', content: 'Cada dia ao seu lado é uma nova aventura.' },
  { type: 'image', src: require('../../assets/amor/foto_1_gotica.jpeg'), alt: 'Gótica', caption: 'Minha gótica favorita 💜' },
  { type: 'image', src: require('../../assets/amor/foto_2_primeiraFoto.jpeg'), alt: 'Nós dois', caption: 'A nossa primeira foto juntos!' },
  { type: 'text', content: 'Lembra desse momento nos altos da afonso pena?' },
  { type: 'image', src: require('../../assets/amor/foto_altos_afonso.jpeg'), alt: 'Lembrança' },
  { type: 'image', src: require('../../assets/amor/foto_3_cabelo.jpeg'), alt: 'Cabelo' , caption: 'Quando você chegou na minha vida meu cabelo até voltou a crescer!!' },
  { type: 'image', src: require('../../assets/amor/foto_fuckyou.jpeg'), alt: 'fcku' , caption: 'Uma época estranha de nossas vidas!' },
  { type: 'image', src: require('../../assets/amor/foto_4_emCasa.jpeg'), alt: 'Dificeis' , caption: 'Muito obrigado por não ter desistido de mim, nessa época que foi dificil!' },
  { type: 'image', src: require('../../assets/amor/foto5_Casamento.jpeg'), alt: 'Casamento' , caption: 'Eu te amo muito, seja você maquiada assim ou sem, você é simplesmente perfeita!' },
  { type: 'image', src: require('../../assets/amor/foto6_viajemSP.jpeg'), alt: 'SaoPaulo' , caption: 'A primeira viajem sozinhos para ficar nas nossas memórias!!  💜' },
  { type: 'image', src: require('../../assets/amor/foto_7_outback.jpeg'), alt: 'Outback' , caption: 'No nosso restaurante preferido!!' },
  { type: 'image', src: require('../../assets/amor/foto_8_soad.jpeg'), alt: 'Soad' , caption: 'E Finalmente nesse show que foi inesquecivel!!' },
  { type: 'text', content: 'E hoje nós possuimos a nossa fámilia!!💜' },
  { type: 'image', src: require('../../assets/amor/gato1.jpeg'), alt: 'safira' , caption: 'A nossa primeira filha que nos escolheu!!' },
  { type: 'image', src: require('../../assets/amor/gato2.jpeg'), alt: 'agata' , caption: 'A nossa segunda filha, que é charmosa demais!!' },
  { type: 'image', src: require('../../assets/amor/gato3.jpeg'), alt: 'jade' , caption: 'A nossa terceira filha, que é sensivel igual a mãe!!' },
  { type: 'image', src: require('../../assets/amor/gato4.jpeg'), alt: 'cristal' , caption: 'A nossa quarta filha que veio para mudar nossas vidas!!' },
  { type: 'text', content: 'O nosso(a) proximo(a) será muito amado(a) e virá da mulher mais linda deste mundo!! VOCÊ!' },
  { type: 'text', content: 'Você ilumina meus dias e faz tudo valer a pena. Obrigado por ser quem você é!' },
  { type: 'image', src: require('../../assets/amor/Presente.jpeg'), alt: 'Presente', caption: 'Seu presente está aqui! Abra essa gaveta para encontrar uma surpresa que você queria com muito amor. 💜' },
  { type: 'text', content: 'Feliz Dia dos Namorados! Te amo!💜' },
];

export default function Amor() {
  const [step, setStep] = useState(0);
  const [audioStarted, setAudioStarted] = useState(false);
  const audioRef = useRef(null);

  // Tenta tocar automaticamente, mas se o navegador bloquear, mostra botão
  useEffect(() => {
    if (audioRef.current && !audioStarted) {
      const playPromise = audioRef.current.play();
      if (playPromise !== undefined) {
        playPromise
          .then(() => setAudioStarted(true))
          .catch(() => setAudioStarted(false));
      }
    }
  }, [audioStarted]);

  const handleStartAudio = () => {
    if (audioRef.current) {
      audioRef.current.play();
      setAudioStarted(true);
    }
  };

  const nextStep = () => setStep((s) => Math.min(s + 1, steps.length - 1));
  const current = steps[step];

  return (
    <div className="amor-bg">
      {/* Música de fundo */}
      <audio
        ref={audioRef}
        src={require('../../assets/amor/System Of A Down - Lonely Day.mp3')}
        autoPlay
        loop
        style={{ display: "none" }}
      />
      {!audioStarted && (
        <div className="audio-overlay">
          <button className="amor-btn btn btn-lg" onClick={handleStartAudio}>
            Tocar música especial 🎵
          </button>
          <div className="audio-msg">
            (Clique para ouvir "Lonely Day" do System of a Down)
          </div>
        </div>
      )}
      <div className="coraçoes-bg">
        <div className="coraçao" />
        <div className="coraçao" />
        <div className="coraçao" />
        <div className="coraçao" />
        <div className="coraçao" />
      </div>
      <div className="amor-card shadow-lg">
        {current.type === 'text' && <h2>{current.content}</h2>}
        {current.type === 'image' && (
          <>
            <img src={current.src} alt={current.alt} className="img-fluid" />
            {current.caption && (
              <div className="amor-caption mt-2">{current.caption}</div>
            )}
          </>
        )}
        {step < steps.length - 1 && (
          <button
            onClick={nextStep}
            className="amor-btn btn btn-lg mt-3"
            type="button"
          >
            Próximo
          </button>
        )}
      </div>
    </div>
  );
}