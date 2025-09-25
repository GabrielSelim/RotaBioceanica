#!/bin/bash
MYSQL_HOST=db
MYSQL_PORT=3306
MYSQL_USER=$MYSQL_USER
MYSQL_PASSWORD=$MYSQL_PASSWORD
MYSQL_DATABASE=$MYSQL_DATABASE

echo "MYSQL_USER=$MYSQL_USER"
echo "MYSQL_PASSWORD=$MYSQL_PASSWORD"



for i in `find /home/database/ -name "*.sql" | sort --version-sort`; do mysql -u"$MYSQL_USER" -p"$MYSQL_PASSWORD" projetogsystem < $i; done;