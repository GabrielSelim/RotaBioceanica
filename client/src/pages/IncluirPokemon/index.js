import React, { useState } from 'react';
import api from '../../services/api';
import './style.css';
import "bootstrap/dist/css/bootstrap.min.css";
import { useNavigate } from 'react-router-dom';

const accessToken = localStorage.getItem('accessToken');

const exemploPokemon = "Genetic Apex_A1_231_Rapidash_Estrela_Fogo_100_Estagio 1_Charizard";
const exemploTreinador = "Genetic Apex_A1_266_Erika_Duas Estrelas_Suporte_Null_Treinador_Charizard";

export default function IncluirPokemon() {
    const [arquivos, setArquivos] = useState([]);
    const [mensagem, setMensagem] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    function handleFileChange(e) {
        setArquivos([...e.target.files]);
        setMensagem('');
    }

    async function handleSubmit(e) {
        e.preventDefault();
        if (!arquivos.length) {
            setMensagem('Selecione pelo menos uma imagem PNG.');
            return;
        }
        setIsLoading(true);
        const formData = new FormData();
        arquivos.forEach(file => {
            formData.append('files', file);
        });
        try {
            await api.post('/v1/api/pokemon/salvarimagenspokemon', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    Authorization: `Bearer ${accessToken}`,
                },
            });
            setMensagem('Imagens enviadas com sucesso!');
            setArquivos([]);
            e.target.reset();
        } catch (error) {
            setMensagem('Erro ao enviar imagens. Verifique o nome do arquivo e tente novamente.');
        }
        setIsLoading(false);
    }

    return (
        <div className="incluir-pokemon-bg">
            <div className="incluir-pokemon-center">
                <div className="card shadow-lg p-4 incluir-pokemon-card">
                    <div className="d-flex justify-content-between align-items-center mb-3">
                        <button
                            className="btn btn-outline-secondary"
                            onClick={() => navigate(-1)}
                        >
                            Voltar
                        </button>
                        <h2 className="mb-0 text-center flex-grow-1">Incluir Imagem de Pokemon</h2>
                    </div>
                    <div className="regra-bloco mb-3">
                        <div className="regra-titulo mb-2">
                            <span>Regra para o nome da imagem:</span>
                        </div>
                        <div className="regra-exemplos">
                            <div className="regra-exemplo">
                                <span className="fw-bold">Pokemon:</span>
                                <code>{exemploPokemon}.png</code>
                            </div>
                            <div className="regra-exemplo">
                                <span className="fw-bold">Treinador/Item:</span>
                                <code>{exemploTreinador}.png</code>
                            </div>
                        </div>
                        <div className="regra-detalhes mt-2">
                            <span className="fw-bold">Ordem:</span>
                            <span className="ms-1">NomeVersao_NumeroVersao_NúmeroCarta_NomePokemon_Raridade_TipoOuSuporte_HP_EstagioOuTreinador_Booster</span>
                        </div>
                    </div>
                    <form onSubmit={handleSubmit} className="d-flex flex-column align-items-center">
                        <div className="mb-3 w-100">
                            <label htmlFor="fileInput" className="form-label fw-bold">Selecione uma ou mais imagens (.png):</label>
                            <input
                                className="form-control"
                                type="file"
                                id="fileInput"
                                accept=".png"
                                multiple
                                onChange={handleFileChange}
                            />
                        </div>
                        <button
                            type="submit"
                            className="btn btn-primary px-4"
                            disabled={isLoading}
                        >
                            {isLoading ? 'Enviando...' : 'Enviar Imagens'}
                        </button>
                    </form>
                    {mensagem && (
                        <div className={`alert mt-3 ${mensagem.includes('sucesso') ? 'alert-success' : 'alert-danger'}`}>
                            {mensagem}
                        </div>
                    )}
                    <div className="mt-4 small text-secondary dicas-bloco">
                        <strong>Dicas:</strong>
                        <ul className="mb-0 ps-3">
                            <li>O nome do arquivo deve seguir exatamente o padrão acima.</li>
                            <li>Você pode enviar várias imagens de uma vez.</li>
                            <li>Somente arquivos <b>.png</b> são aceitos.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    );
}