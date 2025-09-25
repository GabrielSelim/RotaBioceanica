import React, { useEffect, useState } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import '../global.css';
import logoImage from '../assets/LogoSemFundo.png';
import { FiPower, FiSun, FiMoon } from 'react-icons/fi';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import './Navbar.css';
import OpcoesMenu from './OpcoesMenu';

export default function Navbar({ language, setLanguage }) {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [theme, setTheme] = useState('light');
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [userRole, setUserRole] = useState(null);
    const [menuValue, setMenuValue] = useState('');
    const nomeUsuario = localStorage.getItem('nomeUsuario');
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        // Detecta idioma do navegador na primeira visita
        const savedLanguage = localStorage.getItem('language');
        if (!savedLanguage) {
            const browserLang = navigator.language || navigator.userLanguage;
            if (browserLang && browserLang.toLowerCase().startsWith('pt')) {
                setLanguage('pt');
                localStorage.setItem('language', 'pt');
            } else {
                setLanguage('en');
                localStorage.setItem('language', 'en');
            }
        }

        const accessToken = localStorage.getItem('accessToken');
        setIsLoggedIn(!!accessToken);

        const savedTheme = localStorage.getItem('theme') || 'light';
        setTheme(savedTheme);
        document.body.className = savedTheme;

        const role = localStorage.getItem('userRole');
        setUserRole(role);
    }, []);

    useEffect(() => {
        setMenuValue(location.pathname);
    }, [location.pathname]);

    function handleLogout() {
        localStorage.clear();
        setIsLoggedIn(false);
        navigate('/');
        setMenuValue('/');
    }

    function toggleTheme() {
        const newTheme = theme === 'light' ? 'dark' : 'light';
        setTheme(newTheme);
        document.body.className = newTheme;
        localStorage.setItem('theme', newTheme);
    }

    function changeLanguage(lang) {
        setLanguage(lang);
        localStorage.setItem('language', lang);
    }

    function toggleMenu() {
        setIsMenuOpen(!isMenuOpen);
    }

    return (
        <nav className={`navbar navbar-expand-lg ${theme === 'light' ? 'navbar-light bg-light' : 'navbar-dark bg-dark'}`}>
            <div className="container-fluid">
                {/* Logo e botão de sanduíche */}
                <Link
                    to="/"
                    className="navbar-brand d-flex align-items-center"
                    onClick={() => setMenuValue('/')}
                >
                    <img src={logoImage} alt="Logo" className="navbar-logo" />
                    <span className="ms-2">{language === 'pt' ? 'Gabriel Sanz Tech' : 'Gabriel Sanz Tech'}</span>
                </Link>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarNav"
                    aria-controls="navbarNav"
                    aria-expanded={isMenuOpen}
                    aria-label="Toggle navigation"
                    onClick={toggleMenu}
                >
                    <span className="navbar-toggler-icon"></span>
                </button>

                {/* Itens do Navbar */}
                <div className={`collapse navbar-collapse ${isMenuOpen ? 'show' : ''}`} id="navbarNav">
                    <ul
                        className={`navbar-nav w-100 d-flex flex-column flex-lg-row ${
                            isMenuOpen ? 'custom-mobile-alignment' : 'justify-content-lg-end'
                        }`}
                    >
                        {isLoggedIn && (
                            <li className="nav-item d-flex align-items-center mb-2 mb-lg-0 me-auto">
                                <span className="nav-link">
                                    {language === 'pt' ? 'Bem Vindo' : 'Welcome'}, <strong>{nomeUsuario || (language === 'pt' ? 'Usuário' : 'User')}</strong>!
                                </span>
                            </li>
                        )}

                        <li className="nav-item d-flex align-items-center ms-lg-auto">
                            {/* Menu Dropdown */}
                            {isLoggedIn && (
                            <select
                                className={`form-select me-2 ${theme === 'dark' ? 'dropdown-menu-dark' : ''}`}
                                value={menuValue}
                                onChange={(e) => {
                                setMenuValue(e.target.value);
                                if (e.target.value) navigate(e.target.value);
                                }}
                            >
                                {OpcoesMenu
                                .filter(opt => !opt.adminOnly || userRole === 'Admin')
                                .map(opt => (
                                    <option key={opt.value} value={opt.value}>
                                    {opt.label[language] || opt.label.pt}
                                    </option>
                                ))}
                            </select>
                            )}

                            {/* Botão de iluminação */}
                            <button onClick={toggleTheme} className="btn btn-link nav-link me-2">
                                {theme === 'light' ? <FiMoon size={24} /> : <FiSun size={24} />}
                            </button>

                            {/* Dropdown de linguagem */}
                            <select
                                className="form-select me-2"
                                value={language}
                                onChange={(e) => changeLanguage(e.target.value)}
                            >
                                <option value="pt">Português</option>
                                <option value="en">English</option>
                            </select>

                            {/* Botão de login/logoff */}
                            {isLoggedIn ? (
                                <button onClick={handleLogout} className="btn btn-danger btn-login-logout">
                                    <FiPower size={24} />
                                </button>
                            ) : (
                                <button onClick={() => navigate('/login')} className="btn btn-primary btn-login-logout">
                                    {language === 'pt' ? 'Login' : 'Login'}
                                </button>
                            )}
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
}