import React, { useEffect, useState } from "react";
import api from "../../services/api";
import { useNavigate } from "react-router-dom";
import "./pessoaConta.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { FiEdit2, FiTrash2 } from "react-icons/fi";
import { Modal } from "react-bootstrap";

const textos = {
    voltar: { pt: "← Voltar ao Painel Financeiro", en: "← Back to Finance Panel" },
    titulo: { pt: "Adicionar Nova Conta de Pessoa", en: "Add New Account Holder" },
    nomePessoa: { pt: "Nome da Pessoa/Titular da Conta", en: "Account Holder Name" },
    salvar: { pt: "Salvar Conta", en: "Save Account" },
    salvando: { pt: "Salvando...", en: "Saving..." },
    sucesso: { pt: "Conta cadastrada com sucesso!", en: "Account successfully registered!" },
    sucessoEditar: { pt: "Conta editada com sucesso!", en: "Account successfully edited!" },
    sucessoExcluir: { pt: "Conta excluída com sucesso!", en: "Account successfully deleted!" },
    erroCampos: { pt: "Preencha o nome da pessoa.", en: "Fill in the person's name." },
    erroBuscar: { pt: "Erro ao buscar contas.", en: "Error fetching accounts." },
    erroCadastrar: { pt: "Erro ao cadastrar conta.", en: "Error registering account." },
    erroEditar: { pt: "Erro ao editar conta.", en: "Error editing account." },
    erroExcluir: { pt: "Erro ao excluir conta.", en: "Error deleting account." },
    listaTitulo: { pt: "Contas Cadastradas", en: "Registered Accounts" },
    buscar: { pt: "Buscar", en: "Search" },
    buscando: { pt: "Buscando...", en: "Searching..." },
    buscarPlaceholder: { pt: "Buscar por nome da pessoa...", en: "Search by account holder name..." },
    nenhumaConta: { pt: "Nenhuma conta cadastrada.", en: "No accounts registered." },
    editar: { pt: "Editar", en: "Edit" },
    excluir: { pt: "Excluir", en: "Delete" },
    cancelar: { pt: "Cancelar", en: "Cancel" },
    salvarEdicao: { pt: "Salvar Edição", en: "Save Edit" },
    confirmarExclusao: { pt: "Deseja realmente excluir esta conta?", en: "Do you really want to delete this account?" }
};

