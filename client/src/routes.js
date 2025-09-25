import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Livros from './pages/Livros';
import NovoLivro from './pages/NovoLivro';
import Home from './pages/Home';
import Pokemon from './pages/Pokemon';
import IncluirPokemon from './pages/IncluirPokemon';
import Contato from './pages/Contato';
import ListarContatos from './pages/Contato/listarContatos';
import DadosPokemon from './pages/DadosPokemon/DadosPokemon';
import CadastrarCategoriaFinanceira from './pages/Financas/cadastrarCategoriaFinanceira';
import CadastrarCartao from './pages/Financas/cadastrarCartao';
import PessoaConta from './pages/Financas/pessoaConta';
import SobreMim from './pages/SobreMim';
import Agradecimento from './pages/Agradecimento';
import Financa from './pages/Financas';
import LancamentoFinanceiro from './pages/Financas/lancamentoFinancas';
import ParcelamentoFinanceiro from './pages/Financas/parcelamentoFinancas';
import ParcelamentoMensaisFinanceiro from './pages/Financas/parcelamentoMensalFinanca';
import Amor from './pages/Amor/amor';
import CriarUsuario from './pages/CriarUsuario';
import ProtectedRoute from './ProtectedRoute';
import Layout from './Layout';

export default function AppRoutes({ language, setLanguage }) {
    return (
        <BrowserRouter>
            <Routes>
                <Route
                    path="/"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <Home language={language} />
                        </Layout>
                    }
                />
				<Route
                        path="/login"
						element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <Login language={language} />
                        </Layout>
                    }						
                />
                <Route
                        path="/contato"
						element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <Contato language={language} />
                        </Layout>
                    }						
                />
                <Route
                        path="/sobre-mim"
						element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <SobreMim language={language} />
                        </Layout>
                    }						
                />
                <Route
                        path="/agradecimentos"
						element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <Agradecimento language={language} />
                        </Layout>
                    }						
                />
                <Route
                        path="/criar-usuario"
						element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <CriarUsuario language={language} />
                        </Layout>
                    }						
                />
                <Route
                    path="/livros"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <Livros language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/livros/cadastrar/:livroId"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <NovoLivro language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/listar-contatos"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute requiredRole="Admin">
                                <ListarContatos language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <Financa language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas/cadastrar-categoria"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <CadastrarCategoriaFinanceira language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas/cadastrar-cartao"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <CadastrarCartao language={language} />                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas/pessoa-conta"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <PessoaConta language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas/lancamento-financeiro"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <LancamentoFinanceiro language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas/parcelamento"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <ParcelamentoFinanceiro language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/financas/parcelamento-mensais/:id"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute>
                                <ParcelamentoMensaisFinanceiro language={language} />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/pokemon"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute requiredRole="Admin">
                                <DadosPokemon />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/pokemon/alterarsituacao"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute requiredRole="Admin">
                                <Pokemon />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/pokemon/incluirPokemon"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute requiredRole="Admin">
                                <IncluirPokemon />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route
                    path="/amor"
                    element={
                        <Layout language={language} setLanguage={setLanguage}>
                            <ProtectedRoute requiredRole="Admin">
                                <Amor />
                            </ProtectedRoute>
                        </Layout>
                    }
                />
                <Route path="*" element={<Navigate to="/" />} />
            </Routes>
        </BrowserRouter>
    );
}