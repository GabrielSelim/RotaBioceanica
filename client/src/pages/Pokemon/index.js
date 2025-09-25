import React, { useState, useEffect } from 'react';
import './style.css';
import api from '../../services/api';
import { useNavigate } from 'react-router-dom';
import "bootstrap/dist/css/bootstrap.min.css";

const versoesDisponiveis = [
    "Genetic Apex",
    "Mythical Island",
    "Space-Time Smackdown",
    "Triumphant Light",
    "Shining Revelry",
    "Celestial Guardians"
];

export default function Pokemon() {
    const [cartas, setCartas] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [filtroSituacao, setFiltroSituacao] = useState('');
    const [filtroVersao, setFiltroVersao] = useState('');
    const accessToken = localStorage.getItem('accessToken');
    const navigate = useNavigate();

    useEffect(() => {
        buscarMaisCartas().finally(() => {
            setIsLoading(false);
        });
    }, []);

    async function buscarMaisCartas() {
        try {
            const response = await api.get(`/v1/api/pokemon/filtrarporcriterios`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
                params: {
                    version: '1',
                    sortDirection: 'desc',
                    sortField: 'id',
                },
            });
            const cartasComSituacao = response.data.map((carta) => ({
                ...carta,
                situacao: carta.situacao || 'A',
            }));
            setCartas(cartasComSituacao);
        } catch (error) {
            console.error('Erro ao buscar cartas:', error.response || error.message);
        }
    }

    async function atualizarStatusCartas() {
        try {
            const response = await api.get(`/v1/api/pokemon/obtertodossemimagem`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });
            setCartas((prevCartas) =>
                prevCartas.map((carta) => {
                    const atualizada = response.data.find((c) => c.id === carta.id);
                    return atualizada
                        ? { ...carta, situacao: atualizada.situacao }
                        : carta;
                })
            );
        } catch (error) {
            console.error('Erro ao atualizar status das cartas:', error.response || error.message);
        }
    }

    function handleFiltroSituacaoChange(event) {
        setFiltroSituacao(event.target.value);
    }

    function handleFiltroVersaoChange(event) {
        setFiltroVersao(event.target.value);
    }

    const cartasFiltradas = cartas.filter((carta) => {
        const situacaoOk = filtroSituacao === '' || carta.situacao === filtroSituacao;
        const versaoOk = filtroVersao === '' || carta.nomeVersao === filtroVersao;
        return situacaoOk && versaoOk;
    });

    async function ativarPokemon(id) {
        try {
            const response = await api.patch(`/v1/api/pokemon/ativar/${id}`, null, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });
            if (response.status === 200) {
                await atualizarStatusCartas();
            }
        } catch (error) {
            console.error('Erro ao ativar Pokemon:', error.response || error.message);
        }
    }

    async function inativarPokemon(id) {
        try {
            const response = await api.patch(`/v1/api/pokemon/inativar/${id}`, null, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            });
            if (response.status === 200) {
                await atualizarStatusCartas();
            }
        } catch (error) {
            console.error('Erro ao inativar Pokemon:', error.response || error.message);
        }
    }

    if (isLoading) {
        return (
            <div className="pokemon-page-wrapper" style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center" }}>
                <div className="spinner-border text-info" role="status" style={{ width: 60, height: 60 }}>
                    <span className="visually-hidden">Carregando...</span>
                </div>
            </div>
        );
    }

    return (
        <div className="pokemon-page-wrapper">
            <div className="pokemon-toolbar-sticky">
                <button
                    className="btn btn-outline-secondary"
                    onClick={() => navigate(-1)}
                >
                    Voltar
                </button>
                <div className="filtros-group">
                    <div className="filtro-situacao">
                        <label htmlFor="situacao" className="filtro-label">Filtrar por Situação:</label>
                        <select
                            id="situacao"
                            value={filtroSituacao}
                            onChange={handleFiltroSituacaoChange}
                        >
                            <option value="">Todos</option>
                            <option value="A">Ativos</option>
                            <option value="I">Inativos</option>
                        </select>
                    </div>
                    <div className="filtro-versao">
                        <label htmlFor="versao" className="filtro-label">Filtrar por Versão:</label>
                        <select
                            id="versao"
                            value={filtroVersao}
                            onChange={handleFiltroVersaoChange}
                        >
                            <option value="">Todas</option>
                            {versoesDisponiveis.map((versao) => (
                                <option key={versao} value={versao}>{versao}</option>
                            ))}
                        </select>
                    </div>
                </div>
            </div>

            <div className="pokemon-cards-scroll">
                <div className="row g-3">
                    {cartasFiltradas.map((carta) => (
                        <div
                            key={carta.id}
                            className="col-4 d-flex"
                        >
                            <div className={`pokemon-card flex-fill ${carta.situacao === 'A' ? 'active' : 'inactive'}`}>
                                <p><strong>{carta.nomePokemon}</strong></p>
                                <img
                                    src={`data:image/png;base64,${carta.imagem}`}
                                    alt={carta.nomePokemon}
                                    className="pokemon-card-image"
                                />
                                <div className="pokemon-card-buttons">
                                    {carta.situacao === 'A' ? (
                                        <button onClick={() => inativarPokemon(carta.id)} className="inativar-button">
                                            Inativar
                                        </button>
                                    ) : (
                                        <button onClick={() => ativarPokemon(carta.id)} className="ativar-button">
                                            Ativar
                                        </button>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}