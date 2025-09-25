# Etapa 1: Construção do aplicativo React
FROM node:20-alpine AS build

# Defina o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copie os arquivos de dependências para o contêiner
COPY package*.json ./

# Instale as dependências do projeto
RUN npm install

# Copie o restante do código do projeto para o contêiner
COPY . .

# Construa o projeto para produção
RUN npm run build

# Etapa 2: Servir os arquivos estáticos com Nginx
FROM nginx:alpine

# Copie os arquivos construídos para o diretório padrão do Nginx
COPY --from=build /app/build /usr/share/nginx/html

# Copie a configuração customizada do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exponha a porta 80 para o servidor Nginx
EXPOSE 80

# Comando para iniciar o Nginx
CMD ["nginx", "-g", "daemon off;"]