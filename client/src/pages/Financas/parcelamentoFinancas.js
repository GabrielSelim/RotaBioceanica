import React, { useEffect, useState } from "react";
import { Button, Modal, Form, Table, Spinner, Alert } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import "./parcelamentoFinancas.css";
import api from "../../services/api";

const textos = {
    titulo: { pt: "Meus Parcelamentos", en: "My Installments" },
    descricao: { pt: "Descrição", en: "Description" },
    valorTotal: { pt: "Valor Total", en: "Total Value" },
    numeroParcelas: { pt: "Nº Parcelas", en: "Installments" },
    dataInicio: { pt: "Data Início", en: "Start Date" },
    situacao: { pt: "Situação", en: "Status" },
    situacaoTodos: { pt: "Todos", en: "All" },
    situacaoAtivo: { pt: "Ativo", en: "Active" },
    situacaoConcluido: { pt: "Concluído", en: "Completed" },
    situacaoCancelado: { pt: "Cancelado", en: "Canceled" },
    situacaoPendente: { pt: "Pendente", en: "Pending" },
    situacaoSuspenso: { pt: "Suspenso", en: "Suspended" },
    pessoaConta: { pt: "Pessoa/Conta", en: "Person/Account" },
    cartao: { pt: "Cartão", en: "Card" },
    proximaParcela: { pt: "Próxima Parcela", en: "Next Installment" },
    parcelasPagas: { pt: "Parcelas Pagas", en: "Paid Installments" },
    acoes: { pt: "Ações", en: "Actions" },
    adicionar: { pt: "Adicionar Novo Parcelamento", en: "Add New Installment" },
    editar: { pt: "Editar", en: "Edit" },
    excluir: { pt: "Excluir", en: "Delete" },
    verParcelas: { pt: "Ver Parcelas", en: "View Installments" },
    aplicarFiltros: { pt: "Aplicar Filtros", en: "Apply Filters" },
    limparFiltros: { pt: "Limpar Filtros", en: "Clear Filters" },
    salvar: { pt: "Salvar Parcelamento", en: "Save Installment" },
    cancelar: { pt: "Cancelar", en: "Cancel" },
    selecione: { pt: "Selecione...", en: "Select..." },
    confirmacaoExclusao: { pt: "Deseja realmente excluir este parcelamento?", en: "Do you really want to delete this installment?" },
    sim: { pt: "Sim", en: "Yes" },
    nao: { pt: "Não", en: "No" },
    sucesso: { pt: "Operação realizada com sucesso!", en: "Operation successful!" },
    erro: { pt: "Erro ao processar operação.", en: "Error processing operation." },
    carregando: { pt: "Carregando...", en: "Loading..." },
    intervalo: { pt: "Intervalo entre Parcelas", en: "Installment Interval" },
    mensal: { pt: "Mensal", en: "Monthly" },
    quinzenal: { pt: "Quinzenal", en: "Biweekly" },
    semanal: { pt: "Semanal", en: "Weekly" },
    voltar: { pt: "← Voltar ao Painel Financeiro", en: "← Back to Finance Panel" },
    pessoaContaCriar: { pt: "Cadastrar Pessoa/Conta", en: "Add Person/Account" },
    cartaoCriar: { pt: "Cadastrar Cartão", en: "Add Card" },
    verMais: { pt: "Ver mais", en: "See more" },
    verMenos: { pt: "Ver menos", en: "See less" }
};

const situacoesDisponiveis = [
    { value: "Ativo", label: { pt: textos.situacaoAtivo.pt, en: textos.situacaoAtivo.en } },
    { value: "Concluido", label: { pt: textos.situacaoConcluido.pt, en: textos.situacaoConcluido.en } },
    { value: "Cancelado", label: { pt: textos.situacaoCancelado.pt, en: textos.situacaoCancelado.en } },
    { value: "Pendente", label: { pt: textos.situacaoPendente.pt, en: textos.situacaoPendente.en } },
    { value: "Suspenso", label: { pt: textos.situacaoSuspenso.pt, en: textos.situacaoSuspenso.en } }
];

