# OrgaTask Maui

## Visão Geral

OrgaTask Maui é uma aplicação multiplataforma desenvolvida com .NET MAUI, projetada para gerenciar tarefas de forma eficiente e intuitiva. A aplicação consome a OrgaTask API, permitindo aos usuários organizar, rastrear e gerenciar suas tarefas em dispositivos Android, iOS, macOS e Windows. Com uma interface nativa e responsiva, construída com XAML e seguindo a arquitetura MVVM, OrgaTask Maui oferece uma experiência fluida, ideal para produtividade pessoal ou em equipe.

## Tecnologias Utilizadas

- **Core:** .NET MAUI (.NET 9)
- **Arquitetura:** MVVM
- Interface: XAML (*.xaml) para layouts nativos
- ViewModel: Classes para estado e lógica da UI
- Service Layer: Comunicação com API e lógica de negócio

- **Comunicação:**

  - Consumo de API REST via HttpClient
  - Autenticação com JWT Bearer Tokens
  - Serialização JSON com System.Text.Json

- **Injeção de Dependência:** Nativa do .NET (IServiceCollection)



Plataformas Suportadas: Android (API 21+), iOS (13+), macOS (10.15+), Windows (10+)

## Funcionalidades

- Login e autenticação com JWT
- Gerenciamento de tarefas (listar, criar, editar, excluir)
- Interface responsiva e interativa
- Tratamento de erros e feedback visual
- Sincronização com API: Atualização em tempo real com a OrgaTask API.

## Pré-requisitos

- .NET 9 SDK
- OrgaTask API rodando localmente ou em um servidor
- Visual Studio 2022 ou 2025 com workload MAUI
- Dispositivo ou emulador Android/iOS ou Windows Desktop

## Como Executar o Projeto

1. Clone o repositório:

```bash
git clone https://github.com/Jabonelas/OrgaTask-Maui.git
cd OrgaTask-Maui
```

2. Restaure as dependências:

```bash
dotnet restore
```

3. Configure a URL da API:

- Abra o arquivo MauiProgram.cs ou arquivo de configuração equivalente.
- Defina a URL da OrgaTask API (padrão: https://localhost:7170/ para desenvolvimento).

4. Execute a aplicação:


- Android:
 
```bash
dotnet run --framework net8.0-android
```

- iOS (requer macOS):

```bash
dotnet run --framework net8.0-ios
```

- Windows:

```bash
dotnet run --framework net8.0-windows
```

- macOS:
- 
```bash
dotnet run --framework net8.0-macos
```

Ou use o Visual Studio, selecionando a plataforma desejada.


5. Acesse a aplicação:

- Em dispositivos móveis: Executa no emulador ou dispositivo conectado.
- Em desktop: Abre como aplicativo nativo.




## Exemplo de Uso

1. Acesse a página de login e insira credenciais válidas.

2. Após o login, visualize e gerencie suas tarefas na dashboard.

3. Use os formulários para criar ou editar tarefas.



<p><em>Interface Dashboard</em></p>

<img width="410" height="910" alt="image" src="https://github.com/user-attachments/assets/8356278a-292f-4934-9df1-8dbea00fab5a" />

> Painel visual com acompanhamento do progresso e status de todas as atividades

<p><em>Interface Tarefas</em></p>

<img width="407" height="911" alt="image" src="https://github.com/user-attachments/assets/20a736d2-394d-4b9c-b0c9-fe0b5bc3f86a" />

> Visualização integrada de todas as tarefas registradas

Contribuições

Contribuições são bem-vindas! Abra issues para relatar bugs ou sugerir melhorias, ou envie pull requests.
