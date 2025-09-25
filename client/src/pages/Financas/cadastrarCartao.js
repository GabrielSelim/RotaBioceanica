import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../services/api";
import "./cadastrarCartao.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { FiEdit2, FiTrash2 } from "react-icons/fi";
import { Modal } from "react-bootstrap";

const textos = {
    titulo: { pt: "Adicionar Novo Cartão", en: "Add New Card" },
    nomeUsuario: { pt: "Nome do Usuário do Banco", en: "Bank Account Holder Name" },
    nomeBanco: { pt: "Nome do Banco", en: "Bank Name" },
    salvar: { pt: "Salvar Cartão", en: "Save Card" },
    editar: { pt: "Editar", en: "Edit" },
    excluir: { pt: "Excluir", en: "Delete" },
    sucesso: { pt: "Cartão cadastrado com sucesso!", en: "Card successfully registered!" },
    sucessoEditar: { pt: "Cartão editado com sucesso!", en: "Card successfully edited!" },
    sucessoExcluir: { pt: "Cartão excluído com sucesso!", en: "Card successfully deleted!" },
    erroCampos: { pt: "Preencha todos os campos obrigatórios.", en: "Fill in all required fields." },
    erroBuscar: { pt: "Erro ao buscar cartões.", en: "Error fetching cards." },
    erroCadastrar: { pt: "Erro ao cadastrar cartão.", en: "Error registering card." },
    erroEditar: { pt: "Erro ao editar cartão.", en: "Error editing card." },
    erroExcluir: { pt: "Erro ao excluir cartão.", en: "Error deleting card." },
    buscar: { pt: "Buscar", en: "Search" },
    buscando: { pt: "Buscando...", en: "Searching..." },
    listaTitulo: { pt: "Cartões Cadastrados", en: "Registered Cards" },
    buscarPlaceholder: { pt: "Buscar por usuário do banco...", en: "Search by account holder..." },
    nenhumCartao: { pt: "Nenhum cartão cadastrado.", en: "No cards registered." },
    voltar: { pt: "← Voltar ao Painel Financeiro", en: "← Back to Finance Panel" },
    confirmarExclusao: { pt: "Deseja realmente excluir este cartão?", en: "Do you really want to delete this card?" },
    cancelar: { pt: "Cancelar", en: "Cancel" },
    salvarEdicao: { pt: "Salvar Edição", en: "Save Edit" }
};

