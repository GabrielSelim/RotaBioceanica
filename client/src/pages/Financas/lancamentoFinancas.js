import React, { useEffect, useState } from "react";
import api from "../../services/api";
import { useNavigate } from "react-router-dom";
import { FiEdit2, FiTrash2, FiCheckCircle, FiPlus } from "react-icons/fi";
import { Modal, Button, Form } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "./lancamentoFinancas.css";
import dayjs from "dayjs";

const textos = {
    titulo: { pt: "Meus Lançamentos", en: "My Transactions" },
    resumoReceita: { pt: "Total de Receitas do Mês", en: "Total Income (Month)" },
    resumoDespesa: { pt: "Total de Despesas do Mês", en: "Total Expenses (Month)" },
    resumoSaldo: { pt: "Saldo do Mês", en: "Balance (Month)" },
    buscarDescricao: { pt: "Buscar por descrição...", en: "Search by description..." },
    tipo: { pt: "Tipo de Lançamento", en: "Transaction Type" },
    tipoTodos: { pt: "Todos", en: "All" },
    tipoReceita: { pt: "Receita", en: "Income" },
    tipoDespesa: { pt: "Despesa", en: "Expense" },
    situacao: { pt: "Situação", en: "Status" },
    situacaoTodos: { pt: "Todos", en: "All" },
    situacaoPago: { pt: "Pago", en: "Paid" },
    situacaoAPagar: { pt: "A Pagar", en: "To Pay" },
    situacaoRecebido: { pt: "Recebido", en: "Received" },
    situacaoAReceber: { pt: "A Receber", en: "To Receive" },
    categoria: { pt: "Categoria", en: "Category" },
    periodoDe: { pt: "De", en: "From" },
    periodoAte: { pt: "Até", en: "To" },
    aplicarFiltros: { pt: "Aplicar Filtros", en: "Apply Filters" },
    limparFiltros: { pt: "Limpar Filtros", en: "Clear Filters" },
    adicionar: { pt: "Adicionar Novo Lançamento", en: "Add New Transaction" },
    tabelaData: { pt: "Data", en: "Date" },
    tabelaDescricao: { pt: "Descrição", en: "Description" },
    tabelaValor: { pt: "Valor", en: "Amount" },
    tabelaTipo: { pt: "Tipo", en: "Type" },
    tabelaCategoria: { pt: "Categoria", en: "Category" },
    tabelaSituacao: { pt: "Situação", en: "Status" },
    // tabelaParcelamento: { pt: "Parcelamento Mensal", en: "Installment" }, // Removido
    tabelaAcoes: { pt: "Ações", en: "Actions" },
    editar: { pt: "Editar", en: "Edit" },
    excluir: { pt: "Excluir", en: "Delete" },
    marcarPago: { pt: "Marcar como Pago", en: "Mark as Paid" },
    confirmarExclusao: { pt: "Tem certeza que deseja excluir?", en: "Are you sure you want to delete?" },
    sim: { pt: "Sim", en: "Yes" },
    nao: { pt: "Não", en: "No" },
    salvar: { pt: "Salvar Lançamento", en: "Save Transaction" },
    cancelar: { pt: "Cancelar", en: "Cancel" },
    novo: { pt: "Adicionar Lançamento", en: "Add Transaction" },
    editarLanc: { pt: "Editar Lançamento", en: "Edit Transaction" },
    dataLancamento: { pt: "Data do Lançamento", en: "Transaction Date" },
    descricao: { pt: "Descrição", en: "Description" },
    valor: { pt: "Valor", en: "Amount" },
    pago: { pt: "Pago", en: "Paid" },
    aPagar: { pt: "A Pagar", en: "To Pay" },
    recebido: { pt: "Recebido", en: "Received" },
    aReceber: { pt: "A Receber", en: "To Receive" },
    ehParcelado: { pt: "É um lançamento parcelado?", en: "Is this an installment?" },
    selecione: { pt: "Selecione...", en: "Select..." },
    carregando: { pt: "Carregando...", en: "Loading..." },
    sucesso: { pt: "Operação realizada com sucesso!", en: "Operation successful!" },
    erro: { pt: "Erro ao processar operação.", en: "Error processing operation." },
    voltar: { pt: "← Voltar ao Painel Financeiro", en: "← Back to Finance Panel" },
    verMais: { pt: "Ver mais", en: "See more" },
    verMenos: { pt: "Ver menos", en: "See less" }
};

