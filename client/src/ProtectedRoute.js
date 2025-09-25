import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import api from './services/api';

export default function ProtectedRoute({ children, requiredRole }) {
    const [valid, setValid] = useState(null);
    const accessToken = localStorage.getItem('accessToken');
    const userRole = localStorage.getItem('userRole');

    useEffect(() => {
        async function checkToken() {
            if (!accessToken) {
                setValid(false);
                return;
            }
            try {
                const res = await api.get('/api/auth/v1/validate-token', {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                setValid(res.data.valid === true);
            } catch {
                setValid(false);
            }
        }
        checkToken();
    }, [accessToken]);

    if (valid === null) return null; // ou um spinner

    if (!valid) {
        localStorage.clear();
        return <Navigate to="/login" />;
    }

    if (requiredRole && userRole !== requiredRole) {
        return <Navigate to="/" />;
    }

    return children;
}