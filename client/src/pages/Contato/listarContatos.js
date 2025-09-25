import React, { useEffect, useState } from "react";
import api from "../../services/api";
import "bootstrap/dist/css/bootstrap.min.css";
import "./listarContatos.css";

const textos = {
  titulo: { pt: "Mensagens Recebidas", en: "Received Messages" },
  buscar: { pt: "Buscar", en: "Search" },
  buscando: { pt: "Buscando...", en: "Searching..." },
  nenhum: { pt: "Nenhuma mensagem encontrada.", en: "No messages found." },
  visualizar: { pt: "Visualizar", en: "View" },
  fechar: { pt: "Fechar", en: "Close" },
  detalhes: { pt: "Detalhes da Mensagem", en: "Message Details" },
  nome: { pt: "Nome", en: "Name" },
  email: { pt: "E-mail", en: "E-mail" },
  mensagem: { pt: "Mensagem", en: "Message" },
  data: { pt: "Data do Contato", en: "Contact Date" },
  dataInicial: { pt: "Data Inicial", en: "Start Date" },
  dataFinal: { pt: "Data Final", en: "End Date" },
  verMais: { pt: "Ver mais", en: "Show more" },
  verMenos: { pt: "Ver menos", en: "Show less" }
};

export default function ListarContatos({ language = "pt" }) {
  const [contatos, setContatos] = useState([]);
  const [contatosFiltrados, setContatosFiltrados] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [busca, setBusca] = useState("");
  const [buscando, setBuscando] = useState(false);
  const [mensagemSelecionada, setMensagemSelecionada] = useState(null);
  const [dataInicial, setDataInicial] = useState("");
  const [dataFinal, setDataFinal] = useState("");
  const [visibleCount, setVisibleCount] = useState(5);

  const accessToken = localStorage.getItem("accessToken");

  useEffect(() => {
    buscarContatos();
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    aplicarFiltros();
    // eslint-disable-next-line
  }, [contatos, busca, dataInicial, dataFinal]);

  function aplicarFiltros() {
    let filtrados = [...contatos];

    // Filtro por nome
    if (busca.trim()) {
      filtrados = filtrados.filter(c =>
        c.nome.toLowerCase().includes(busca.trim().toLowerCase())
      );
    }

    // Filtro por data
    if (dataInicial) {
      filtrados = filtrados.filter(c =>
        new Date(c.dataContato) >= new Date(dataInicial)
      );
    }
    if (dataFinal) {
      // Considera o final do dia
      const dataFinalObj = new Date(dataFinal);
      dataFinalObj.setHours(23, 59, 59, 999);
      filtrados = filtrados.filter(c =>
        new Date(c.dataContato) <= dataFinalObj
      );
    }

    setContatosFiltrados(filtrados);
    setVisibleCount(5); // Sempre volta para 5 ao aplicar filtro
  }

  async function buscarContatos() {
    setIsLoading(true);
    try {
      const response = await api.get("/v1/api/contato", {
        headers: { Authorization: `Bearer ${accessToken}` }
      });
      setContatos(response.data);
    } catch {
      setContatos([]);
    }
    setIsLoading(false);
  }

  async function visualizarMensagem(id) {
    try {
      const response = await api.get(`/v1/api/contato/${id}`, {
        headers: { Authorization: `Bearer ${accessToken}` }
      });
      setMensagemSelecionada(response.data);
    } catch {
      setMensagemSelecionada(null);
    }
  }

  function fecharModal() {
    setMensagemSelecionada(null);
  }

  function handleBuscar(e) {
    e.preventDefault();
    aplicarFiltros();
  }

  function handleVerMais() {
    setVisibleCount((prev) => prev + 5);
  }

  function handleVerMenos() {
    setVisibleCount(5);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  // Mensagens a serem exibidas na página
  const mensagensVisiveis = contatosFiltrados.slice(0, visibleCount);

  return (
    <div className="contato-bg">
      <div className="contato-card shadow-lg listar-contatos-card">
        <h2 className="contato-title mb-3">{textos.titulo[language]}</h2>
        <form className="d-flex flex-wrap mb-3 gap-2 align-items-end" onSubmit={handleBuscar}>
          <div>
            <label className="form-label mb-0" style={{ fontSize: "0.95rem" }}>
              {textos.nome[language]}
            </label>
            <input
              type="text"
              className="form-control"
              placeholder={language === "pt" ? "Buscar por nome..." : "Search by name..."}
              value={busca}
              onChange={e => setBusca(e.target.value)}
              style={{ maxWidth: 220 }}
            />
          </div>
          <div>
            <label className="form-label mb-0" style={{ fontSize: "0.95rem" }}>
              {textos.dataInicial[language]}
            </label>
            <input
              type="date"
              className="form-control"
              value={dataInicial}
              onChange={e => setDataInicial(e.target.value)}
              style={{ minWidth: 130 }}
            />
          </div>
          <div>
            <label className="form-label mb-0" style={{ fontSize: "0.95rem" }}>
              {textos.dataFinal[language]}
            </label>
            <input
              type="date"
              className="form-control"
              value={dataFinal}
              onChange={e => setDataFinal(e.target.value)}
              style={{ minWidth: 130 }}
            />
          </div>
          <button className="btn btn-gradient" type="submit" disabled={buscando}>
            {buscando ? textos.buscando[language] : textos.buscar[language]}
          </button>
        </form>
        {isLoading ? (
          <div className="text-center py-5">
            <div className="spinner-border text-info" role="status"></div>
          </div>
        ) : (
          <>
            {/* Tabela para desktop */}
            <div className="table-responsive d-none d-md-block">
              <table className="table table-bordered align-middle listar-contatos-table">
                <thead>
                  <tr>
                    <th>{textos.nome[language]}</th>
                    <th>{textos.email[language]}</th>
                    <th>{textos.data[language]}</th>
                    <th className="text-center">{textos.visualizar[language]}</th>
                  </tr>
                </thead>
                <tbody>
                  {mensagensVisiveis.length === 0 ? (
                    <tr>
                      <td colSpan={4} className="text-center text-secondary">
                        {textos.nenhum[language]}
                      </td>
                    </tr>
                  ) : (
                    mensagensVisiveis.map((contato) => (
                      <tr key={contato.id}>
                        <td>{contato.nome}</td>
                        <td>{contato.email}</td>
                        <td>{new Date(contato.dataContato).toLocaleString(language === "pt" ? "pt-BR" : "en-US")}</td>
                        <td className="text-center">
                          <button
                            className="btn btn-info btn-sm"
                            onClick={() => visualizarMensagem(contato.id)}
                          >
                            {textos.visualizar[language]}
                          </button>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
            {/* Cards para mobile */}
            <div className="listar-contatos-mobile d-block d-md-none">
              {mensagensVisiveis.length === 0 ? (
                <div className="text-center text-secondary py-3">
                  {textos.nenhum[language]}
                </div>
              ) : (
                mensagensVisiveis.map((contato) => (
                  <div className="listar-contatos-card-mobile mb-3" key={contato.id}>
                    <div>
                      <strong>{textos.nome[language]}:</strong> {contato.nome}
                    </div>
                    <div>
                      <strong>{textos.email[language]}:</strong> {contato.email}
                    </div>
                    <div>
                      <strong>{textos.data[language]}:</strong> {new Date(contato.dataContato).toLocaleString(language === "pt" ? "pt-BR" : "en-US")}
                    </div>
                    <div className="mt-2 text-end">
                      <button
                        className="btn btn-info btn-sm"
                        onClick={() => visualizarMensagem(contato.id)}
                      >
                        {textos.visualizar[language]}
                      </button>
                    </div>
                  </div>
                ))
              )}
            </div>
            {/* Paginação */}
            <div className="d-flex justify-content-center gap-2 mt-3">
              {visibleCount < contatosFiltrados.length && (
                <button className="btn btn-outline-info" onClick={handleVerMais}>
                  {textos.verMais[language]}
                </button>
              )}
              {visibleCount > 5 && (
                <button className="btn btn-outline-secondary" onClick={handleVerMenos}>
                  {textos.verMenos[language]}
                </button>
              )}
            </div>
          </>
        )}
      </div>

      {/* Modal de visualização */}
      {mensagemSelecionada && (
        <div className="listar-contatos-modal-bg">
          <div className="listar-contatos-modal" tabIndex="-1">
            <div className="listar-contatos-modal-header">
              <span className="listar-contatos-modal-title">
                <i className="bi bi-envelope-paper-heart me-2"></i>
                {textos.detalhes[language]}
              </span>
              <button
                className="listar-contatos-modal-close"
                onClick={fecharModal}
                aria-label="Fechar"
                title="Fechar"
              >
                &times;
              </button>
            </div>
            <div className="listar-contatos-modal-body">
              <div className="modal-detail-row">
                <span className="listar-contatos-modal-label">{textos.nome[language]}</span>
                <span className="listar-contatos-modal-value">{mensagemSelecionada.nome}</span>
              </div>
              <div className="modal-detail-row">
                <span className="listar-contatos-modal-label">{textos.email[language]}</span>
                <span className="listar-contatos-modal-value">{mensagemSelecionada.email}</span>
              </div>
              <div className="modal-detail-row">
                <span className="listar-contatos-modal-label">{textos.data[language]}</span>
                <span className="listar-contatos-modal-value">
                  {new Date(mensagemSelecionada.dataContato).toLocaleString(language === "pt" ? "pt-BR" : "en-US")}
                </span>
              </div>
              <div className="modal-detail-row">
                <span className="listar-contatos-modal-label">{textos.mensagem[language]}</span>
                <div className="listar-contatos-modal-mensagem">
                  {mensagemSelecionada.mensagem}
                </div>
              </div>
            </div>
            <div className="listar-contatos-modal-footer">
              <button className="btn btn-secondary" onClick={fecharModal}>
                {textos.fechar[language]}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}