export default function ParcelamentoFinancas({ language = "pt" }) {
    const [parcelamentos, setParcelamentos] = useState([]);
    const [filtros, setFiltros] = useState({
        descricao: "",
        pessoaContaId: "",
        cartaoId: "",
        situacao: ""
    });
    const [pessoasContas, setPessoasContas] = useState([]);
    const [cartoes, setCartoes] = useState([]);
    const [pessoaNomes, setPessoaNomes] = useState({});
    const [cartaoNomes, setCartaoNomes] = useState({});
    const [loading, setLoading] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [editParcelamento, setEditParcelamento] = useState(null);
    const [mensagem, setMensagem] = useState("");
    const [mensagemTipo, setMensagemTipo] = useState("success");
    const [salvando, setSalvando] = useState(false);
    const [showDelete, setShowDelete] = useState(false);
    const [deleteId, setDeleteId] = useState(null);
    const [exibirLimite, setExibirLimite] = useState(10);
    const [todosParcelamentos, setTodosParcelamentos] = useState([]);
    const navigate = useNavigate();

    // Estado controlado para o formulário do modal
    const [formModal, setFormModal] = useState({
        descricao: "",
        valorTotal: "",
        numeroParcelas: "",
        dataPrimeiraParcela: "",
        intervalo: "30",
        pessoaContaId: "",
        cartaoId: "",
        situacao: "Ativo"
    });

    // Carregar pessoas/contas e cartões
    useEffect(() => {
        async function fetchFiltros() {
            try {
                const accessToken = localStorage.getItem("accessToken");
                // Pessoa/Conta
                const pessoasRes = await api.get("/v1/api/pessoaconta", {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                setPessoasContas(Array.isArray(pessoasRes.data) ? pessoasRes.data : []);
                // Cartão
                const cartoesRes = await api.get("/v1/api/cartao", {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                setCartoes(Array.isArray(cartoesRes.data) ? cartoesRes.data : []);
            } catch {}
        }
        fetchFiltros();
    }, []);

    // Carregar parcelamentos e buscar nomes de pessoa/cartão
    async function carregarParcelamentos() {
        setLoading(true);
        try {
            const accessToken = localStorage.getItem("accessToken");
            const res = await api.get("/v1/api/parcelamento", {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            const lista = Array.isArray(res.data) ? res.data : [];
            setParcelamentos(lista);
            setTodosParcelamentos(lista);

            // Buscar nomes de pessoa e cartão para cada parcelamento
            const pessoaIds = [...new Set(lista.map(p => p.pessoaContaId).filter(Boolean))];
            const cartaoIds = [...new Set(lista.map(p => p.cartaoId).filter(Boolean))];
            const pessoaNomesTemp = {};
            const cartaoNomesTemp = {};

            await Promise.all([
                ...pessoaIds.map(async id => {
                    try {
                        const res = await api.get(`/v1/api/pessoaconta/${id}`, {
                            headers: { Authorization: `Bearer ${accessToken}` }
                        });
                        pessoaNomesTemp[id] = res.data?.nomePessoa || "-";
                    } catch {
                        pessoaNomesTemp[id] = "-";
                    }
                }),
                ...cartaoIds.map(async id => {
                    try {
                        const res = await api.get(`/v1/api/cartao/${id}`, {
                            headers: { Authorization: `Bearer ${accessToken}` }
                        });
                        cartaoNomesTemp[id] = res.data?.nomeUsuario && res.data?.nomeBanco
                            ? `${res.data.nomeUsuario} - ${res.data.nomeBanco}`
                            : "-";
                    } catch {
                        cartaoNomesTemp[id] = "-";
                    }
                })
            ]);
            setPessoaNomes(pessoaNomesTemp);
            setCartaoNomes(cartaoNomesTemp);
        } catch {
            setParcelamentos([]);
            setTodosParcelamentos([]);
        }
        setLoading(false);
    }

    useEffect(() => {
        carregarParcelamentos();
        // eslint-disable-next-line
    }, []);

    // Filtro front-end
    function aplicarFiltros(e) {
        if (e) e.preventDefault();
        let filtrados = [...todosParcelamentos];
        if (filtros.descricao.trim()) {
            filtrados = filtrados.filter(p =>
                p.descricao &&
                p.descricao.toLowerCase().includes(filtros.descricao.trim().toLowerCase())
            );
        }
        if (filtros.pessoaContaId) {
            filtrados = filtrados.filter(p => String(p.pessoaContaId) === String(filtros.pessoaContaId));
        }
        if (filtros.cartaoId) {
            filtrados = filtrados.filter(p => String(p.cartaoId) === String(filtros.cartaoId));
        }
        if (filtros.situacao) {
            filtrados = filtrados.filter(p => p.situacao === filtros.situacao);
        }
        setParcelamentos(filtrados);
        setExibirLimite(10);
    }

    function limparFiltros() {
        setFiltros({ descricao: "", pessoaContaId: "", cartaoId: "", situacao: "" });
        setParcelamentos(todosParcelamentos);
        setExibirLimite(10);
    }

    function abrirModal(parcelamento = null) {
        setEditParcelamento(parcelamento);
        setShowModal(true);
        setFormModal({
            descricao: parcelamento?.descricao || "",
            valorTotal: parcelamento?.valorTotal || "",
            numeroParcelas: parcelamento?.numeroParcelas || "",
            dataPrimeiraParcela: parcelamento?.dataPrimeiraParcela ? parcelamento.dataPrimeiraParcela.slice(0, 16) : "",
            intervalo: parcelamento?.intervaloParcelas?.toString() || "30",
            pessoaContaId: parcelamento?.pessoaContaId || "",
            cartaoId: parcelamento?.cartaoId || "",
            situacao: parcelamento?.situacao || "Ativo"
        });
    }

    function fecharModal() {
        setEditParcelamento(null);
        setShowModal(false);
        setFormModal({
            descricao: "",
            valorTotal: "",
            numeroParcelas: "",
            dataPrimeiraParcela: "",
            intervalo: "30",
            pessoaContaId: "",
            cartaoId: "",
            situacao: "Ativo"
        });
    }

    function abrirDelete(id) {
        setDeleteId(id);
        setShowDelete(true);
    }
    function fecharDelete() {
        setDeleteId(null);
        setShowDelete(false);
    }

    function verParcelasMensais(parcelamentoId) {
        navigate(`/financas/parcelamento-mensais/${parcelamentoId}`);
    }

    async function salvarParcelamento(e) {
        e.preventDefault();
        setMensagem("");
        setMensagemTipo("success");
        setSalvando(true);

        const dados = {
            id: editParcelamento?.id,
            descricao: formModal.descricao,
            valorTotal: parseFloat(formModal.valorTotal),
            numeroParcelas: parseInt(formModal.numeroParcelas, 10),
            dataPrimeiraParcela: formModal.dataPrimeiraParcela,
            intervaloParcelas: parseInt(formModal.intervalo, 10),
            pessoaContaId: parseInt(formModal.pessoaContaId, 10),
            cartaoId: formModal.cartaoId ? parseInt(formModal.cartaoId, 10) : null,
            situacao: formModal.situacao
        };

        if (
            !dados.descricao ||
            isNaN(dados.valorTotal) ||
            isNaN(dados.numeroParcelas) ||
            !dados.dataPrimeiraParcela ||
            isNaN(dados.intervaloParcelas) ||
            isNaN(dados.pessoaContaId) ||
            !dados.situacao
        ) {
            setMensagem(language === "pt" ? "Preencha todos os campos obrigatórios." : "Fill in all required fields.");
            setMensagemTipo("danger");
            setSalvando(false);
            return;
        }

        try {
            const accessToken = localStorage.getItem("accessToken");
            if (editParcelamento && editParcelamento.id) {
                await api.put(`/v1/api/parcelamento`, dados, {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                setMensagem(language === "pt" ? "Parcelamento atualizado com sucesso!" : "Installment updated successfully!");
            } else {
                await api.post(`/v1/api/parcelamento`, dados, {
                    headers: { Authorization: `Bearer ${accessToken}` }
                });
                setMensagem(language === "pt" ? "Parcelamento criado com sucesso!" : "Installment created successfully!");
            }
            setMensagemTipo("success");
            fecharModal();
            carregarParcelamentos();
        } catch {
            setMensagem(textos.erro[language]);
            setMensagemTipo("danger");
        }
        setSalvando(false);
    }

    async function excluirParcelamento() {
        if (!deleteId) return;
        try {
            const accessToken = localStorage.getItem("accessToken");
            await api.delete(`/v1/api/parcelamento/${deleteId}`, {
                headers: { Authorization: `Bearer ${accessToken}` }
            });
            setMensagem(language === "pt" ? "Parcelamento excluído com sucesso!" : "Installment deleted successfully!");
            setMensagemTipo("success");
            fecharDelete();
            carregarParcelamentos();
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
        document.querySelector(".parcelamento-table")?.scrollIntoView({ behavior: "smooth" });
    }

    return (
        <div className="parcelamento-bg py-5">
            <div className="container parcelamento-container shadow-lg rounded-4 p-4">
                <button
                    className="btn btn-outline-secondary mb-3"
                    onClick={() => navigate("/financas")}
                >
                    {textos.voltar[language]}
                </button>
                <h2 className="fw-bold text-primary mb-4">{textos.titulo[language]}</h2>
                <Form className="mb-3" onSubmit={aplicarFiltros}>
                    <div className="row g-2 align-items-end parcelamento-filtros-row">
                        <div className="col-md-3">
                            <Form.Label className="form-label">{textos.descricao[language]}</Form.Label>
                            <Form.Control
                                placeholder={textos.descricao[language]}
                                value={filtros.descricao}
                                onChange={e => setFiltros(f => ({ ...f, descricao: e.target.value }))}
                            />
                        </div>
                        <div className="col-md-2">
                            <Form.Label className="form-label">{textos.pessoaConta[language]}</Form.Label>
                            <div className="d-flex align-items-center">
                                <Form.Select
                                    value={filtros.pessoaContaId}
                                    onChange={e => setFiltros(f => ({ ...f, pessoaContaId: e.target.value }))}
                                >
                                    <option value="">{textos.selecione[language]}</option>
                                    {pessoasContas.map(pc => (
                                        <option key={pc.id} value={pc.id}>{pc.nomePessoa}</option>
                                    ))}
                                </Form.Select>
                                <Button
                                    variant="outline-primary"
                                    size="sm"
                                    className="ms-2"
                                    onClick={() => navigate("/financas/pessoa-conta")}
                                    title={textos.pessoaContaCriar[language]}
                                    style={{ whiteSpace: "nowrap" }}
                                >
                                    +
                                </Button>
                            </div>
                        </div>
                        <div className="col-md-2">
                            <Form.Label className="form-label">{textos.cartao[language]}</Form.Label>
                            <div className="d-flex align-items-center">
                                <Form.Select
                                    value={filtros.cartaoId}
                                    onChange={e => setFiltros(f => ({ ...f, cartaoId: e.target.value }))}
                                >
                                    <option value="">{textos.selecione[language]}</option>
                                    {cartoes.map(c => (
                                        <option key={c.id} value={c.id}>
                                            {c.nomeUsuario} - {c.nomeBanco}
                                        </option>
                                    ))}
                                </Form.Select>
                                <Button
                                    variant="outline-primary"
                                    size="sm"
                                    className="ms-2"
                                    onClick={() => navigate("/financas/cadastrar-cartao")}
                                    title={textos.cartaoCriar[language]}
                                    style={{ whiteSpace: "nowrap" }}
                                >
                                    +
                                </Button>
                            </div>
                        </div>
                        <div className="col-md-2">
                            <Form.Label className="form-label">{textos.situacao[language]}</Form.Label>
                            <Form.Select
                                value={filtros.situacao}
                                onChange={e => setFiltros(f => ({ ...f, situacao: e.target.value }))}
                            >
                                <option value="">{textos.situacaoTodos[language]}</option>
                                {situacoesDisponiveis.map(opt => (
                                    <option key={opt.value} value={opt.value}>{opt.label[language]}</option>
                                ))}
                            </Form.Select>
                        </div>
                        <div className="col-md-3 d-flex gap-2">
                            <Button type="submit" variant="primary" className="w-100">{textos.aplicarFiltros[language]}</Button>
                            <Button type="button" variant="secondary" className="w-100" onClick={limparFiltros}>{textos.limparFiltros[language]}</Button>
                        </div>
                    </div>
                </Form>
                <div className="mb-3 text-end">
                    <Button variant="success" onClick={() => abrirModal()}>{textos.adicionar[language]}</Button>
                </div>
                {mensagem && <Alert variant={mensagemTipo}>{mensagem}</Alert>}
                {loading ? (
                    <div className="text-center my-5"><Spinner animation="border" /></div>
                ) : (
                    <div className="table-responsive">
                        <Table bordered hover className="parcelamento-table align-middle">
                            <thead>
                                <tr>
                                    <th>{textos.descricao[language]}</th>
                                    <th>{textos.valorTotal[language]}</th>
                                    <th>{textos.numeroParcelas[language]}</th>
                                    <th>{textos.dataInicio[language]}</th>
                                    <th>{textos.situacao[language]}</th>
                                    <th>{textos.pessoaConta[language]}</th>
                                    <th>{textos.cartao[language]}</th>
                                    <th>{textos.acoes[language]}</th>
                                </tr>
                            </thead>
                            <tbody>
                                {parcelamentos.length === 0 ? (
                                    <tr>
                                        <td colSpan={8} className="text-center text-secondary">
                                            {language === "pt" ? "Nenhum parcelamento encontrado." : "No installments found."}
                                        </td>
                                    </tr>
                                ) : (
                                    parcelamentos.slice(0, exibirLimite).map(p => (
                                        <tr key={p.id}>
                                            <td>{p.descricao}</td>
                                            <td>R$ {Number(p.valorTotal).toLocaleString("pt-BR", { minimumFractionDigits: 2 })}</td>
                                            <td>{p.numeroParcelas}</td>
                                            <td>{new Date(p.dataPrimeiraParcela).toLocaleDateString(language === "pt" ? "pt-BR" : "en-US")}</td>
                                            <td>
                                                <span className={p.situacao === "Ativo" ? "parcelamento-status-ativo" : "parcelamento-status-concluido"}>
                                                    {situacoesDisponiveis.find(s => s.value === p.situacao)?.label[language] || p.situacao}
                                                </span>
                                            </td>
                                            <td>{pessoaNomes[p.pessoaContaId] || "-"}</td>
                                            <td>{p.cartaoId ? (cartaoNomes[p.cartaoId] || "-") : "-"}</td>
                                            <td>
                                                <Button size="sm" variant="info" className="me-1" onClick={() => verParcelasMensais(p.id)}>{textos.verParcelas[language]}</Button>
                                                <Button size="sm" variant="warning" className="me-1" onClick={() => abrirModal(p)}>{textos.editar[language]}</Button>
                                                <Button size="sm" variant="danger" onClick={() => abrirDelete(p.id)}>{textos.excluir[language]}</Button>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </Table>
                    </div>
                )}
                {/* Botão ver mais/ver menos */}
                {parcelamentos.length > 10 && (
                    <div className="d-flex justify-content-center mt-3">
                        {exibirLimite < parcelamentos.length ? (
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
                {/* Modal de criação/edição */}
                <Modal show={showModal} onHide={fecharModal} centered>
                    <Form onSubmit={salvarParcelamento}>
                        <Modal.Header closeButton>
                            <Modal.Title>
                                {editParcelamento ? textos.editar[language] : textos.adicionar[language]}
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <Form.Group className="mb-3">
                                <Form.Label>{textos.descricao[language]} *</Form.Label>
                                <Form.Control
                                    name="descricao"
                                    value={formModal.descricao}
                                    onChange={e => setFormModal(f => ({ ...f, descricao: e.target.value }))}
                                    required
                                />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>{textos.valorTotal[language]} *</Form.Label>
                                <Form.Control
                                    name="valorTotal"
                                    type="number"
                                    step="0.01"
                                    min="0"
                                    value={formModal.valorTotal}
                                    onChange={e => setFormModal(f => ({ ...f, valorTotal: e.target.value }))}
                                    required
                                />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>{textos.numeroParcelas[language]} *</Form.Label>
                                <Form.Control
                                    name="numeroParcelas"
                                    type="number"
                                    min="1"
                                    value={formModal.numeroParcelas}
                                    onChange={e => setFormModal(f => ({ ...f, numeroParcelas: e.target.value }))}
                                    required
                                />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>{textos.dataInicio[language]} *</Form.Label>
                                <Form.Control
                                    name="dataPrimeiraParcela"
                                    type="datetime-local"
                                    value={formModal.dataPrimeiraParcela}
                                    onChange={e => setFormModal(f => ({ ...f, dataPrimeiraParcela: e.target.value }))}
                                    required
                                />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>{textos.intervalo[language]} *</Form.Label>
                                <Form.Select
                                    name="intervalo"
                                    value={formModal.intervalo}
                                    onChange={e => setFormModal(f => ({ ...f, intervalo: e.target.value }))}
                                    required
                                >
                                    <option value="30">{textos.mensal[language]}</option>
                                    <option value="15">{textos.quinzenal[language]}</option>
                                    <option value="7">{textos.semanal[language]}</option>
                                </Form.Select>
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <div className="d-flex align-items-center">
                                    <Form.Label className="mb-0 me-2">{textos.pessoaConta[language]} *</Form.Label>
                                    <Button
                                        variant="outline-primary"
                                        size="sm"
                                        onClick={() => {
                                            fecharModal();
                                            navigate("/financas/pessoa-conta");
                                        }}
                                        title={textos.pessoaContaCriar[language]}
                                        style={{ whiteSpace: "nowrap" }}
                                    >
                                        + {textos.pessoaConta[language]}
                                    </Button>
                                </div>
                                <Form.Select
                                    name="pessoaContaId"
                                    value={formModal.pessoaContaId}
                                    onChange={e => setFormModal(f => ({ ...f, pessoaContaId: e.target.value }))}
                                    required
                                >
                                    <option value="">{textos.selecione[language]}</option>
                                    {pessoasContas.map(pc => (
                                        <option key={pc.id} value={pc.id}>{pc.nomePessoa}</option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <div className="d-flex align-items-center">
                                    <Form.Label className="mb-0 me-2">{textos.cartao[language]}</Form.Label>
                                    <Button
                                        variant="outline-primary"
                                        size="sm"
                                        onClick={() => {
                                            fecharModal();
                                            navigate("/financas/cadastrar-cartao");
                                        }}
                                        title={textos.cartaoCriar[language]}
                                        style={{ whiteSpace: "nowrap" }}
                                    >
                                        + {textos.cartao[language]}
                                    </Button>
                                </div>
                                <Form.Select
                                    name="cartaoId"
                                    value={formModal.cartaoId}
                                    onChange={e => setFormModal(f => ({ ...f, cartaoId: e.target.value }))}
                                >
                                    <option value="">{textos.selecione[language]}</option>
                                    {cartoes.map(c => (
                                        <option key={c.id} value={c.id}>
                                            {c.nomeUsuario} - {c.nomeBanco}
                                        </option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>{textos.situacao[language]} *</Form.Label>
                                <Form.Select
                                    name="situacao"
                                    value={formModal.situacao}
                                    onChange={e => setFormModal(f => ({ ...f, situacao: e.target.value }))}
                                    required
                                >
                                    {situacoesDisponiveis.map(opt => (
                                        <option key={opt.value} value={opt.value}>{opt.label[language]}</option>
                                    ))}
                                </Form.Select>
                            </Form.Group>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="secondary" onClick={fecharModal}>{textos.cancelar[language]}</Button>
                            <Button variant="primary" type="submit" disabled={salvando}>
                                {salvando ? <Spinner size="sm" animation="border" /> : textos.salvar[language]}
                            </Button>
                        </Modal.Footer>
                    </Form>
                </Modal>
                {/* Modal de exclusão */}
                <Modal show={showDelete} onHide={fecharDelete} centered>
                    <Modal.Header closeButton>
                        <Modal.Title>{textos.excluir[language]}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>{textos.confirmacaoExclusao[language]}</p>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={fecharDelete}>{textos.nao[language]}</Button>
                        <Button variant="danger" onClick={excluirParcelamento}>{textos.sim[language]}</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        </div>
    );
}