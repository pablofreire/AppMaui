#!/bin/bash

echo "Parando e removendo contêineres Docker do SportSphere..."
docker-compose down

echo "Removendo volumes Docker do SportSphere..."
docker-compose down -v

echo "Removendo imagens Docker do SportSphere..."
docker rmi $(docker images | grep "sportsphere" | awk '{print $3}') 2>/dev/null || true

echo "Limpeza concluída!" 