export default function PessoaConta({ language = "pt" }) {
    const [nomePessoa, setNomePessoa] = useState("");
    const [contas, setContas] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [mensagem, setMensagem] = useState(null);
    const [mensagemTipo, setMensagemTipo] = useState("success");
    const [busca, setBusca] = useState("");
    const [buscando, setBuscando] = useState(false);
    const [editandoId, setEditandoId] = useState(null);
    const [editNomePessoa, setEditNomePessoa] = useState("");
    const [excluindoId, setExcluindoId] = useState(null);

    // Para modal de exclusão
    const [showDelete, setShowDelete] = useState(false);
    const [deleteId, setDeleteId] = useState(null);

    const accessToken = localStorage.getItem("accessToken");
    const navigate = useNavigate();

    useEffect(() => {
        buscarContas();
        // eslint-disable-next-line
    }, []);

    async function buscarContas() {
        setIsLoading(true);
        try {
            const response = await api.get("/v1/api/pessoaconta", {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setContas(response.data);
            setMensagem(null);
        } catch (error) {
            if (error.response && error.response.status === 404) {
                setContas([]);
                setMensagem(null);
            } else {
                setMensagem(textos.erroBuscar[language]);
                setMensagemTipo("danger");
            }
        }
        setIsLoading(false);
    }

    async function buscarPorNome(e) {
        e.preventDefault();
        if (!busca.trim()) {
            buscarContas();
            return;
        }
        setBuscando(true);
        try {
            const response = await api.get(`/v1/api/pessoaconta/nome/${encodeURIComponent(busca.trim())}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setContas(response.data);
            setMensagem(null);
        } catch (error) {
            if (error.response && error.response.status === 404) {
                setContas([]);
                setMensagem(null);
            } else {
                setMensagem(textos.erroBuscar[language]);
                setMensagemTipo("danger");
            }
        }
        setBuscando(false);
    }

    async function handleSalvarConta(e) {
        e.preventDefault();
        if (!nomePessoa.trim()) {
            setMensagem(textos.erroCampos[language]);
            setMensagemTipo("danger");
            return;
        }
        setIsSaving(true);
        try {
            await api.post(
                "/v1/api/pessoaconta",
                { nomePessoa: nomePessoa.trim() },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            setMensagem(textos.sucesso[language]);
            setMensagemTipo("success");
            setNomePessoa("");
            buscarContas();
        } catch (error) {
            setMensagem(textos.erroCadastrar[language]);
            setMensagemTipo("danger");
        }
        setIsSaving(false);
    }

    function iniciarEdicao(conta) {
        setEditandoId(conta.id);
        setEditNomePessoa(conta.nomePessoa);
        setMensagem(null);
    }

    function cancelarEdicao() {
        setEditandoId(null);
        setEditNomePessoa("");
    }

    async function salvarEdicaoConta(id) {
        if (!editNomePessoa.trim()) {
            setMensagem(textos.erroCampos[language]);
            setMensagemTipo("danger");
            return;
        }
        setIsSaving(true);
        try {
            await api.patch(
                "/v1/api/pessoaconta",
                { Id: id, NomePessoa: editNomePessoa.trim() },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            setMensagem(textos.sucessoEditar[language]);
            setMensagemTipo("success");
            setEditandoId(null);
            setEditNomePessoa("");
            buscarContas();
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
    async function excluirContaConfirmada() {
        setExcluindoId(deleteId);
        try {
            await api.delete(`/v1/api/pessoaconta/${deleteId}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setMensagem(textos.sucessoExcluir[language]);
            setMensagemTipo("success");
            buscarContas();
        } catch (error) {
            setMensagem(textos.erroExcluir[language]);
            setMensagemTipo("danger");
        }
        setExcluindoId(null);
        fecharDelete();
    }

    return (
        <div className="pessoaconta-bg py-5">
            <div className="container pessoaconta-container shadow-lg rounded-4 p-4">
                <button
                    className="btn btn-outline-secondary mb-3"
                    onClick={() => navigate("/financas")}
                >
                    {textos.voltar[language]}
                </button>
                <div className="row g-4 pessoaconta-row">
                    {/* Seção 1: Formulário */}
                    <div className="col-12 col-lg-5">
                        <div className="card p-4 mb-4 pessoaconta-form-card">
                            <h4 className="fw-bold mb-3 text-primary">{textos.titulo[language]}</h4>
                            <form onSubmit={handleSalvarConta}>
                                <div className="mb-3">
                                    <label className="form-label fw-semibold pessoaconta-label">
                                        {textos.nomePessoa[language]} <span className="text-danger">*</span>
                                    </label>
                                    <input
                                        type="text"
                                        className={`form-control ${!nomePessoa && mensagem && mensagemTipo === "danger" ? "is-invalid" : ""}`}
                                        value={nomePessoa}
                                        onChange={e => setNomePessoa(e.target.value)}
                                        placeholder={language === "pt" ? "Ex: Maria Oliveira" : "Ex: Mary Smith"}
                                        maxLength={60}
                                        required
                                    />
                                </div>
                                <button
                                    type="submit"
                                    className="btn btn-gradient w-100 fw-bold"
                                    disabled={isSaving}
                                >
                                    {isSaving ? textos.salvando[language] : textos.salvar[language]}
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
                        <div className="card p-4 pessoaconta-list-card h-100">
                            <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                                <h4 className="fw-bold text-primary mb-0">{textos.listaTitulo[language]}</h4>
                                <form className="d-flex" onSubmit={buscarPorNome}>
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
                                    <table className="table table-bordered align-middle pessoaconta-table">
                                        <thead>
                                            <tr>
                                                <th>{textos.nomePessoa[language]}</th>
                                                <th className="text-center">{textos.editar[language] + " / " + textos.excluir[language]}</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {contas.length === 0 ? (
                                                <tr>
                                                    <td colSpan={2} className="text-center text-secondary">
                                                        {textos.nenhumaConta[language]}
                                                    </td>
                                                </tr>
                                            ) : (
                                                contas.map((conta) => (
                                                    <tr key={conta.id}>
                                                        {editandoId === conta.id ? (
                                                            <>
                                                                <td>
                                                                    <input
                                                                        type="text"
                                                                        className="form-control form-control-sm"
                                                                        value={editNomePessoa}
                                                                        onChange={e => setEditNomePessoa(e.target.value)}
                                                                        maxLength={60}
                                                                    />
                                                                </td>
                                                                <td className="text-center">
                                                                    <button
                                                                        className="btn btn-success btn-sm me-2"
                                                                        onClick={() => salvarEdicaoConta(conta.id)}
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
                                                                <td>{conta.nomePessoa}</td>
                                                                <td className="text-center">
                                                                    <button
                                                                        className="btn btn-warning btn-sm me-2"
                                                                        title={textos.editar[language]}
                                                                        onClick={() => iniciarEdicao(conta)}
                                                                        type="button"
                                                                    >
                                                                        <FiEdit2 />
                                                                    </button>
                                                                    <button
                                                                        className="btn btn-danger btn-sm"
                                                                        title={textos.excluir[language]}
                                                                        onClick={() => abrirDelete(conta.id)}
                                                                        disabled={excluindoId === conta.id}
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
                    <button className="btn btn-danger" onClick={excluirContaConfirmada} disabled={excluindoId === deleteId}>
                        {textos.excluir[language]}
                    </button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}