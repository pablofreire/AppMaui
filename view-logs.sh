#!/bin/bash

if [ "$1" == "api" ]; then
    echo "Visualizando logs do contêiner da API..."
    docker-compose logs -f api
elif [ "$1" == "db" ]; then
    echo "Visualizando logs do contêiner do banco de dados..."
    docker-compose logs -f db
else
    echo "Visualizando logs de todos os contêineres..."
    docker-compose logs -f
fi 