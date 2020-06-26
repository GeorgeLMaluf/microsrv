# microsrv

Um serviço simples escrito em C# usando .NET Core, utilizando o serviço Apache Kafka como mecanismo de troca de mensagens.

# Requisitos
- .NET CORE SDK 3.1
- Docker CLI

# Configuração

Para ser executado, é necessário utilizar o Docker Compose para subir os containers do Apache Kafka.

Execute a seguinte linha de comando:

$ docker-compose up -d

# Executando como publicador de mensagens
Abra uma instancia do seu intepretador de linhas de comando (bash, cmd, powershell, etc.) e dentro da pasta app, execute a linha:

$ dotnet run --p localhost:9092

Uma mensagem 'Hello World!' será enviada a cada 5 segundos para o servidor

# Executando como leitor de mensagens
Abra uma instancia do seu intepretador de linhas de comando (bash, cmd, powershell, etc.) e dentro da pasta app, execute a linha:

$ dotnet run --c localhost:9092

# Obtendo ajuda da linha de comando
Abra uma instancia do seu intepretador de linhas de comando (bash, cmd, powershell, etc.) e dentro da pasta app, execute a linha:
$ dotnet run

