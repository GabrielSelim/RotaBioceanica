FROM mysql:5.7.22
EXPOSE 3306
COPY ./Projeto_Gabriel/db/migrations/ /home/database/
COPY ./Projeto_Gabriel/db/dataset/ /home/database/
COPY ./Projeto_Gabriel/ci/init_database.sh/ /docker-entrypoint-initdb.d/init_database.sh