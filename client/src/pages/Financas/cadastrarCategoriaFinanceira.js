import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../services/api";
import "./cadastrarCategoriaFinanceira.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { FiEdit2, FiTrash2 } from "react-icons/fi";
import { Modal } from "react-bootstrap";

const textos = {
    voltar: { pt: "← Voltar ao Painel Financeiro", en: "← Back to Finance Panel" },
    titulo: { pt: "Adicionar Nova Categoria", en: "Add New Category" },
    nomeCategoria: { pt: "Nome da Categoria", en: "Category Name" },
    tipoCategoria: { pt: "Tipo de Categoria", en: "Category Type" },
    receita: { pt: "Receita", en: "Income" },
    despesa: { pt: "Despesa", en: "Expense" },
    salvar: { pt: "Salvar Categoria", en: "Save Category" },
    salvando: { pt: "Salvando...", en: "Saving..." },
    sucesso: { pt: "Categoria cadastrada com sucesso!", en: "Category successfully registered!" },
    sucessoEditar: { pt: "Categoria editada com sucesso!", en: "Category successfully edited!" },
    sucessoExcluir: { pt: "Categoria excluída com sucesso!", en: "Category successfully deleted!" },
    erroCampos: { pt: "Preencha todos os campos obrigatórios.", en: "Fill in all required fields." },
    erroBuscar: { pt: "Erro ao buscar categorias.", en: "Error fetching categories." },
    erroCadastrar: { pt: "Erro ao cadastrar categoria.", en: "Error registering category." },
    erroEditar: { pt: "Erro ao editar categoria.", en: "Error editing category." },
    erroExcluir: { pt: "Erro ao excluir categoria.", en: "Error deleting category." },
    listaTitulo: { pt: "Categorias Cadastradas", en: "Registered Categories" },
    receitasPrimeiro: { pt: "Receitas Primeiro", en: "Income First" },
    despesasPrimeiro: { pt: "Despesas Primeiro", en: "Expense First" },
    ordemPadrao: { pt: "Ordem Padrão", en: "Default Order" },
    nenhumaCategoria: { pt: "Nenhuma categoria cadastrada.", en: "No categories registered." },
    nomeExemplo: { pt: "Ex: Alimentação, Salário...", en: "Ex: Food, Salary..." },
    tipoObrigatorio: { pt: "Tipo de Categoria é obrigatório.", en: "Category Type is required." },
    editar: { pt: "Editar", en: "Edit" },
    excluir: { pt: "Excluir", en: "Delete" },
    cancelar: { pt: "Cancelar", en: "Cancel" },
    salvarEdicao: { pt: "Salvar Edição", en: "Save Edit" },
    confirmarExclusao: { pt: "Deseja realmente excluir esta categoria?", en: "Do you really want to delete this category?" }
};

