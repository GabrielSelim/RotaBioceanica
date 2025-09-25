import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import './style.css';
import logoImage from '../../assets/LogoSemFundo.png';
import api from '../../services/api';
import '../../global.css';
import ReCAPTCHA from "react-google-recaptcha";

export default function Login({ language }) {
    const [nomeUsuario, setUserName] = useState('');
    const [senha, setPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [loginErro, setLoginErro] = useState(false);
    const [captchaOk, setCaptchaOk] = useState(false);
    const [erroMensagem, setErroMensagem] = useState('');
    const navigate = useNavigate();

    async function login(e) {
        e.preventDefault();
        if (!captchaOk) {
            setErroMensagem(language === 'pt' ? 'Confirme o captcha!' : 'Please confirm the captcha!');
            return;
        }
        setErroMensagem('');
        const data = { nomeUsuario, senha };

        try {
            const response = await api.post('api/auth/v1/signin', data);
            localStorage.setItem('nomeUsuario', nomeUsuario);
            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('refreshToken', response.data.refreshToken);
            localStorage.setItem('userRole', response.data.role);
            window.location.href = '/';
        } catch (error) {
            if (error.response && error.response.status === 401) {
                setLoginErro(true);
                setErroMensagem(language === 'pt' ? 'Nome de usuário ou senha inválida' : 'Invalid username or password');
            } else {
                setErroMensagem(language === 'pt' ? 'Ocorreu um erro inesperado. Tente novamente mais tarde.' : 'An unexpected error occurred. Please try again later.');
            }
        }
    }

    return (
        <div className="login-bg d-flex align-items-center justify-content-center">
            <div className="login-card shadow-lg p-4 rounded-4">
                <div className="text-center mb-4">
                    <img src={logoImage} alt="Logo" className="login-logo mb-2" />
                    <h2 className="fw-bold text-white">{language === 'pt' ? 'Bem-vindo!' : 'Welcome!'}</h2>
                    <p className="text-light mb-0" style={{ opacity: 0.8 }}>
                        {language === 'pt' ? 'Acesse sua conta para continuar' : 'Access your account to continue'}
                    </p>
                </div>
                {/* ALERTA DE ERRO */}
                {erroMensagem && (
                    <div className="alert alert-danger py-2 text-center" role="alert">
                        {erroMensagem}
                    </div>
                )}
                <form onSubmit={login}>
                    <div className="mb-3">
                        <input
                            type="text"
                            className={`form-control form-control-lg ${loginErro ? 'is-invalid' : ''}`}
                            placeholder={language === 'pt' ? 'Digite seu usuário' : 'Enter your username'}
                            value={nomeUsuario}
                            onChange={e => {
                                setUserName(e.target.value);
                                if (loginErro) setLoginErro(false);
                            }}
                        />
                    </div>
                    <div className="mb-3 position-relative">
                        <input
                            type={showPassword ? "text" : "password"}
                            className={`form-control form-control-lg ${loginErro ? 'is-invalid' : ''}`}
                            placeholder={language === 'pt' ? 'Digite sua senha' : 'Enter your password'}
                            value={senha}
                            onChange={e => {
                                setPassword(e.target.value);
                                if (loginErro) setLoginErro(false);
                            }}
                        />
                        <button
                            type="button"
                            className="btn btn-link btn-sm show-hide-btn"
                            tabIndex={-1}
                            onClick={() => setShowPassword(!showPassword)}
                        >
                            {showPassword ? "🙈" : "👁️"}
                        </button>
                    </div>
                    <div className="mb-3 d-flex justify-content-center">
                        <ReCAPTCHA
                            sitekey={process.env.REACT_APP_RECAPTCHA_SITE_KEY}
                            onChange={() => setCaptchaOk(true)}
                            theme="dark"
                        />
                    </div>
                    <button className="btn btn-gradient w-100 py-2 mb-2" type="submit">
                        {language === 'pt' ? 'Entrar' : 'Log in'}
                    </button>
                    <button
                        className="btn btn-outline-light w-100 py-2"
                        type="button"
                        onClick={() => navigate('/criar-usuario')}
                    >
                        {language === 'pt' ? 'Criar Usuário' : 'Create User'}
                    </button>
                </form>
            </div>
        </div>
    );
}