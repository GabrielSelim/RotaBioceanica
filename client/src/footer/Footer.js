import React from 'react';
import { useNavigate } from 'react-router-dom';
import instagramLogo from '../assets/instagram.png';
import githubLogo from '../assets/github.png';
import swaggerLogo from '../assets/swagger.png';
import linkedinLogo from '../assets/linkedin.png';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import './Footer.css';

export default function Footer({ language }) {
    const navigate = useNavigate();

    return (
        <footer className="footer">
            <div className="container-fluid d-flex flex-column flex-lg-row align-items-center justify-content-between">
                <div className="footer-btn-group d-flex gap-3 align-items-center">
                    <button
                        className="footer-contact-btn"
                        onClick={() => navigate('/sobre-mim')}
                    >
                        <i className="bi bi-person-fill"></i>
                        <span className="footer-contact-text">
                            {language === 'pt' ? 'Sobre mim' : 'About Me'}
                        </span>
                    </button>
                    <button
                        className="footer-contact-btn"
                        onClick={() => navigate('/contato')}
                    >
                        <i className="bi bi-envelope-fill"></i>
                        <span className="footer-contact-text">
                            {language === 'pt' ? 'Entre em Contato' : 'Contact Me'}
                        </span>
                    </button>
                    <button
                        className="footer-contact-btn"
                        onClick={() => navigate('/agradecimentos')}
                    >
                        <i className="bi bi-star-fill"></i>
                        <span className="footer-contact-text">
                            {language === 'pt' ? 'Agradecimentos' : 'Acknowledgements'}
                        </span>
                    </button>
                </div>
                <div className="d-flex gap-3 align-items-center">
                    <a href="https://www.linkedin.com/in/gabriel-sanz/" target="_blank" rel="noopener noreferrer">
                        <img src={linkedinLogo} alt="Linkedin" className="footer-logo" />
                    </a>
                    <a href="https://www.instagram.com/gabriel.selim" target="_blank" rel="noopener noreferrer">
                        <img src={instagramLogo} alt="Instagram" className="footer-logo" />
                    </a>
                    <a href="https://github.com/GabrielSelim" target="_blank" rel="noopener noreferrer">
                        <img src={githubLogo} alt="GitHub" className="footer-logo github" />
                    </a>
                    <a href="https://api.gabrielsanztech.com.br/" target="_blank" rel="noopener noreferrer">
                        <img src={swaggerLogo} alt="Swagger" className="footer-logo" />
                    </a>
                </div>
            </div>
        </footer>
    );
}