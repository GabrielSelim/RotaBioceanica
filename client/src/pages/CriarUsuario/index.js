import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import './style.css';
import logoImage from '../../assets/LogoSemFundo.png';
import api from '../../services/api';
import ReCAPTCHA from "react-google-recaptcha";

export default function CriarUsuario({ language }) {
    const [NomeUsuario, setNomeUsuario] = useState('');
    const [Senha, setSenha] = useState('');
    const [NomeCompleto, setNomeCompleto] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [captchaOk, setCaptchaOk] = useState(false);
    const [erroMensagem, setErroMensagem] = useState('');
    const navigate = useNavigate();

    async function handleCreateUser(e) {
        e.preventDefault();

        if (!NomeUsuario || !Senha || !NomeCompleto) {
            setErroMensagem(language === 'pt' ? 'Todos os campos são obrigatórios!' : 'All fields are required!');
            return;
        }
        if (!/^[a-zA-Z\s]+$/.test(NomeCompleto)) {
            setErroMensagem(language === 'pt' ? 'O campo Nome Completo deve conter apenas letras!' : 'The Full Name field must contain only letters!');
            return;
        }
        if (!captchaOk) {
            setErroMensagem(language === 'pt' ? 'Confirme o captcha!' : 'Please confirm the captcha!');
            return;
        }
        setErroMensagem('');
        const queryParams = new URLSearchParams({
            NomeUsuario,
            Senha,
            NomeCompleto
        }).toString();

        try {
            await api.post(`api/auth/v1?${queryParams}`);
            navigate('/login');
        } catch (error) {
            if (
                error.response &&
                error.response.data === 'Erro de validação: Já existe um usuário cadastrado com este NomeUsuario.'
            ) {
                setErroMensagem(language === 'pt'
                    ? 'Já existe um usuário cadastrado com este Nome de Usuário.'
                    : 'A user with this username already exists.');
            } else {
                setErroMensagem(language === 'pt'
                    ? 'Erro ao criar usuário, tente novamente!'
                    : 'Error creating user, please try again!');
            }
        }
    }

    return (
        <div className="login-bg d-flex align-items-center justify-content-center">
            <div className="login-card shadow-lg p-4 rounded-4">
                <div className="text-center mb-4">
                    <img src={logoImage} alt="Logo" className="login-logo mb-2" />
                    <h2 className="fw-bold text-white">{language === 'pt' ? 'Criar Usuário' : 'Create User'}</h2>
                    <p className="text-light mb-0" style={{ opacity: 0.8 }}>
                        {language === 'pt'
                            ? 'Preencha os campos para criar sua conta'
                            : 'Fill in the fields to create your account'}
                    </p>
                </div>
                {/* ALERTA DE ERRO ÚNICO */}
                {erroMensagem && (
                    <div className="alert alert-danger py-2 text-center" role="alert">
                        {erroMensagem}
                    </div>
                )}
                <form onSubmit={handleCreateUser}>
                    <div className="mb-3">
                        <input
                            type="text"
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Digite seu usuário' : 'Enter your username'}
                            value={NomeUsuario}
                            onChange={e => setNomeUsuario(e.target.value)}
                        />
                    </div>
                    <div className="mb-3 position-relative">
                        <input
                            type={showPassword ? "text" : "password"}
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Digite sua senha' : 'Enter your password'}
                            value={Senha}
                            onChange={e => setSenha(e.target.value)}
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
                    <div className="mb-3">
                        <input
                            type="text"
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Digite seu nome completo' : 'Enter your full name'}
                            value={NomeCompleto}
                            onChange={e => setNomeCompleto(e.target.value)}
                        />
                    </div>
                    <div className="mb-3 d-flex justify-content-center">
                        <ReCAPTCHA
                            sitekey={process.env.REACT_APP_RECAPTCHA_SITE_KEY}
                            onChange={() => setCaptchaOk(true)}
                            theme="dark"
                        />
                    </div>
                    <button className="btn btn-gradient w-100 py-2 mb-2" type="submit">
                        {language === 'pt' ? 'Criar' : 'Create'}
                    </button>
                    <button
                        className="btn btn-outline-light w-100 py-2"
                        type="button"
                        onClick={() => navigate('/login')}
                    >
                        {language === 'pt' ? 'Voltar ao Login' : 'Back to Login'}
                    </button>
                </form>
            </div>
        </div>
    );
}