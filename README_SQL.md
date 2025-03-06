# SportSphere - Scripts de Banco de Dados

Este diretório contém os scripts SQL necessários para configurar o banco de dados do aplicativo SportSphere.

## Arquivos

- `database_schema.sql`: Script para criar o esquema do banco de dados (tabelas, índices, chaves primárias e estrangeiras)
- `initial_data.sql`: Script para inserir dados iniciais nas tabelas
- `setup_database.sql`: Script principal que executa os dois scripts acima em sequência
- `verify_data.sql`: Script para verificar se os dados foram inseridos corretamente

## Como usar

### Opção 1: Executar o script de configuração completo

1. Abra o SQL Server Management Studio (SSMS)
2. Conecte-se ao seu servidor SQL
3. Abra o arquivo `setup_database.sql`
4. Execute o script (F5 ou botão "Execute")
5. Para verificar os dados, execute o script `verify_data.sql`

### Opção 2: Executar os scripts individualmente

Se preferir executar os scripts separadamente:

1. Execute primeiro o `database_schema.sql` para criar o banco de dados e as tabelas
2. Em seguida, execute o `initial_data.sql` para inserir os dados iniciais
3. Por fim, execute o `verify_data.sql` para verificar se os dados foram inseridos corretamente

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

## Verificação de Dados

O script `verify_data.sql` permite verificar se os dados foram inseridos corretamente no banco de dados. Ele exibe:

- Contagem de registros em cada tabela
- Lista de usuários cadastrados
- Lista de esportes cadastrados
- Lista de eventos com detalhes
- Esportes favoritos de cada usuário
- Participantes de cada evento
- Esportes oferecidos em cada local

## Requisitos

- SQL Server 2019 ou superior
- Permissões para criar banco de dados e tabelas no servidor SQL

## Observações

- Os scripts foram projetados para serem executados em um ambiente SQL Server
- Certifique-se de ter permissões adequadas no servidor antes de executar os scripts
- Os dados de exemplo são fictícios e destinados apenas para fins de teste e demonstração 