import React from "react";
import { useNavigate } from "react-router-dom";
import "./style.css";
import "bootstrap/dist/css/bootstrap.min.css";

const textos = {
    painel: { pt: "Painel Financeiro", en: "Finance Panel" },
    gerenciarCartoes: { pt: "Gerenciar Cartões", en: "Manage Cards" },
    cadastrarPessoaConta: { pt: "Cadastrar Nova Pessoa Conta", en: "Register New Account Holder" },
    gerenciarCategorias: { pt: "Gerenciar Categorias Financeiras", en: "Manage Financial Categories" },
    lancamentoFinanceiro: { pt: "Lançamento Financeiro", en: "Financial Entry" },
    parcelamentoFinanceiro: { pt: "Parcelamento Financeiro", en: "Financial Installment" },
    emBreve: { pt: "Em breve: gráficos e análises financeiras!", en: "Coming soon: financial charts and analytics!" },
    explicacoes: {
        cadastrarCartao: {
            pt: "Cadastre e gerencie todos os seus cartões de crédito e débito. Mantenha o controle dos bancos e titulares para facilitar lançamentos e parcelamentos.",
            en: "Register and manage all your credit and debit cards. Keep track of banks and holders to make entries and installments easier."
        },
        pessoaConta: {
            pt: "Adicione pessoas ou contas para associar aos seus lançamentos e parcelamentos. Ideal para separar despesas e receitas por membro da família ou conta bancária.",
            en: "Add people or accounts to link to your entries and installments. Great for separating expenses and income by family member or bank account."
        },
        categoria: {
            pt: "Crie e organize categorias financeiras para classificar seus lançamentos. Isso ajuda a visualizar para onde vai seu dinheiro e analisar seus gastos e receitas.",
            en: "Create and organize financial categories to classify your entries. This helps you see where your money goes and analyze your expenses and income."
        },
        lancamento: {
            pt: "Registre receitas e despesas do dia a dia. Aqui você pode lançar pagamentos, recebimentos, associar categorias, marcar como pago e acompanhar o saldo mensal.",
            en: "Record daily income and expenses. Here you can add payments, receipts, assign categories, mark as paid, and track your monthly balance."
        },
        parcelamento: {
            pt: "Gerencie parcelamentos de compras ou dívidas. Cadastre, edite e acompanhe o status de cada parcelamento, associando a cartões e contas.",
            en: "Manage purchase or debt installments. Register, edit, and track the status of each installment, linking to cards and accounts."
        }
    }
};

export default function Financas({ language = "pt" }) {
    const navigate = useNavigate();

    return (
        <div className="financas-bg d-flex align-items-center justify-content-center min-vh-100">
            <div className="container financas-container shadow-lg rounded-4 p-5">
                <h2 className="fw-bold text-primary mb-4 text-center">{textos.painel[language]}</h2>
                <div className="financas-btns-list">
                    <div className="financas-btn-expl">
                        <button
                            className="btn btn-gradient btn-lg fw-bold"
                            onClick={() => navigate("/financas/lancamento-financeiro")}
                        >
                            {textos.lancamentoFinanceiro[language]}
                        </button>
                        <div className="financas-explicacao">
                            {textos.explicacoes.lancamento[language]}
                        </div>
                    </div>
                    <div className="financas-btn-expl">
                        <button
                            className="btn btn-gradient btn-lg fw-bold"
                            onClick={() => navigate("/financas/cadastrar-cartao")}
                        >
                            {textos.gerenciarCartoes[language]}
                        </button>
                        <div className="financas-explicacao">
                            {textos.explicacoes.cadastrarCartao[language]}
                        </div>
                    </div>
                    <div className="financas-btn-expl">
                        <button
                            className="btn btn-gradient btn-lg fw-bold"
                            onClick={() => navigate("/financas/pessoa-conta")}
                        >
                            {textos.cadastrarPessoaConta[language]}
                        </button>
                        <div className="financas-explicacao">
                            {textos.explicacoes.pessoaConta[language]}
                        </div>
                    </div>
                    <div className="financas-btn-expl">
                        <button
                            className="btn btn-gradient btn-lg fw-bold"
                            onClick={() => navigate("/financas/cadastrar-categoria")}
                        >
                            {textos.gerenciarCategorias[language]}
                        </button>
                        <div className="financas-explicacao">
                            {textos.explicacoes.categoria[language]}
                        </div>
                    </div>
                    <div className="financas-btn-expl">
                        <button
                            className="btn btn-gradient btn-lg fw-bold"
                            onClick={() => navigate("/financas/parcelamento")}
                        >
                            {textos.parcelamentoFinanceiro[language]}
                        </button>
                        <div className="financas-explicacao">
                            {textos.explicacoes.parcelamento[language]}
                        </div>
                    </div>
                </div>
                {/* <div className="mt-5 text-center text-secondary">
                    <p>{textos.emBreve[language]}</p>
                </div> */}
            </div>
        </div>
    );
}