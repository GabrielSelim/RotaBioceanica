import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Table, Button, Spinner, Alert } from "react-bootstrap";
import "./parcelamentoMensalFinanca.css";
import api from "../../services/api";

const textos = {
    titulo: { pt: "Parcelas do Parcelamento", en: "Installment Details" },
    voltar: { pt: "← Voltar", en: "← Back" },
    numeroParcela: { pt: "Nº Parcela", en: "Installment Nº" },
    dataVencimento: { pt: "Data de Vencimento", en: "Due Date" },
    dataPagamento: { pt: "Data de Pagamento", en: "Payment Date" },
    valorParcela: { pt: "Valor Parcela", en: "Installment Value" },
    valorPago: { pt: "Valor Pago", en: "Paid Value" },
    situacao: { pt: "Situação", en: "Status" },
    carregando: { pt: "Carregando...", en: "Loading..." },
    erro: { pt: "Erro ao carregar parcelas.", en: "Error loading installments." },
    nenhum: { pt: "Nenhuma parcela encontrada.", en: "No installments found." },
    pago: { pt: "Pago", en: "Paid" },
    apagar: { pt: "A Pagar", en: "To Pay" }
};

export default function ParcelamentoMensalFinanca({ language = "pt" }) {
    const { id } = useParams();
    const [parcelas, setParcelas] = useState([]);
    const [loading, setLoading] = useState(true);
    const [mensagem, setMensagem] = useState("");
    const [mensagemTipo, setMensagemTipo] = useState("success");
    const navigate = useNavigate();

    useEffect(() => {
        async function fetchParcelas() {
            setLoading(true);
            try {
                const accessToken = localStorage.getItem("accessToken");
                const res = await api.get(`/v1/api/parcelamento/${id}/mensais`, {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                setParcelas(Array.isArray(res.data) ? res.data : []);
                setMensagem("");
            } catch {
                setMensagem(textos.erro[language]);
                setMensagemTipo("danger");
            }
            setLoading(false);
        }
        fetchParcelas();
        // eslint-disable-next-line
    }, [id, language]);

    // Função para verificar se a situação é "paga" ou "pago" (case-insensitive)
    function isPago(situacao) {
        if (!situacao) return false;
        const s = situacao.trim().toLowerCase();
        return s === "paga" || s === "pago";
    }

    return (
        <div className="parcelas-bg py-5">
            <div className="container parcelas-container shadow-lg rounded-4 p-4">
                <Button variant="outline-secondary" className="mb-3" onClick={() => navigate(-1)}>
                    {textos.voltar[language]}
                </Button>
                <h2 className="fw-bold text-primary mb-4">{textos.titulo[language]}</h2>
                {mensagem && <Alert variant={mensagemTipo}>{mensagem}</Alert>}
                {loading ? (
                    <div className="text-center my-5"><Spinner animation="border" /></div>
                ) : (
                    <div className="table-responsive">
                        <Table bordered hover className="parcelas-table align-middle">
                            <thead>
                                <tr>
                                    <th>{textos.numeroParcela[language]}</th>
                                    <th>{textos.dataVencimento[language]}</th>
                                    <th>{textos.dataPagamento[language]}</th>
                                    <th>{textos.valorParcela[language]}</th>
                                    <th>{textos.valorPago[language]}</th>
                                    <th>{textos.situacao[language]}</th>
                                </tr>
                            </thead>
                            <tbody>
                                {parcelas.length === 0 ? (
                                    <tr>
                                        <td colSpan={6} className="text-center text-secondary">
                                            {textos.nenhum[language]}
                                        </td>
                                    </tr>
                                ) : (
                                    parcelas.map((parc) => (
                                        <tr key={parc.id}>
                                            <td>{parc.numeroParcela}</td>
                                            <td>{parc.dataVencimento ? new Date(parc.dataVencimento).toLocaleDateString(language === "pt" ? "pt-BR" : "en-US") : "-"}</td>
                                            <td>{parc.dataPagamento ? new Date(parc.dataPagamento).toLocaleDateString(language === "pt" ? "pt-BR" : "en-US") : "-"}</td>
                                            <td>R$ {Number(parc.valorParcela).toLocaleString("pt-BR", { minimumFractionDigits: 2 })}</td>
                                            <td>R$ {Number(parc.valorPago).toLocaleString("pt-BR", { minimumFractionDigits: 2 })}</td>
                                            <td>
                                                <span className={isPago(parc.situacao) ? "status-pago" : "status-apagar"}>
                                                    {isPago(parc.situacao)
                                                        ? textos.pago[language]
                                                        : textos.apagar[language]}
                                                </span>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </Table>
                    </div>
                )}
            </div>
        </div>
    );
}