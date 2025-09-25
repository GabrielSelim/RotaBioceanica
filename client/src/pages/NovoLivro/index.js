import React, { useState, useEffect } from "react";
import './style.css';
import logoImage from '../../assets/LogoSemFundo.png';
import { Link, useNavigate, useParams } from "react-router-dom";
import { FiArrowLeft } from "react-icons/fi";
import api from '../../services/api';

export default function NovoLivro({ language }) {
    const [Escritor, setEscritor] = useState('');
    const [id, setId] = useState(null);
    const [titulo, setTitulo] = useState('');
    const [preco, setPreco] = useState('');
    const [dataLancamento, setDataLancamento] = useState('');
    const [ativo, setAtivo] = useState(true);
    const { livroId } = useParams();
    const accessToken = localStorage.getItem('accessToken');
    const [isLoading, setIsLoading] = useState(true);
    const [erroMensagem, setErroMensagem] = useState('');
    const headers = {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    };

    const navigate = useNavigate();

    useEffect(() => {
        if (livroId !== '0') {
            api.get(`v1/api/livro/${livroId}`, headers)
                .then(response => {
                    setId(response.data.code);
                    setTitulo(response.data.titulo);
                    setEscritor(response.data.Escritor);
                    setPreco(response.data.preco.toString().replace('.', ',')); // Mostra com vírgula se vier ponto
                    setDataLancamento(response.data.dataLancamento.split('T')[0]);
                    setAtivo(response.data.ativo);
                })
                .catch(error => {
                    setErroMensagem(language === 'pt' ? 'Erro ao buscar livro.' : 'Error fetching book.');
                })
                .finally(() => {
                    setIsLoading(false);
                });
        } else {
            setIsLoading(false);
        }
    }, [livroId, accessToken, language]);

    async function cadastrarOuAtualizarLivro(e) {
        e.preventDefault();
        setErroMensagem('');
        if (!titulo || !Escritor || !preco || !dataLancamento) {
            setErroMensagem(language === 'pt' ? 'Todos os campos são obrigatórios!' : 'All fields are required!');
            return;
        }
        // Validação do preço: aceita vírgula ou ponto, só um separador, e precisa ser número positivo
        let precoFormatado = preco.replace(',', '.');
        if (!/^\d+(\.\d{1,2})?$/.test(precoFormatado)) {
            setErroMensagem(language === 'pt' ? 'Preço inválido!' : 'Invalid price!');
            return;
        }
        const precoNumber = Number(precoFormatado);
        if (isNaN(precoNumber) || precoNumber <= 0) {
            setErroMensagem(language === 'pt' ? 'Preço inválido!' : 'Invalid price!');
            return;
        }
        const data = {
            Escritor,
            titulo,
            preco: precoNumber,
            dataLancamento,
            ativo
        };

        try {
            if (livroId === '0') {
                await api.post('v1/api/livro', data, headers);
                navigate('/livros');
            } else {
                data.code = id;
                await api.put(`v1/api/livro`, data, headers);
                navigate('/livros');
            }
        } catch (error) {
            setErroMensagem(language === 'pt' ? 'Erro ao cadastrar livro, tente novamente!' : 'Error registering book, please try again!');
        }
    }

    return (
        <div className="novo-livro-bg d-flex align-items-center justify-content-center">
            <div className="novo-livro-card shadow-lg p-4 rounded-4">
                <div className="text-center mb-4">
                    <img src={logoImage} alt="Logo" className="novo-livro-logo mb-2" />
                    <h2 className="fw-bold text-white mb-2">
                        {livroId === '0'
                            ? (language === 'pt' ? 'Cadastrar Novo Livro' : 'Register New Book')
                            : (language === 'pt' ? 'Atualizar Livro' : 'Update Book')}
                    </h2>
                    <Link to="/livros" className="btn btn-outline-light btn-sm mb-2">
                        <FiArrowLeft className="me-2" />
                        {language === 'pt' ? 'Voltar para Livros' : 'Back to Books'}
                    </Link>
                </div>
                {erroMensagem && (
                    <div className="alert alert-danger py-2 text-center" role="alert">
                        {erroMensagem}
                    </div>
                )}
                <form onSubmit={cadastrarOuAtualizarLivro}>
                    <div className="mb-3">
                        <input
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Título do Livro' : 'Book Title'}
                            value={titulo}
                            onChange={e => setTitulo(e.target.value)}
                        />
                    </div>
                    <div className="mb-3">
                        <input
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Escritor' : 'Author'}
                            value={Escritor}
                            onChange={e => setEscritor(e.target.value)}
                        />
                    </div>
                    <div className="mb-3">
                        <input
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Preço' : 'Price'}
                            value={preco}
                            onChange={e => {
                                let value = e.target.value
                                    .replace(',', '.')
                                    .replace(/[^0-9.]/g, '');
                                // Permite apenas um ponto
                                const parts = value.split('.');
                                if (parts.length > 2) {
                                    value = parts[0] + '.' + parts[1];
                                }
                                setPreco(value.replace('.', ','));
                            }}
                            inputMode="decimal"
                        />
                    </div>
                    <div className="mb-3">
                        <input
                            type="date"
                            className="form-control form-control-lg"
                            placeholder={language === 'pt' ? 'Data de Publicação' : 'Publication Date'}
                            value={dataLancamento}
                            onChange={e => setDataLancamento(e.target.value)}
                        />
                    </div>
                    <div className="mb-3">
                        <select
                            className="form-control form-control-lg"
                            value={ativo}
                            onChange={e => setAtivo(e.target.value === 'true')}
                        >
                            <option value="true">{language === 'pt' ? 'Ativo: Sim' : 'Active: Yes'}</option>
                            <option value="false">{language === 'pt' ? 'Ativo: Não' : 'Active: No'}</option>
                        </select>
                    </div>
                    <button className="btn btn-gradient w-100 py-2 mb-2" type="submit">
                        {livroId === '0'
                            ? (language === 'pt' ? 'Cadastrar' : 'Register')
                            : (language === 'pt' ? 'Atualizar' : 'Update')}
                    </button>
                </form>
            </div>
        </div>
    );
}