import React, { useState, useEffect } from "react";
import './style.css';
import { Link, useNavigate } from "react-router-dom";
import { FiEdit, FiTrash2 } from "react-icons/fi";
import api from '../../services/api';

export default function Livros({ language }) {
    const [livros, setLivros] = useState([]);
    const [pagina, setPagina] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [isLastPage, setIsLastPage] = useState(false);
    const nomeUsuario = localStorage.getItem('nomeUsuario');
    const accessToken = localStorage.getItem('accessToken');
    const [erroMensagem, setErroMensagem] = useState('');
    const navigate = useNavigate();

    const headers = {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    }

    useEffect(() => {
        carregarLivrosIniciais();
        // eslint-disable-next-line
    }, [accessToken, language]);

    async function carregarLivrosIniciais() {
        setIsLoading(true);
        setErroMensagem('');
        setIsLastPage(false);
        setPagina(1);
        try {
            const response = await api.get(`v1/api/livro/desc/4/1`, headers);
            setLivros(response.data.list);
            setIsLastPage(response.data.list.length < 4);
        } catch (error) {
            setErroMensagem(language === 'pt' ? "Erro ao buscar livros." : "Error fetching books.");
        }
        setIsLoading(false);
    }

    async function buscarMaisLivros() {
        setIsLoading(true);
        setErroMensagem('');
        try {
            const response = await api.get(`v1/api/livro/desc/4/${pagina + 1}`, headers);
            if (response.data.list.length === 0) {
                setIsLastPage(true);
            } else {
                setLivros([...livros, ...response.data.list]);
                setPagina(pagina + 1);
                if (response.data.list.length < 4) setIsLastPage(true);
            }
        } catch (error) {
            if (error.response && error.response.status === 404) {
                setIsLastPage(true);
            } else {
                setErroMensagem(language === 'pt' ? "Erro ao buscar livros." : "Error fetching books.");
            }
        }
        setIsLoading(false);
    }

    async function editarLivro(code) {
        navigate(`/livros/cadastrar/${code}`);
    }

    async function deletarLivro(code) {
        try {
            await api.delete(`v1/api/livro/${code}`, headers);
            setLivros(livros.filter(livro => livro.code !== code));
        } catch (error) {
            setErroMensagem(language === 'pt' ? "Erro ao deletar livro." : "Error deleting book.");
        }
    }

    function verMenos() {
        carregarLivrosIniciais();
    }

    // Loader enquanto carrega livros
    if (isLoading) {
        return (
            <div className="livros-bg py-5" style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center" }}>
                <div className="spinner-border text-info" role="status" style={{ width: 60, height: 60 }}>
                    <span className="visually-hidden">Carregando...</span>
                </div>
            </div>
        );
    }

    return (
        <div className="livros-bg py-5">
            <div className="container livros-container shadow-lg rounded-4 p-4">
                <div className="d-flex flex-column flex-md-row align-items-center justify-content-between mb-4">
                    <h2 className="fw-bold text-white mb-3 mb-md-0">
                        {language === 'pt' ? "Livros Cadastrados" : "Registered Books"}
                    </h2>
                    <Link className="btn btn-gradient" to="cadastrar/0">
                        {language === 'pt' ? "+ Criar Novo Livro" : "+ Add New Book"}
                    </Link>
                </div>
                {erroMensagem && (
                    <div className="alert alert-danger py-2 text-center" role="alert">
                        {erroMensagem}
                    </div>
                )}
                <div className="row g-4">
                    {livros.map(livro => (
                        <div key={livro.code} className="col-12 col-md-6">
                            <div className="card livro-card h-100 shadow-sm">
                                <div className="card-body">
                                    <h5 className="fw-bold mb-2" style={{ color: "#00c6ff" }}>{livro.titulo}</h5>
                                    <p className="mb-1"><strong>{language === 'pt' ? "Autor:" : "Author:"}</strong> {livro.Escritor}</p>
                                    <p className="mb-1"><strong>{language === 'pt' ? "Preço:" : "Price:"}</strong> {Intl.NumberFormat(language === 'pt' ? 'pt-br' : 'en-US', { style: 'currency', currency: 'BRL' }).format(livro.preco)}</p>
                                    <p className="mb-1"><strong>{language === 'pt' ? "Data de Publicação:" : "Publication Date:"}</strong> {Intl.DateTimeFormat(language === 'pt' ? 'pt-BR' : 'en-US').format(new Date(livro.dataLancamento))}</p>
                                    <p className="mb-2"><strong>{language === 'pt' ? "Ativo?" : "Active?"}</strong> {livro.ativo ? (language === 'pt' ? 'Sim' : 'Yes') : (language === 'pt' ? 'Não' : 'No')}</p>
                                    <div className="d-flex gap-2">
                                        <button onClick={() => editarLivro(livro.code)} className="btn btn-outline-light btn-sm">
                                            <FiEdit size={18} />
                                        </button>
                                        <button onClick={() => deletarLivro(livro.code)} className="btn btn-outline-danger btn-sm">
                                            <FiTrash2 size={18} />
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
                <div className="d-flex justify-content-center mt-4">
                    {!isLastPage ? (
                        <button className="btn btn-gradient" onClick={buscarMaisLivros} disabled={isLoading}>
                            {isLoading
                                ? (language === 'pt' ? "Carregando..." : "Loading...")
                                : (language === 'pt' ? "Carregar mais" : "Load more")}
                        </button>
                    ) : (
                        <button className="btn btn-outline-light" onClick={verMenos}>
                            {language === 'pt' ? "Ver menos" : "See less"}
                        </button>
                    )}
                </div>
            </div>
        </div>
    );
}