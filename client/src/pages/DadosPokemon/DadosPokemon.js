import React, { useEffect, useState } from "react";
import api from "../../services/api";
import "./DadosPokemon.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { useNavigate } from "react-router-dom";

// Ordem fixa dos boosters conforme sua planilha
const BOOSTERS_FIXOS = [
    "Qualquer",
    "Mewtwo",
    "Pikachu",
    "Charizard",
    "Mew",
    "Dialga",
    "Palkia",
    "Arceus",
    "Shinny Charizard",
    "Solgaleo",
    "Lunala"
];

// Ordem fixa das raridades conforme sua planilha
const RARIDADES_FIXAS = [
    "Quatro Diamantes",
    "Tres Diamantes",
    "Dois Diamantes",
    "Diamante",
    "Estrela",
    "Duas Estrelas",
    "Tres Estrelas",
    "Rei",
    "Shinny",
    "Duas Shinnys"
];

export default function DadosPokemon() {
    const [dados, setDados] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        async function fetchData() {
            try {
                const accessToken = localStorage.getItem("accessToken");
                const response = await api.get("/v1/api/pokemon/obtertodossemimagem", {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                });
                // Só cartas ativas
                const ativos = response.data.filter((item) => item.situacao === "A");
                setDados(ativos);
            } catch (error) {
                alert("Erro ao buscar dados dos pokémons.");
            }
            setLoading(false);
        }
        fetchData();
    }, []);

    // Função para contar total por raridade
    function totalPorRaridade(raridade) {
        return dados.filter((item) => item.raridade === raridade).length;
    }

    // Função para contar por booster e raridade
    function totalPorRaridadeBooster(raridade, booster) {
        return dados.filter(
            (item) => item.raridade === raridade && item.booster === booster
        ).length;
    }

    // Total geral
    const totalGeral = dados.length;
    // Total por booster
    function totalPorBooster(booster) {
        return dados.filter((item) => item.booster === booster).length;
    }

    return (
        <div className="dados-pokemon-bg py-4">
            <div className="container">
                <div className="d-flex justify-content-between align-items-center mb-4">
                    <div>
                        <button
                            className="btn btn-primary me-2"
                            onClick={() => navigate("/pokemon/alterarsituacao")}
                        >
                            Alterar Situação 
                        </button>
                        <button
                            className="btn btn-primary me-2"
                            onClick={() => navigate("/pokemon/incluirPokemon")}
                        >
                            Incluir Pokemon
                        </button>
                    </div>
                </div>
                {loading ? (
                    <div className="text-center py-5">
                        <div className="spinner-border text-info" role="status"></div>
                    </div>
                ) : (
                    <div className="table-responsive">
                        <table className="table table-bordered align-middle dados-pokemon-table">
                            <thead>
                                <tr>
                                    <th className="sticky-col th-raridade">Raridade</th>
                                    <th className="th-total">Total</th>
                                    {BOOSTERS_FIXOS.map((booster, idx) => (
                                        <th key={booster} className={`th-booster th-booster-${idx}`}>{booster}</th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {RARIDADES_FIXAS.map((raridade) => (
                                    <tr key={raridade}>
                                        <td className="sticky-col th-raridade">{raridade}</td>
                                        <td className="th-total">
                                            <span className="badge bg-info text-dark">
                                                {totalPorRaridade(raridade)}
                                            </span>
                                        </td>
                                        {BOOSTERS_FIXOS.map((booster, idx) => (
                                            <td key={booster} className={`th-booster th-booster-${idx}`}>
                                                {totalPorRaridadeBooster(raridade, booster) > 0 ? (
                                                    <span className="badge bg-success">
                                                        {totalPorRaridadeBooster(raridade, booster)}
                                                    </span>
                                                ) : (
                                                    <span className="text-muted">0</span>
                                                )}
                                            </td>
                                        ))}
                                    </tr>
                                ))}
                            </tbody>
                            <tfoot>
                                <tr>
                                    <th className="sticky-col th-raridade">Total Geral</th>
                                    <th className="th-total">
                                        <span className="badge bg-primary">{totalGeral}</span>
                                    </th>
                                    {BOOSTERS_FIXOS.map((booster, idx) => (
                                        <th key={booster} className={`th-booster th-booster-${idx}`}>
                                            <span className="badge bg-primary">{totalPorBooster(booster)}</span>
                                        </th>
                                    ))}
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                )}
            </div>
        </div>
    );
}