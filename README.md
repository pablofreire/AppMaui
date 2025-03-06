# SportSphere

SportSphere é uma rede social voltada para esportes que permite aos usuários encontrar e participar de atividades esportivas próximas, descobrir estabelecimentos esportivos e conectar-se com outros entusiastas de esportes.

## Funcionalidades

- **Mapa Interativo**: Visualize eventos esportivos e estabelecimentos próximos em um mapa.
- **Perfil de Usuário**: Crie um perfil com seus esportes favoritos e preferências.
- **Eventos Esportivos**: Crie, participe e gerencie eventos esportivos.
- **Estabelecimentos**: Encontre locais para praticar seus esportes favoritos.
- **Comunidade**: Conecte-se com outros entusiastas de esportes.

## Tecnologias Utilizadas

- **Backend**: ASP.NET Core 9 Web API
- **Frontend**: .NET MAUI (Multi-platform App UI)
- **Banco de Dados**: SQL Server
- **Autenticação**: JWT (JSON Web Tokens)
- **Mapeamento**: Microsoft.Maui.Controls.Maps
- **Containerização**: Docker e Docker Compose

## Estrutura do Projeto

- **SportSphere.API**: Backend em ASP.NET Core 9 Web API
- **SportSphere.App**: Aplicativo móvel em .NET MAUI
- **SportSphere.Shared**: Biblioteca compartilhada com modelos e DTOs

## Requisitos

- .NET 9 SDK
- SQL Server (ou Docker para executar o SQL Server em contêiner)
- Visual Studio 2022 ou superior (recomendado)
- Docker e Docker Compose (opcional, para execução em contêineres)

## Configuração e Execução

### Método 1: Execução Local

1. Clone o repositório
2. Configure a string de conexão no arquivo `appsettings.json` do projeto API
3. Execute as migrações do Entity Framework Core para criar o banco de dados:
   ```
   dotnet ef database update
   ```
4. Execute o projeto API:
   ```
   dotnet run --project SportSphere.API
   ```
5. Execute o projeto MAUI:
   ```
   dotnet run --project SportSphere.App
   ```

### Método 2: Execução com Docker

1. Clone o repositório
2. Execute o script para iniciar os contêineres Docker:
   ```bash
   # Linux/macOS
   ./run-docker.sh
   
   # Windows
   run-docker.bat
   ```
3. Aguarde a inicialização dos serviços (pode levar alguns minutos na primeira execução)
4. A API estará disponível em `http://localhost:5000`
5. Para executar o aplicativo MAUI, use o script:
   ```bash
   # Linux/macOS
   ./run-app.sh
   
   # Windows
   run-app.bat
   ```

### Dados de Exemplo

O projeto inclui um script SQL (`init-sample-data.sql`) que popula o banco de dados com dados de exemplo:

- **Usuários**: 5 usuários incluindo um administrador
  - Credenciais de exemplo: `joao@example.com` / `123456`
  - Credenciais de administrador: `admin@sportsphere.com` / `123456`
- **Esportes**: 10 esportes diferentes
- **Estabelecimentos**: 5 estabelecimentos esportivos
- **Eventos**: 5 eventos esportivos
- **Localizações**: 5 localizações em diferentes cidades

Estes dados são carregados automaticamente quando você executa o projeto com Docker.

### Ambiente de Desenvolvimento

Para desenvolvimento, você pode usar o arquivo `docker-compose.dev.yml`:

```bash
# Linux/macOS
docker-compose -f docker-compose.dev.yml up -d

# Windows
docker-compose -f docker-compose.dev.yml up -d
```

Este arquivo configura volumes para permitir edição de código em tempo real.

### Ambiente de Produção

Para implantação em produção, você pode usar o arquivo `docker-compose.prod.yml`:

```bash
# Linux/macOS
./run-prod.sh

# Windows
run-prod.bat
```

Este arquivo configura a aplicação para execução em ambiente de produção, com configurações otimizadas.

### Scripts Utilitários

O projeto inclui vários scripts para facilitar o desenvolvimento e a implantação:

- **run-docker.sh/bat**: Inicia os contêineres Docker em modo de desenvolvimento
- **run-app.sh/bat**: Executa o aplicativo MAUI após iniciar os contêineres Docker
- **run-prod.sh/bat**: Inicia os contêineres Docker em modo de produção
- **run-migrations.sh/bat**: Executa as migrações do Entity Framework Core no contêiner Docker
- **view-logs.sh/bat**: Visualiza os logs dos contêineres Docker
- **clean-docker.sh/bat**: Remove todos os contêineres, volumes e imagens Docker relacionados ao projeto

### Notas sobre Docker

- O banco de dados SQL Server será executado em um contêiner com os dados persistidos em um volume Docker
- A API será construída e executada em um contêiner separado
- A string de conexão já está configurada para acessar o banco de dados no contêiner
- Para parar os serviços, execute:
  ```
  docker-compose down
  ```
- Para remover todos os dados e recomeçar do zero:
  ```
  docker-compose down -v
  ```

## Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para mais detalhes.

## Contato

Para mais informações, entre em contato com a equipe de desenvolvimento.

# SportSphere - Scripts de Banco de Dados

Este diretório contém os scripts SQL necessários para configurar o banco de dados do aplicativo SportSphere.

## Arquivos

- `database_schema.sql`: Script para criar o esquema do banco de dados (tabelas, índices, chaves primárias e estrangeiras)
- `initial_data.sql`: Script para inserir dados iniciais nas tabelas
- `setup_database.sql`: Script principal que executa os dois scripts acima em sequência

## Como usar

### Opção 1: Executar o script de configuração completo

1. Abra o SQL Server Management Studio (SSMS)
2. Conecte-se ao seu servidor SQL
3. Abra o arquivo `setup_database.sql`
4. Execute o script (F5 ou botão "Execute")

### Opção 2: Executar os scripts individualmente

Se preferir executar os scripts separadamente:

1. Execute primeiro o `database_schema.sql` para criar o banco de dados e as tabelas
2. Em seguida, execute o `initial_data.sql` para inserir os dados iniciais

## Estrutura do Banco de Dados

O banco de dados SportSphere contém as seguintes tabelas principais:

- `User`: Armazena informações dos usuários do sistema
- `Location`: Armazena informações de localização (coordenadas, endereço)
- `Sport`: Armazena informações sobre modalidades esportivas
- `Venue`: Armazena informações sobre locais para prática de esportes
- `Event`: Armazena informações sobre eventos esportivos

E as seguintes tabelas de relacionamento:

- `UserFavoriteSport`: Relaciona usuários com seus esportes favoritos
- `EventParticipant`: Relaciona usuários participantes de eventos
- `VenueSport`: Relaciona locais (venues) com os esportes oferecidos

## Dados de Exemplo

O script `initial_data.sql` insere dados de exemplo em todas as tabelas, incluindo:

- 10 localizações em diferentes cidades brasileiras
- 10 modalidades esportivas
- 10 usuários
- 10 locais para prática de esportes
- 10 eventos esportivos
- Relacionamentos entre usuários, esportes, eventos e locais

## Requisitos

- SQL Server 2019 ou superior
- Permissões para criar banco de dados e tabelas no servidor SQL

## Observações

- Os scripts foram projetados para serem executados em um ambiente SQL Server
- Certifique-se de ter permissões adequadas no servidor antes de executar os scripts
- Os dados de exemplo são fictícios e destinados apenas para fins de teste e demonstração 