export default function LancamentoFinancas({ language = "pt" }) {
    const [descricaoFiltro, setDescricaoFiltro] = useState("");
    const [tipoFiltro, setTipoFiltro] = useState("");
    const [situacaoFiltro, setSituacaoFiltro] = useState("");
    const [categoriaFiltro, setCategoriaFiltro] = useState("");
    const [dataDe, setDataDe] = useState("");
    const [dataAte, setDataAte] = useState("");
    const [categorias, setCategorias] = useState([]);
    const [lancamentos, setLancamentos] = useState([]);
    const [resumo, setResumo] = useState({ receita: 0, despesa: 0, saldo: 0 });
    const [isLoading, setIsLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [editLancamento, setEditLancamento] = useState(null);
    const [showDelete, setShowDelete] = useState(false);
    const [deleteId, setDeleteId] = useState(null);
    const [showPago, setShowPago] = useState(false);
    const [pagarLancamento, setPagarLancamento] = useState(null);
    const [mensagem, setMensagem] = useState("");
    const [mensagemTipo, setMensagemTipo] = useState("success");
    const [modalNovoLancamento, setModalNovoLancamento] = useState(true);
    const [exibirLimite, setExibirLimite] = useState(10);

    const accessToken = localStorage.getItem("accessToken");
    const navigate = useNavigate();

    const categoriasFiltradas = tipoFiltro
        ? categorias.filter(cat => cat.tipoCategoria === tipoFiltro)
        : categorias;

    function getCategoriasPorTipo(tipo) {
        if (!tipo) return categorias;
        return categorias.filter(cat => cat.tipoCategoria === tipo);
    }

    function getMesAtualRange() {
        const inicio = dayjs().startOf("month").format("YYYY-MM-DD");
        const fim = dayjs().endOf("month").format("YYYY-MM-DD");
        return { inicio, fim };
    }

    useEffect(() => {
        carregarCategorias();
        const { inicio, fim } = getMesAtualRange();
        setDataDe(inicio);
        setDataAte(fim);
        carregarLancamentos({ dataDe: inicio, dataAte: fim });
        // eslint-disable-next-line
    }, []);

    async function carregarCategorias() {
        try {
            const res = await api.get("/v1/api/categoria", {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setCategorias(res.data);
        } catch {
            setCategorias([]);
        }
    }

    async function carregarLancamentos(filtros) {
        setIsLoading(true);
        try {
            let dataDeFiltro = filtros?.dataDe ?? dataDe;
            let dataAteFiltro = filtros?.dataAte ?? dataAte;
            if (!dataDeFiltro || !dataAteFiltro) {
                const { inicio, fim } = getMesAtualRange();
                dataDeFiltro = dataDeFiltro || inicio;
                dataAteFiltro = dataAteFiltro || fim;
            }
            const params = {
                dataInicio: dataDeFiltro,
                dataFim: dataAteFiltro
            };

            const res = await api.get("/v1/api/lancamento/periodo", {
                headers: { Authorization: `Bearer ${accessToken}` },
                params
            });

            let lista = Array.isArray(res.data) ? res.data : [];

            // Filtros do frontend
            const descFiltro = filtros?.descricao ?? descricaoFiltro;
            const tipoFiltroFront = filtros?.tipoLancamento ?? tipoFiltro;
            const situacaoFiltroFront = filtros?.situacao ?? situacaoFiltro;
            const categoriaFiltroFront = filtros?.categoriaId ?? categoriaFiltro;

            if (descFiltro && descFiltro.trim() !== "") {
                lista = lista.filter(l =>
                    l.descricao &&
                    l.descricao.toLowerCase().includes(descFiltro.trim().toLowerCase())
                );
            }
            if (tipoFiltroFront) {
                lista = lista.filter(l => l.tipoLancamento === tipoFiltroFront);
            }
            if (situacaoFiltroFront) {
                lista = lista.filter(l => l.situacao === situacaoFiltroFront);
            }
            if (categoriaFiltroFront) {
                lista = lista.filter(l => String(l.categoriaId) === String(categoriaFiltroFront));
            }

            setLancamentos(lista);

            let receita = 0, despesa = 0;
            lista.forEach(l => {
                if (l.tipoLancamento === "Receita") receita += Number(l.valor || 0);
                if (l.tipoLancamento === "Despesa") despesa += Number(l.valor || 0);
            });
            setResumo({
                receita,
                despesa,
                saldo: receita - despesa
            });
            setExibirLimite(10);
        } catch {
            setLancamentos([]);
            setResumo({ receita: 0, despesa: 0, saldo: 0 });
        }
        setIsLoading(false);
    }

    function limparFiltros() {
        setDescricaoFiltro("");
        setTipoFiltro("");
        setSituacaoFiltro("");
        setCategoriaFiltro("");
        setDataDe("");
        setDataAte("");
        carregarLancamentos({
            descricao: "",
            tipoLancamento: "",
            situacao: "",
            categoriaId: "",
            dataDe: "",
            dataAte: ""
        });
    }

    function aplicarFiltros(e) {
        e.preventDefault();
        carregarLancamentos({
            descricao: descricaoFiltro,
            tipoLancamento: tipoFiltro,
            situacao: situacaoFiltro,
            categoriaId: categoriaFiltro,
            dataDe,
            dataAte
        });
    }

    function abrirModal(lancamento = null) {
        setEditLancamento(lancamento);
        setModalNovoLancamento(!lancamento);
        setShowModal(true);
    }
    function fecharModal() {
        setEditLancamento(null);
        setShowModal(false);
    }
    function abrirDelete(id) {
        setDeleteId(id);
        setShowDelete(true);
    }
    function fecharDelete() {
        setDeleteId(null);
        setShowDelete(false);
    }
    function abrirPago(lancamento) {
        setPagarLancamento(lancamento);
        setShowPago(true);
    }
    function fecharPago() {
        setPagarLancamento(null);
        setShowPago(false);
    }

    async function salvarLancamento(e) {
        e.preventDefault();
        setMensagem("");
        setMensagemTipo("success");
        const form = e.target;
        const dados = {
            Id: editLancamento?.id,
            DataLancamento: form.dataLancamento.value,
            Descricao: form.descricao.value,
            Valor: parseFloat(form.valor.value),
            TipoLancamento: form.tipo.value,
            CategoriaId: parseInt(form.categoria.value, 10),
            ParcelamentoMensalId: null,
            Situacao: form.situacao.value
        };

        if (
            !dados.Descricao ||
            !dados.TipoLancamento ||
            !dados.Situacao ||
            !dados.DataLancamento ||
            isNaN(dados.Valor) ||
            isNaN(dados.CategoriaId)
        ) {
            setMensagem("Preencha todos os campos obrigatórios.");
            setMensagemTipo("danger");
            return;
        }

        try {
            if (editLancamento && editLancamento.id) {
                await api.patch(`/v1/api/lancamento`, dados, {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                        "Content-Type": "application/json"
                    }
                });
            } else {
                await api.post("/v1/api/lancamento", dados, {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                        "Content-Type": "application/json"
                    }
                });
            }
            setMensagem(textos.sucesso[language]);
            setMensagemTipo("success");
            fecharModal();
            carregarLancamentos();
        } catch (err) {
            setMensagem(textos.erro[language]);
            setMensagemTipo("danger");
        }
    }

    async function excluirLancamento() {
        try {
            await api.delete(`/v1/api/lancamento?id=${deleteId}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setMensagem(textos.sucesso[language]);
            setMensagemTipo("success");
            fecharDelete();
            carregarLancamentos();
        } catch {
            setMensagem(textos.erro[language]);
            setMensagemTipo("danger");
        }
    }

    function getSituacoesDisponiveis(tipo) {
        if (tipo === "Receita") return [
            { value: "Recebido", label: textos.situacaoRecebido[language] },
            { value: "A Receber", label: textos.situacaoAReceber[language] }
        ];
        if (tipo === "Despesa") return [
            { value: "Pago", label: textos.situacaoPago[language] },
            { value: "A Pagar", label: textos.situacaoAPagar[language] }
        ];
        return [
            { value: "Pago", label: textos.situacaoPago[language] },
            { value: "A Pagar", label: textos.situacaoAPagar[language] },
            { value: "Recebido", label: textos.situacaoRecebido[language] },
            { value: "A Receber", label: textos.situacaoAReceber[language] }
        ];
    }

    async function marcarComoPago(e) {
        e.preventDefault();
        const form = e.target;
        try {
            await api.put(`/v1/api/lancamento/${pagarLancamento.id}/pagar`, {
                Id: pagarLancamento.id,
                valorPago: parseFloat(form.valorPago.value),
                dataPagamento: form.dataPagamento.value
            }, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setMensagem(textos.sucesso[language]);
            setMensagemTipo("success");
            fecharPago();
            carregarLancamentos();
        } catch {
            setMensagem(textos.erro[language]);
            setMensagemTipo("danger");
        }
    }

    function verMais() {
        setExibirLimite((prev) => prev + 10);
    }
    function verMenos() {
        setExibirLimite(10);
        document.querySelector(".lancamento-table")?.scrollIntoView({ behavior: "smooth" });
    }

    if (isLoading) {
        return (
            <div className="lancamento-bg d-flex align-items-center justify-content-center" style={{ minHeight: "100vh" }}>
                <div className="spinner-border text-info" role="status" style={{ width: 60, height: 60 }}>
                    <span className="visually-hidden">{textos.carregando[language]}</span>
                </div>
            </div>
        );
    }

    return (
        <div className="lancamento-bg py-5">
            <div className="container lancamento-container shadow-lg rounded-4 p-4">
                <button
                    className="btn btn-outline-secondary mb-3"
                    onClick={() => navigate("/financas")}
                >
                    {textos.voltar[language]}
                </button>
                <h2 className="fw-bold text-primary mb-4">{textos.titulo[language]}</h2>
                {/* Dashboard Resumido */}
                <div className="row mb-4">
                    <div className="col-12 col-md-4 mb-2">
                        <div className="card text-white bg-success h-100">
                            <div className="card-body">
                                <h6 className="card-title">{textos.resumoReceita[language]}</h6>
                                <h4 className="card-text">{resumo.receita.toLocaleString(language === "pt" ? "pt-BR" : "en-US", { style: "currency", currency: "BRL" })}</h4>
                            </div>
                        </div>
                    </div>
                    <div className="col-12 col-md-4 mb-2">
                        <div className="card text-white bg-danger h-100">
                            <div className="card-body">
                                <h6 className="card-title">{textos.resumoDespesa[language]}</h6>
                                <h4 className="card-text">{resumo.despesa.toLocaleString(language === "pt" ? "pt-BR" : "en-US", { style: "currency", currency: "BRL" })}</h4>
                            </div>
                        </div>
                    </div>
                    <div className="col-12 col-md-4 mb-2">
                        <div className="card text-white bg-dark h-100">
                            <div className="card-body">
                                <h6 className="card-title">{textos.resumoSaldo[language]}</h6>
                                <h4 className="card-text">{resumo.saldo.toLocaleString(language === "pt" ? "pt-BR" : "en-US", { style: "currency", currency: "BRL" })}</h4>
                            </div>
                        </div>
                    </div>
                </div>
                {/* Barra de Filtros */}
                <Form className="mb-4" onSubmit={aplicarFiltros}>
                    <div className="lancamento-filtros-row">
                        <div className="lancamento-filtro-col">
                            <Form.Label>{textos.buscarDescricao[language]}</Form.Label>
                            <Form.Control
                                type="text"
                                value={descricaoFiltro}
                                onChange={e => setDescricaoFiltro(e.target.value)}
                                placeholder={textos.buscarDescricao[language]}
                            />
                        </div>
                        <div className="lancamento-filtro-col">
                            <Form.Label>{textos.tipo[language]}</Form.Label>
                            <Form.Select value={tipoFiltro} onChange={e => setTipoFiltro(e.target.value)}>
                                <option value="">{textos.tipoTodos[language]}</option>
                                <option value="Receita">{textos.tipoReceita[language]}</option>
                                <option value="Despesa">{textos.tipoDespesa[language]}</option>
                            </Form.Select>
                        </div>
                        <div className="lancamento-filtro-col">
                            <Form.Label>{textos.situacao[language]}</Form.Label>
                            <Form.Select value={situacaoFiltro} onChange={e => setSituacaoFiltro(e.target.value)}>
                                <option value="">{textos.situacaoTodos[language]}</option>
                                {getSituacoesDisponiveis(tipoFiltro).map(opt => (
                                    <option key={opt.value} value={opt.value}>{opt.label}</option>
                                ))}
                            </Form.Select>
                        </div>
                        <div className="lancamento-filtro-col">
                            <Form.Label>{textos.categoria[language]}</Form.Label>
                            <Form.Select value={categoriaFiltro} onChange={e => setCategoriaFiltro(e.target.value)}>
                                <option value="">{textos.selecione[language]}</option>
                                {categoriasFiltradas.map(cat => (
                                    <option key={cat.id} value={cat.id}>{cat.nomeCategoria}</option>
                                ))}
                            </Form.Select>
                        </div>
                        <div className="lancamento-filtro-col lancamento-filtro-col-date">
                            <Form.Label>{textos.periodoDe[language]}</Form.Label>
                            <Form.Control type="date" value={dataDe} onChange={e => setDataDe(e.target.value)} />
                        </div>
                        <div className="lancamento-filtro-col lancamento-filtro-col-date">
                            <Form.Label>{textos.periodoAte[language]}</Form.Label>
                            <Form.Control type="date" value={dataAte} onChange={e => setDataAte(e.target.value)} />
                        </div>
                        <div className="lancamento-filtro-col-btns">
                            <Button variant="primary" type="submit" className="w-100 mb-2">
                                {textos.aplicarFiltros[language]}
                            </Button>
                            <Button variant="secondary" type="button" className="w-100" onClick={limparFiltros}>
                                {textos.limparFiltros[language]}
                            </Button>
                        </div>
                    </div>
                </Form>
                {/* Botão Adicionar */}
                <div className="mb-3 text-end">
                    <Button variant="success" onClick={() => abrirModal()}><FiPlus /> {textos.adicionar[language]}</Button>
                </div>
                {/* Mensagem */}
                {mensagem && (
                    <div className={`alert alert-${mensagemTipo} py-2 text-center`} role="alert">
                        {mensagem}
                    </div>
                )}
                {/* Tabela */}
                <div className="table-responsive" style={{ overflowX: "auto" }}>
                    <table className="table table-bordered align-middle lancamento-table">
                        <thead>
                            <tr>
                                <th>{textos.tabelaData[language]}</th>
                                <th>{textos.tabelaDescricao[language]}</th>
                                <th>{textos.tabelaValor[language]}</th>
                                <th>{textos.tabelaTipo[language]}</th>
                                <th>{textos.tabelaCategoria[language]}</th>
                                <th>{textos.tabelaSituacao[language]}</th>
                                {/* <th>{textos.tabelaParcelamento[language]}</th> Removido */}
                                <th>{textos.tabelaAcoes[language]}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {lancamentos.length === 0 ? (
                                <tr>
                                    <td colSpan={7} className="text-center text-secondary">
                                        {language === "pt" ? "Nenhum lançamento encontrado." : "No transactions found."}
                                    </td>
                                </tr>
                            ) : (
                                lancamentos.slice(0, exibirLimite).map(lanc => (
                                    <tr key={lanc.id}>
                                        <td>{new Date(lanc.dataLancamento).toLocaleDateString(language === "pt" ? "pt-BR" : "en-US")}</td>
                                        <td>{lanc.descricao}</td>
                                        <td style={{ color: lanc.tipoLancamento === "Receita" ? "#198754" : "#dc3545", fontWeight: "bold" }}>
                                            {parseFloat(lanc.valor).toLocaleString(language === "pt" ? "pt-BR" : "en-US", { style: "currency", currency: "BRL" })}
                                        </td>
                                        <td>
                                            <span className={`badge ${lanc.tipoLancamento === "Receita" ? "bg-success" : "bg-danger"}`}>
                                                {lanc.tipoLancamento === "Receita" ? textos.tipoReceita[language] : textos.tipoDespesa[language]}
                                            </span>
                                        </td>
                                        <td>
                                            {categorias.find(cat => String(cat.id) === String(lanc.categoriaId))?.nomeCategoria || "-"}
                                        </td>
                                        <td>
                                            <span className={`badge ${
                                                lanc.situacao === "Pago" || lanc.situacao === "Recebido"
                                                    ? "bg-success"
                                                    : lanc.situacao === "A Receber"
                                                    ? "bg-info text-dark"
                                                    : "bg-warning text-dark"
                                            }`}>
                                                {lanc.situacao === "Pago"
                                                    ? textos.situacaoPago[language]
                                                    : lanc.situacao === "Recebido"
                                                    ? textos.situacaoRecebido[language]
                                                    : lanc.situacao === "A Receber"
                                                    ? textos.situacaoAReceber[language]
                                                    : textos.situacaoAPagar[language]}
                                            </span>
                                        </td>
                                        {/* Removido: <td>{lanc.parcelamentoMensalId ? `Parc. ${lanc.parcelamentoMensalId}` : "-"}</td> */}
                                        <td>
                                            {/* Só mostra editar se NÃO for parcelamento mensal */}
                                            {!lanc.parcelamentoMensalId && (
                                                <Button variant="outline-info" size="sm" className="me-1" title={textos.editar[language]} onClick={() => abrirModal(lanc)}>
                                                    <FiEdit2 />
                                                </Button>
                                            )}
                                            <Button variant="outline-danger" size="sm" className="me-1" title={textos.excluir[language]} onClick={() => abrirDelete(lanc.id)}>
                                                <FiTrash2 />
                                            </Button>
                                            {lanc.situacao !== "Pago" && lanc.situacao !== "Recebido" && lanc.situacao !== "A Receber" && (
                                                <Button variant="outline-success" size="sm" title={textos.marcarPago[language]} onClick={() => abrirPago(lanc)}>
                                                    <FiCheckCircle />
                                                </Button>
                                            )}
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
                {/* Botão ver mais/ver menos */}
                {lancamentos.length > 10 && (
                    <div className="d-flex justify-content-center mt-3">
                        {exibirLimite < lancamentos.length ? (
                            <button className="btn btn-gradient" onClick={verMais}>
                                {textos.verMais[language]}
                            </button>
                        ) : (
                            <button className="btn btn-outline-light" onClick={verMenos}>
                                {textos.verMenos[language]}
                            </button>
                        )}
                    </div>
                )}
            </div>

            {/* Modal de Criação/Edição */}
            <Modal show={showModal} onHide={fecharModal} centered>
                <Form onSubmit={salvarLancamento}>
                    <Modal.Header closeButton>
                        <Modal.Title>
                            {modalNovoLancamento ? textos.novo[language] : textos.editarLanc[language]}
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.dataLancamento[language]}</Form.Label>
                            <Form.Control
                                type="datetime-local"
                                name="dataLancamento"
                                defaultValue={editLancamento ? editLancamento.dataLancamento?.slice(0, 16) : ""}
                                required
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.descricao[language]}</Form.Label>
                            <Form.Control
                                type="text"
                                name="descricao"
                                defaultValue={editLancamento?.descricao || ""}
                                required
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.valor[language]}</Form.Label>
                            <Form.Control
                                type="number"
                                name="valor"
                                step="0.01"
                                min="0"
                                defaultValue={editLancamento?.valor || ""}
                                required
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.tipo[language]}</Form.Label>
                            <div>
                                <Form.Check
                                    inline
                                    label={textos.tipoReceita[language]}
                                    name="tipo"
                                    type="radio"
                                    value="Receita"
                                    defaultChecked={editLancamento?.tipoLancamento === "Receita"}
                                    required
                                    onChange={() => setEditLancamento(prev => ({ ...prev, tipoLancamento: "Receita" }))}
                                />
                                <Form.Check
                                    inline
                                    label={textos.tipoDespesa[language]}
                                    name="tipo"
                                    type="radio"
                                    value="Despesa"
                                    defaultChecked={editLancamento?.tipoLancamento === "Despesa"}
                                    required
                                    onChange={() => setEditLancamento(prev => ({ ...prev, tipoLancamento: "Despesa" }))}
                                />
                            </div>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <div className="d-flex align-items-center">
                                <Form.Label className="mb-0 me-2">{textos.categoria[language]}</Form.Label>
                                <Button
                                    variant="outline-primary"
                                    size="sm"
                                    onClick={() => {
                                        fecharModal();
                                        navigate("/financas/cadastrar-categoria");
                                    }}
                                    title="Cadastrar nova categoria"
                                    style={{ whiteSpace: "nowrap" }}
                                >
                                    + {textos.categoria[language]}
                                </Button>
                            </div>
                            <Form.Select name="categoria" defaultValue={editLancamento?.categoriaId || ""} required>
                                <option value="">{textos.selecione[language]}</option>
                                {getCategoriasPorTipo(editLancamento?.tipoLancamento).map(cat => (
                                    <option key={cat.id} value={cat.id}>{cat.nomeCategoria}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.situacao[language]}</Form.Label>
                            <div>
                                {getSituacoesDisponiveis(editLancamento?.tipoLancamento).map(opt => (
                                    <Form.Check
                                        key={opt.value}
                                        inline
                                        label={opt.label}
                                        name="situacao"
                                        type="radio"
                                        value={opt.value}
                                        defaultChecked={editLancamento?.situacao === opt.value}
                                        required
                                    />
                                ))}
                            </div>
                        </Form.Group>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={fecharModal}>
                            {textos.cancelar[language]}
                        </Button>
                        <Button variant="primary" type="submit">
                            {textos.salvar[language]}
                        </Button>
                    </Modal.Footer>
                </Form>
            </Modal>

            {/* Modal de Exclusão */}
            <Modal show={showDelete} onHide={fecharDelete} centered>
                <Modal.Header closeButton>
                    <Modal.Title>{textos.excluir[language]}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>{textos.confirmarExclusao[language]}</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={fecharDelete}>{textos.nao[language]}</Button>
                    <Button variant="danger" onClick={excluirLancamento}>{textos.sim[language]}</Button>
                </Modal.Footer>
            </Modal>

            {/* Modal de Marcar como Pago */}
            <Modal show={showPago} onHide={fecharPago} centered>
                <Form onSubmit={marcarComoPago}>
                    <Modal.Header closeButton>
                        <Modal.Title>{textos.marcarPago[language]}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.valor[language]}</Form.Label>
                            <Form.Control
                                type="number"
                                name="valorPago"
                                step="0.01"
                                min="0"
                                defaultValue={pagarLancamento?.valor || ""}
                                required
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>{textos.dataLancamento[language]}</Form.Label>
                            <Form.Control
                                type="datetime-local"
                                name="dataPagamento"
                                required
                            />
                        </Form.Group>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={fecharPago}>
                            {textos.cancelar[language]}
                        </Button>
                        <Button variant="success" type="submit">
                            <FiCheckCircle /> {textos.marcarPago[language]}
                        </Button>
                    </Modal.Footer>
                </Form>
            </Modal>
        </div>
    );
}