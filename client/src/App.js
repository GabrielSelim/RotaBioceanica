import React, { useState, useEffect } from 'react';
import AppRoutes from './routes';
import api from './services/api';

export default function App() {
    const [language, setLanguage] = useState('pt');
    const [tokenChecked, setTokenChecked] = useState(false);

    useEffect(() => {
        async function validateToken() {
            const accessToken = localStorage.getItem('accessToken');
            if (!accessToken) {
                setTokenChecked(true);
                return;
            }
            try {
                const res = await api.get('/api/auth/v1/validate-token', {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                if (res.data.valid !== true) {
                    localStorage.clear();
                    window.location.href = '/login';
                } else {
                    setTokenChecked(true);
                }
            } catch {
                localStorage.clear();
                window.location.href = '/login';
            }
        }
        validateToken();
    }, []);

    if (!tokenChecked) {
        // Mostra um fundo padrão e spinner enquanto carrega
        return (
            <div style={{
                minHeight: "100vh",
                width: "100vw",
                background: "linear-gradient(135deg, #0f2027 0%, #2c5364 50%, #00c6ff 100%)",
                display: "flex",
                alignItems: "center",
                justifyContent: "center"
            }}>
                <div className="spinner-border text-info" role="status" style={{ width: 60, height: 60 }}>
                    <span className="visually-hidden">Carregando...</span>
                </div>
            </div>
        );
    }

    return <AppRoutes language={language} setLanguage={setLanguage} />;
}