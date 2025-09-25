const OpcoesMenu = [
  {
    value: '/',
    label: { pt: 'Inicio', en: 'Home' },
    adminOnly: false,
  },
  {
    value: '/livros',
    label: { pt: 'Listar Livros', en: 'List Books' },
    adminOnly: false,
  },
  {
    value: '/amor',
    label: { pt: 'Amor', en: 'Love' },
    adminOnly: true,
  },
  {
    value: '/pokemon',
    label: { pt: 'Tabela Dados Pokemon', en: 'Pokemon Data Table' },
    adminOnly: true,
  },
  {
    value: '/listar-contatos',
    label: { pt: 'Tabela Dados Contatos', en: 'Contacts Data Table' },
    adminOnly: true,
  },
  {
    value: '/financas',
    label: { pt: 'Finanças', en: 'Finances' },
    adminOnly: false,
  },
];

export default OpcoesMenu;