export default function CadastrarCategoriaFinanceira({ language = "pt" }) {
    const [nomeCategoria, setNomeCategoria] = useState("");
    const [tipoCategoria, setTipoCategoria] = useState("");
    const [categorias, setCategorias] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [mensagem, setMensagem] = useState(null);
    const [mensagemTipo, setMensagemTipo] = useState("success");
    const [ordem, setOrdem] = useState("todas");

    // Para edição
    const [editandoId, setEditandoId] = useState(null);
    const [editNomeCategoria, setEditNomeCategoria] = useState("");
    const [editTipoCategoria, setEditTipoCategoria] = useState("");

    // Para exclusão
    const [showDelete, setShowDelete] = useState(false);
    const [deleteId, setDeleteId] = useState(null);
    const [excluindoId, setExcluindoId] = useState(null);

    const accessToken = localStorage.getItem("accessToken");
    const navigate = useNavigate();

    useEffect(() => {
        buscarCategorias();
        // eslint-disable-next-line
    }, []);

    async function buscarCategorias() {
        setIsLoading(true);
        try {
            const response = await api.get("/v1/api/categoria", {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setCategorias(response.data);
            setMensagem(null);
        } catch (error) {
            setMensagem(textos.erroBuscar[language]);
            setMensagemTipo("danger");
        }
        setIsLoading(false);
    }

    async function handleSalvarCategoria(e) {
        e.preventDefault();
        if (!nomeCategoria.trim() || !tipoCategoria) {
            setMensagem(textos.erroCampos[language]);
            setMensagemTipo("danger");
            return;
        }
        setIsSaving(true);
        try {
            await api.post(
                "/v1/api/categoria",
                { nomeCategoria: nomeCategoria.trim(), tipoCategoria },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            setMensagem(textos.sucesso[language]);
            setMensagemTipo("success");
            setNomeCategoria("");
            setTipoCategoria("");
            buscarCategorias();
        } catch (error) {
            setMensagem(textos.erroCadastrar[language]);
            setMensagemTipo("danger");
        }
        setIsSaving(false);
    }

    function getCategoriasOrdenadas() {
        if (ordem === "todas") return categorias;
        return categorias.slice().sort((a, b) => {
            if (ordem === "receita") {
                if (a.tipoCategoria === "Receita" && b.tipoCategoria !== "Receita") return -1;
                if (a.tipoCategoria !== "Receita" && b.tipoCategoria === "Receita") return 1;
            }
            if (ordem === "despesa") {
                if (a.tipoCategoria === "Despesa" && b.tipoCategoria !== "Despesa") return -1;
                if (a.tipoCategoria !== "Despesa" && b.tipoCategoria === "Despesa") return 1;
            }
            return a.nomeCategoria.localeCompare(b.nomeCategoria);
        });
    }

    // Edição
    function iniciarEdicao(cat) {
        setEditandoId(cat.id);
        setEditNomeCategoria(cat.nomeCategoria);
        setEditTipoCategoria(cat.tipoCategoria);
        setMensagem(null);
    }
    function cancelarEdicao() {
        setEditandoId(null);
        setEditNomeCategoria("");
        setEditTipoCategoria("");
    }
    async function salvarEdicaoCategoria(id) {
        if (!editNomeCategoria.trim() || !editTipoCategoria) {
            setMensagem(textos.erroCampos[language]);
            setMensagemTipo("danger");
            return;
        }
        setIsSaving(true);
        try {
            await api.patch(
                "/v1/api/categoria",
                { id, nomeCategoria: editNomeCategoria.trim(), tipoCategoria: editTipoCategoria },
                { headers: { Authorization: `Bearer ${accessToken}` } }
            );
            setMensagem(textos.sucessoEditar[language]);
            setMensagemTipo("success");
            setEditandoId(null);
            setEditNomeCategoria("");
            setEditTipoCategoria("");
            buscarCategorias();
        } catch (error) {
            setMensagem(textos.erroEditar[language]);
            setMensagemTipo("danger");
        }
        setIsSaving(false);
    }

    // Exclusão
    function abrirDelete(id) {
        setDeleteId(id);
        setShowDelete(true);
    }
    function fecharDelete() {
        setDeleteId(null);
        setShowDelete(false);
    }
    async function excluirCategoriaConfirmada() {
        setExcluindoId(deleteId);
        try {
            await api.delete(`/v1/api/categoria/${deleteId}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setMensagem(textos.sucessoExcluir[language]);
            setMensagemTipo("success");
            buscarCategorias();
        } catch (error) {
            setMensagem(textos.erroExcluir[language]);
            setMensagemTipo("danger");
        }
        setExcluindoId(null);
        fecharDelete();
    }

    return (
        <div className="categoria-bg py-5">
            <div className="container categoria-container shadow-lg rounded-4 p-4">
                <button
                    className="btn btn-outline-secondary mb-3"
                    onClick={() => navigate("/financas")}
                >
                    {textos.voltar[language]}
                </button>
                <div className="row g-4 categoria-row">
                    {/* Seção 1: Formulário */}
                    <div className="col-12 col-lg-5">
                        <div className="card p-4 mb-4 categoria-form-card">
                            <h4 className="fw-bold mb-3 text-primary">{textos.titulo[language]}</h4>
                            <form onSubmit={handleSalvarCategoria}>
                                <div className="mb-3">
                                    <label className="form-label fw-semibold categoria-label">
                                        {textos.nomeCategoria[language]} <span className="text-danger">*</span>
                                    </label>
                                    <input
                                        type="text"
                                        className={`form-control ${!nomeCategoria && mensagem && mensagemTipo === "danger" ? "is-invalid" : ""}`}
                                        value={nomeCategoria}
                                        onChange={e => setNomeCategoria(e.target.value)}
                                        placeholder={textos.nomeExemplo[language]}
                                        maxLength={40}
                                        required
                                    />
                                </div>
                                <div className="mb-3">
                                    <label className="form-label fw-semibold categoria-label">
                                        {textos.tipoCategoria[language]} <span className="text-danger">*</span>
                                    </label>
                                    <div className="d-flex gap-3">
                                        <div className="form-check">
                                            <input
                                                className="form-check-input"
                                                type="radio"
                                                name="tipoCategoria"
                                                id="receita"
                                                value="Receita"
                                                checked={tipoCategoria === "Receita"}
                                                onChange={() => setTipoCategoria("Receita")}
                                                required
                                            />
                                            <label className="form-check-label categoria-label" htmlFor="receita">
                                                {textos.receita[language]}
                                            </label>
                                        </div>
                                        <div className="form-check">
                                            <input
                                                className="form-check-input"
                                                type="radio"
                                                name="tipoCategoria"
                                                id="despesa"
                                                value="Despesa"
                                                checked={tipoCategoria === "Despesa"}
                                                onChange={() => setTipoCategoria("Despesa")}
                                                required
                                            />
                                            <label className="form-check-label categoria-label" htmlFor="despesa">
                                                {textos.despesa[language]}
                                            </label>
                                        </div>
                                    </div>
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
                        <div className="card p-4 categoria-list-card h-100">
                            <div className="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                                <h4 className="fw-bold text-primary mb-0">{textos.listaTitulo[language]}</h4>
                                <div>
                                    <button
                                        className={`btn btn-outline-info btn-sm me-2 ${ordem === "receita" ? "active" : ""}`}
                                        onClick={() => setOrdem("receita")}
                                    >
                                        {textos.receitasPrimeiro[language]}
                                    </button>
                                    <button
                                        className={`btn btn-outline-warning btn-sm me-2 ${ordem === "despesa" ? "active" : ""}`}
                                        onClick={() => setOrdem("despesa")}
                                    >
                                        {textos.despesasPrimeiro[language]}
                                    </button>
                                    <button
                                        className={`btn btn-outline-secondary btn-sm ${ordem === "todas" ? "active" : ""}`}
                                        onClick={() => setOrdem("todas")}
                                    >
                                        {textos.ordemPadrao[language]}
                                    </button>
                                </div>
                            </div>
                            {isLoading ? (
                                <div className="text-center py-5">
                                    <div className="spinner-border text-info" role="status"></div>
                                </div>
                            ) : (
                                <div className="table-responsive">
                                    <table className="table table-bordered align-middle categoria-table">
                                        <thead>
                                            <tr>
                                                <th>{textos.nomeCategoria[language]}</th>
                                                <th>{textos.tipoCategoria[language]}</th>
                                                <th className="text-center">{textos.editar[language] + " / " + textos.excluir[language]}</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {getCategoriasOrdenadas().length === 0 ? (
                                                <tr>
                                                    <td colSpan={3} className="text-center text-secondary">
                                                        {textos.nenhumaCategoria[language]}
                                                    </td>
                                                </tr>
                                            ) : (
                                                getCategoriasOrdenadas().map((cat) => (
                                                    <tr key={cat.id}>
                                                        {editandoId === cat.id ? (
                                                            <>
                                                                <td>
                                                                    <input
                                                                        type="text"
                                                                        className="form-control form-control-sm"
                                                                        value={editNomeCategoria}
                                                                        onChange={e => setEditNomeCategoria(e.target.value)}
                                                                        maxLength={40}
                                                                    />
                                                                </td>
                                                                <td>
                                                                    <div className="d-flex gap-2">
                                                                        <div className="form-check">
                                                                            <input
                                                                                className="form-check-input"
                                                                                type="radio"
                                                                                name={`editTipoCategoria${cat.id}`}
                                                                                id={`edit-receita-${cat.id}`}
                                                                                value="Receita"
                                                                                checked={editTipoCategoria === "Receita"}
                                                                                onChange={() => setEditTipoCategoria("Receita")}
                                                                                required
                                                                            />
                                                                            <label className="form-check-label categoria-label" htmlFor={`edit-receita-${cat.id}`}>
                                                                                {textos.receita[language]}
                                                                            </label>
                                                                        </div>
                                                                        <div className="form-check">
                                                                            <input
                                                                                className="form-check-input"
                                                                                type="radio"
                                                                                name={`editTipoCategoria${cat.id}`}
                                                                                id={`edit-despesa-${cat.id}`}
                                                                                value="Despesa"
                                                                                checked={editTipoCategoria === "Despesa"}
                                                                                onChange={() => setEditTipoCategoria("Despesa")}
                                                                                required
                                                                            />
                                                                            <label className="form-check-label categoria-label" htmlFor={`edit-despesa-${cat.id}`}>
                                                                                {textos.despesa[language]}
                                                                            </label>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                                <td className="text-center">
                                                                    <button
                                                                        className="btn btn-success btn-sm me-2"
                                                                        onClick={() => salvarEdicaoCategoria(cat.id)}
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
                                                                <td>{cat.nomeCategoria}</td>
                                                                <td>
                                                                    <span className={`badge ${cat.tipoCategoria === "Receita" ? "bg-success" : "bg-danger"}`}>
                                                                        {cat.tipoCategoria === "Receita"
                                                                            ? textos.receita[language]
                                                                            : textos.despesa[language]}
                                                                    </span>
                                                                </td>
                                                                <td className="text-center">
                                                                    <button
                                                                        className="btn btn-warning btn-sm me-2"
                                                                        title={textos.editar[language]}
                                                                        onClick={() => iniciarEdicao(cat)}
                                                                        type="button"
                                                                    >
                                                                        <FiEdit2 />
                                                                    </button>
                                                                    <button
                                                                        className="btn btn-danger btn-sm"
                                                                        title={textos.excluir[language]}
                                                                        onClick={() => abrirDelete(cat.id)}
                                                                        disabled={excluindoId === cat.id}
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
                    <button className="btn btn-danger" onClick={excluirCategoriaConfirmada} disabled={excluindoId === deleteId}>
                        {textos.excluir[language]}
                    </button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}