export default function CadastrarCartao({ language = "pt" }) {
    const [nomeUsuario, setNomeUsuario] = useState("");
    const [nomeBanco, setNomeBanco] = useState("");
    const [cartoes, setCartoes] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [mensagem, setMensagem] = useState(null);
    const [mensagemTipo, setMensagemTipo] = useState("success");
    const [busca, setBusca] = useState("");
    const [buscando, setBuscando] = useState(false);
    const [editandoId, setEditandoId] = useState(null);
    const [editNomeUsuario, setEditNomeUsuario] = useState("");
    const [editNomeBanco, setEditNomeBanco] = useState("");
    const [excluindoId, setExcluindoId] = useState(null);

    // Para modal de exclusão
    const [showDelete, setShowDelete] = useState(false);
    const [deleteId, setDeleteId] = useState(null);

    const accessToken = localStorage.getItem("accessToken");
    const navigate = useNavigate();

    useEffect(() => {
        buscarCartoes();
        // eslint-disable-next-line
    }, []);

    async function buscarCartoes() {
        setIsLoading(true);
        try {
            const response = await api.get("/v1/api/cartao", {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setCartoes(response.data);
            setMensagem(null);
        } catch (error) {
            if (error.response && error.response.status === 404) {
                setCartoes([]);
                setMensagem(null);
            } else {
                setMensagem(textos.erroBuscar[language]);
                setMensagemTipo("danger");
            }
        }
        setIsLoading(false);
    }

    async function buscarPorUsuario(e) {
        e.preventDefault();
        if (!busca.trim()) {
            buscarCartoes();
            return;
        }
        setBuscando(true);
        try {
            const response = await api.get(`/v1/api/cartao/usuario/${encodeURIComponent(busca.trim())}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setCartoes(response.data);
            setMensagem(null);
        } catch (error) {
            if (error.response && error.response.status === 404) {
                setCartoes([]);
                setMensagem(null);
            } else {
                setMensagem(textos.erroBuscar[language]);
                setMensagemTipo("danger");
            }
        }
        setBuscando(false);
    }

    async function handleSalvarCartao(e) {
        e.preventDefault();
        if (!nomeUsuario.trim() || !nomeBanco.trim()) {
            setMensagem(textos.erroCampos[language]);
            setMensagemTipo("danger");
            return;
        }
        setIsSaving(true);
        try {
            await api.post(
                "/v1/api/cartao",
                { nomeUsuario: nomeUsuario.trim(), nomeBanco: nomeBanco.trim() },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            setMensagem(textos.sucesso[language]);
            setMensagemTipo("success");
            setNomeUsuario("");
            setNomeBanco("");
            buscarCartoes();
        } catch (error) {
            setMensagem(textos.erroCadastrar[language]);
            setMensagemTipo("danger");
        }
        setIsSaving(false);
    }

    function iniciarEdicao(cartao) {
        setEditandoId(cartao.id);
        setEditNomeUsuario(cartao.nomeUsuario);
        setEditNomeBanco(cartao.nomeBanco);
        setMensagem(null);
    }

    function cancelarEdicao() {
        setEditandoId(null);
        setEditNomeUsuario("");
        setEditNomeBanco("");
    }

    async function salvarEdicaoCartao(id) {
        if (!editNomeUsuario.trim() || !editNomeBanco.trim()) {
            setMensagem(textos.erroCampos[language]);
            setMensagemTipo("danger");
            return;
        }
        setIsSaving(true);
        try {
            await api.patch(
                "/v1/api/cartao",
                { id, nomeUsuario: editNomeUsuario.trim(), nomeBanco: editNomeBanco.trim() },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            setMensagem(textos.sucessoEditar[language]);
            setMensagemTipo("success");
            setEditandoId(null);
            setEditNomeUsuario("");
            setEditNomeBanco("");
            buscarCartoes();
        } catch (error) {
            setMensagem(textos.erroEditar[language]);
            setMensagemTipo("danger");
        }
        setIsSaving(false);
    }

    // Modal de exclusão
    function abrirDelete(id) {
        setDeleteId(id);
        setShowDelete(true);
    }
    function fecharDelete() {
        setDeleteId(null);
        setShowDelete(false);
    }
    async function excluirCartaoConfirmado() {
        setExcluindoId(deleteId);
        try {
            await api.delete(`/v1/api/cartao/${deleteId}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setMensagem(textos.sucessoExcluir[language]);
            setMensagemTipo("success");
            buscarCartoes();
        } catch (error) {
            setMensagem(textos.erroExcluir[language]);
            setMensagemTipo("danger");
        }
        setExcluindoId(null);
        fecharDelete();
    }

    return (
        <div className="cartao-bg py-5">
            <div className="container cartao-container shadow-lg rounded-4 p-4">
                <button
                    className="btn btn-outline-secondary mb-3"
                    onClick={() => navigate("/financas")}
                >
                    {textos.voltar[language]}
                </button>
                <div className="row g-4 cartao-row">
                    {/* Seção 1: Formulário */}
                    <div className="col-12 col-lg-5">
                        <div className="card p-4 mb-4 cartao-form-card">
                            <h4 className="fw-bold mb-3 text-primary">{textos.titulo[language]}</h4>
                            <form onSubmit={handleSalvarCartao}>
                                <div className="mb-3">
                                    <label className="form-label fw-semibold cartao-label">
                                        {textos.nomeUsuario[language]} <span className="text-danger">*</span>
                                    </label>
                                    <input
                                        type="text"
                                        className={`form-control ${!nomeUsuario && mensagem && mensagemTipo === "danger" ? "is-invalid" : ""}`}
                                        value={nomeUsuario}
                                        onChange={e => setNomeUsuario(e.target.value)}
                                        placeholder={language === "pt" ? "Ex: João Silva" : "Ex: John Smith"}
                                        maxLength={40}
                                        required
                                    />
                                </div>
                                <div className="mb-3">
                                    <label className="form-label fw-semibold cartao-label">
                                        {textos.nomeBanco[language]} <span className="text-danger">*</span>
                                    </label>
                                    <input
                                        type="text"
                                        className={`form-control ${!nomeBanco && mensagem && mensagemTipo === "danger" ? "is-invalid" : ""}`}
                                        value={nomeBanco}
                                        onChange={e => setNomeBanco(e.target.value)}
                                        placeholder={language === "pt" ? "Ex: Banco do Brasil" : "Ex: Bank of America"}
                                        maxLength={40}
                                        required
                                    />
                                </div>
                                <button
                                    type="submit"
                                    className="btn btn-gradient w-100 fw-bold"
                                    disabled={isSaving}
                                >
                                    {isSaving ? (language === "pt" ? "Salvando..." : "Saving...") : textos.salvar[language]}
                                </button>
                            </form>
                            {mensagem && (
                                <div className={`alert mt-3 alert-${mensagemTipo} py-2 text-center`} role="alert">
                                    {mensagem}
                                </div>
                            )}
                        </div>
                    </div>
                    {/* Seção 2: Lista */}
                    <div className="col-12 col-lg-7">
                        <div className="card p-4 cartao-list-card h-100">
                            <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                                <h4 className="fw-bold text-primary mb-0">{textos.listaTitulo[language]}</h4>
                                <form className="d-flex" onSubmit={buscarPorUsuario}>
                                    <input
                                        type="text"
                                        className="form-control form-control-sm me-2"
                                        placeholder={textos.buscarPlaceholder[language]}
                                        value={busca}
                                        onChange={e => setBusca(e.target.value)}
                                        style={{ minWidth: 180 }}
                                    />
                                    <button className="btn btn-outline-info btn-sm" type="submit" disabled={buscando}>
                                        {buscando ? textos.buscando[language] : textos.buscar[language]}
                                    </button>
                                </form>
                            </div>
                            {isLoading ? (
                                <div className="text-center py-5">
                                    <div className="spinner-border text-info" role="status"></div>
                                </div>
                            ) : (
                                <div className="table-responsive">
                                    <table className="table table-bordered align-middle cartao-table">
                                        <thead>
                                            <tr>
                                                <th>{textos.nomeUsuario[language]}</th>
                                                <th>{textos.nomeBanco[language]}</th>
                                                <th className="text-center">{textos.editar[language] + " / " + textos.excluir[language]}</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {cartoes.length === 0 ? (
                                                <tr>
                                                    <td colSpan={3} className="text-center text-secondary">
                                                        {textos.nenhumCartao[language]}
                                                    </td>
                                                </tr>
                                            ) : (
                                                cartoes.map((cartao) => (
                                                    <tr key={cartao.id}>
                                                        {editandoId === cartao.id ? (
                                                            <>
                                                                <td>
                                                                    <input
                                                                        type="text"
                                                                        className="form-control form-control-sm"
                                                                        value={editNomeUsuario}
                                                                        onChange={e => setEditNomeUsuario(e.target.value)}
                                                                        maxLength={40}
                                                                    />
                                                                </td>
                                                                <td>
                                                                    <input
                                                                        type="text"
                                                                        className="form-control form-control-sm"
                                                                        value={editNomeBanco}
                                                                        onChange={e => setEditNomeBanco(e.target.value)}
                                                                        maxLength={40}
                                                                    />
                                                                </td>
                                                                <td className="text-center">
                                                                    <button
                                                                        className="btn btn-success btn-sm me-2"
                                                                        onClick={() => salvarEdicaoCartao(cartao.id)}
                                                                        disabled={isSaving}
                                                                        type="button"
                                                                    >
                                                                        {textos.salvarEdicao[language]}
                                                                    </button>
                                                                    <button
                                                                        className="btn btn-secondary btn-sm"
                                                                        onClick={cancelarEdicao}
                                                                        type="button"
                                                                    >
                                                                        {textos.cancelar[language]}
                                                                    </button>
                                                                </td>
                                                            </>
                                                        ) : (
                                                            <>
                                                                <td>{cartao.nomeUsuario}</td>
                                                                <td>{cartao.nomeBanco}</td>
                                                                <td className="text-center">
                                                                    <button
                                                                        className="btn btn-warning btn-sm me-2"
                                                                        title={textos.editar[language]}
                                                                        onClick={() => iniciarEdicao(cartao)}
                                                                        type="button"
                                                                    >
                                                                        <FiEdit2 />
                                                                    </button>
                                                                    <button
                                                                        className="btn btn-danger btn-sm"
                                                                        title={textos.excluir[language]}
                                                                        onClick={() => abrirDelete(cartao.id)}
                                                                        disabled={excluindoId === cartao.id}
                                                                        type="button"
                                                                    >
                                                                        <FiTrash2 />
                                                                    </button>
                                                                </td>
                                                            </>
                                                        )}
                                                    </tr>
                                                ))
                                            )}
                                        </tbody>
                                    </table>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
            {/* Modal de exclusão */}
            <Modal show={showDelete} onHide={fecharDelete} centered>
                <Modal.Header closeButton>
                    <Modal.Title>{textos.excluir[language]}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>{textos.confirmarExclusao[language]}</p>
                </Modal.Body>
                <Modal.Footer>
                    <button className="btn btn-secondary" onClick={fecharDelete}>
                        {textos.cancelar[language]}
                    </button>
                    <button className="btn btn-danger" onClick={excluirCartaoConfirmado} disabled={excluindoId === deleteId}>
                        {textos.excluir[language]}
                    </button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}