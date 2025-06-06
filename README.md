# Global Solution – Monitoramento de Falhas de Energia

Este repositório contém a solução em C# para simular e gerenciar eventos de falha de energia em um data center, contemplando cenários de ataques cibernéticos a sensores de tensão. O sistema é uma aplicação console que registra falhas, gera alertas, mantém logs, produz relatórios em CSV e permite simulações de exceções para validação de entradas.

---

## 1. Finalidade do Sistema

- **Objetivo principal:**  
  Oferecer uma ferramenta simplificada em C# para que um operador de data center possa:
  1. Autenticar-se (login simples).  
  2. Registrar eventos de falha de energia (informando data, hora, sensor e valor de tensão).  
  3. Detectar automaticamente leituras de tensão fora dos limites seguros e gerar alertas categorizados em “Aviso” ou “Crítico”.  
  4. Manter um histórico de logs de todas as operações (cadastro, alerta, erro de validação, autenticação).  
  5. Gerar relatórios de falhas em formato CSV, filtrados por período.  
  6. Testar cenários de exceção (data inválida, campo vazio, número inválido) por meio de um menu de simulação de erros.

Essa aplicação serve para demonstrar boas práticas de Programação Orientada a Objetos em C#, divisão de responsabilidades (SRP), uso de arquivos texto/CSV como fonte de dados e tratamento robusto de exceções via try-catch. 

---

## 2. Instruções de Execução

### 2.1 Pré-requisitos

1. **.NET Framework 4.7.2 ou superior** (instalado no Windows)  
   - Caso não possua Visual Studio, é possível compilar e executar via linha de comando com `csc.exe`.  
2. **Git (opcional)**, para clonar ou versionar o repositório.  
3. **Visual Studio Community 2019/2022** (recomendado) ou **Visual Studio Code + extensão C# (OmniSharp)**.  
4. **Mono** (opcional, para execução em Linux/macOS):
   - Instalar `mono-complete` (Ubuntu/Debian: `sudo apt install mono-complete`).  

### 2.2 Passos para Compilar e Executar

#### 2.2.1 Via Visual Studio (Windows)

1. Clone este repositório ou baixe o ZIP e extraia.  
2. Abra a solução `GlobalSolutionCSharp.sln` no Visual Studio.  
3. Defina `Program.cs` como projeto de início, se necessário.  
4. Pressione **F5** ou clique em **“Iniciar Depuração”**.  
5. O console será exibido. Primeiro, você deverá realizar o **login** (usuário padrão é criado automaticamente se `data/users.csv` estiver vazio).  
6. Após autenticar, o menu principal aparecerá com as opções de “Registrar Falha”, “Gerar Relatório”, “Simular Erro” e “Sair”.

#### 2.2.2 Via Linha de Comando (Windows)

1. Garanta que o .NET Framework SDK (incluindo `csc.exe`) esteja instalado.  
2. Abra o terminal na pasta raiz do projeto (onde está o arquivo `Program.cs`).  
3. Compile todos os arquivos `.cs` de uma só vez. Por exemplo:
    ```bash
    csc /out:GlobalSolutionCSharp.exe /t:exe Models\*.cs Repositories\*.cs Services\*.cs Utils\*.cs Views\*.cs Program.cs
    ```
4. Execute:
```bash
GlobalSolutionCSharp.exe
``` 
5. Siga as instruções no console (login → menu principal → escolha de funcionalidades).

#### 2.2.3 Via Mono (Linux/macOS)
1. Instale o Mono (por exemplo, no Ubuntu: sudo apt install mono-complete).
2. No terminal, navegue até a pasta raiz do projeto.
3. Compile usando:
```bash
mcs -out:GlobalSolutionCSharp.exe Models/*.cs Repositories/*.cs Services/*.cs Utils/*.cs Views/*.cs Program.cs
```
4. Execute com:
```bash
mono GlobalSolutionCSharp.exe
```
5. Interaja com o console conforme instruções (login → menu principal → escolha de funcionalidades).

## 3. Dependências
Este projeto foi desenvolvido sem bibliotecas externas. Todas as funcionalidades foram implementadas utilizando apenas classes da biblioteca padrão do .NET (BCL):
- `System` (tipos básicos, enums, exceções).
- `System.IO` (`StreamReader`, `StreamWriter`, `File`, `Directory`) para leitura/gravação de arquivos texto/CSV.
- `System.Globalization` (`DateTime.ParseExact`) para formatação e parsing de datas/hora.
- `System.Collections.Generic` (`List<T>`, `Func<T>`, `IEnumerable<T>`) para manipulação de coleções.
Não há pacotes NuGet nem frameworks adicionais. Basta ter o compilador C# e o .NET Framework / Mono.

## 4. Estrutura de Pastas
```bash
GlobalSolutionCSharp/
├── Models/  
│   ├── Alerta.cs            # Classe que representa um alerta gerado  
│   ├── FalhaEnergia.cs      # Entidade de um evento de falha de energia  
│   ├── LogEvento.cs         # Classe para gravar logs (herda de Evento abstrato)  
│   ├── User.cs              # Entidade que representa um usuário (login)  
│   └── Enums.cs             # Enums (TipoEvento, NivelAlerta etc.)  
│
├── Repositories/  
│   ├── IRepositorio.cs      # Interface genérica para operações CRUD em arquivo  
│   └── ArquivoRepositorio.cs# Implementação de IRepositorio<T> usando CSV/Textos  
│
├── Services/  
│   ├── AlertaService.cs     # Lógica de geração de alertas (verifica limites de tensão)  
│   ├── FalhaService.cs      # Registra falhas, aciona alertas e gera relatórios  
│   ├── LogService.cs        # Escreve logs de eventos em data/logs.txt  
│   ├── SimuladorErros.cs    # Métodos para forçar exceções (Formatação de data, campo vazio, número inválido)  
│   └── UserService.cs       # Autenticação de usuário (login) e criação de usuário padrão  
│
├── Utils/  
│   ├── Validador.cs         # Métodos auxiliares para validar data, hora e decimais (opcional)  
│   └── CsvHelper.cs         # Métodos para criar cabeçalhos de CSV (opcional)  
│
├── Views/  
│   ├── MenuFalha.cs         # Coleta dados para “Registrar Falha”  
│   ├── MenuPrincipal.cs     # Exibe menu principal e lê opção do usuário  
│   ├── MenuRelatorio.cs     # Coleta período e caminho de saída para relatórios  
│   └── MenuSimulacao.cs     # Exibe opções de simulação de erros e lê entrada  
│
├── Program.cs               # Ponto de entrada: login → menu principal → roteamento de funcionalidades  
├── GlobalSolutionCSharp.sln # Solução do Visual Studio (se disponível)  
├── data/  
│   ├── users.csv            # Armazena credenciais de login (Id;Username;Password)  
│   ├── falhas.csv           # Armazena falhas registradas (Id;DataHora;Sensor;Tensao)  
│   ├── alertas.csv          # Armazena alertas gerados (Id;DataHoraGeracao;Nivel;Mensagem)  
│   └── logs.txt             # Armazena logs de eventos (Timestamp | TipoEvento | Descrição)  
│
└── README.md                # Este arquivo de documentação
```

- **Models/**: Contém todas as classes que representam entidades e enums do domínio.

- **Repositories/**: Define a interface IRepositorio<T> e implementação genérica ArquivoRepositorio<T> para CRUD em arquivo CSV/txt.

- **Services/**: Lógica de negócio do sistema (registro de falhas, geração de alertas, gravação de logs, autenticação, simulação de erros).

- **Utils/**: Classes auxiliares para validações e helpers de CSV (opcional).

- **Views/**: Classes responsáveis por exibir menus e coletar dados do usuário via console, sem lógica de negócio.

- **data/**: Diretório onde os arquivos de persistência são armazenados. Se não existir, a aplicação cria automaticamente.

- **Program.cs**: Roteia o fluxo principal: autenticação → exibição de menu → chamada de serviços conforme opção.

## 5. Resumo das Funcionalidades
### 1. Login (Autenticação)

- Entrada: Username e Password

- Verifica credenciais em data/users.csv

- Se não houver usuário, cria um padrão (admin/admin123)

- Até 3 tentativas; se falhar, encerra a aplicação

### 2. Registrar Falha de Energia

- Entrada: Data (dd/MM/yyyy), Hora (HH:mm), Sensor (string), Tensão (decimal)

- Validações (Formatação → FormatException; Campo vazio → ArgumentException; Tensão ≤ 0 → ArgumentException)

- Cria objeto FalhaEnergia e grava em data/falhas.csv

- Aciona AlertaService para verificar limite de tensão e gerar alerta, se necessário

### 3. Geração de Alerta

- Verifica se tensão está fora do intervalo:

    - < 110 V ou > 240 V → Crítico

    - 110 V ≤ tensão ≤ 115 V ou 235 V ≤ tensão ≤ 240 V → Aviso

- Cria e grava objeto Alerta em data/alertas.csv

- Exibe mensagem destacada no console

- Registra evento em data/logs.txt

### 4. Registro de Logs de Eventos

- Sempre que ocorre:

    - Cadastro de falha

    - Geração de alerta

    - Erro de validação (data/hora/sensor/tensão)

    - Login bem-sucedido ou falha de login

    - Criação de usuário padrão

    - Geração de relatório

    - Simulação de erro

- Um objeto LogEvento (herda de Evento) é criado com:

    - Timestamp = DateTime.Now

    - TipoEvento (enum com valores como CadastroFalha, GeraAlerta, ErroValidacao, LoginSucesso, etc.)

    - Descrição (mensagem explicativa)

Grava a linha formatada em data/logs.txt

### 5. Geração de Relatórios de Status

- Entrada:

    - Data Início (dd/MM/yyyy)

    - Hora Início (HH:mm)

    - Data Fim (dd/MM/yyyy)

    - Hora Fim (HH:mm)

    - Caminho do arquivo de saída (por ex., reports/relatorio.csv)

- Validações:

    - Formato de data/hora → FormatException

    - DataHoraInício ≤ DataHoraFim; se não, exibe erro e registra log

- Filtra todas as falhas em data/falhas.csv entre as datas informadas

- Cria ou sobrescreve o arquivo CSV de saída com cabeçalho Id;DataHora;Sensor;Tensao e escreve as linhas correspondentes

- Registra evento TipoEvento.GerarRelatorio em data/logs.txt

### 6. Simulação de Erros (Try-Catch)

- Apresenta submenu com 3 opções:

    - Simular Data Inválida: chama SimuladorErros.ForcarErroData (lança FormatException)

    - Simular Campo Vazio: chama SimuladorErros.ForcarErroCampoVazio (lança ArgumentException)

    - Simular Número Inválido: chama SimuladorErros.ForcarErroNumero (lança FormatException)

- Cada simulação exibe a mensagem de erro no console e registra TipoEvento.SimulacaoErro em data/logs.txt

## 6. Exemplo de Uso (Fluxo Rápido)
### 1. Ao iniciar, o console exibe:
```bash
** Usuário padrão criado: admin / admin123 **
=== Sistema de Login ===
Username:
```

### 2. Digite:
```bash
Username: admin
Password: admin123
```

### 3. Se autenticado:
```bash
Bem-vindo, admin!
(pausa de 1s)
```

### 4. Surge o Menu Principal:
```bash
=== Global Solution – Monitoramento de Falhas de Energia ===
1) Registrar Falha
2) Gerar Relatório
3) Simular Erro
4) Sair
Escolha uma opção (1-4):
```

### 5. Digite “1” para Registrar Falha:
```bash
=== Registrar Falha de Energia ===
Data (dd/MM/yyyy):
Hora (HH:mm):
Sensor:
Tensão (por ex., 230.5):
```
- Se a tensão estiver fora do intervalo, aparece:
    ```bash
    *** ALERTA (Crítico): Tensão abaixo do limite: 105V no sensor SensorA ***
    ```
    - E o arquivo data/falhas.csv e data/alertas.csv são atualizados.

### 6. Após registrar, volta ao Menu Principal.
### 7. Digite “2” para Gerar Relatório:
```bash
=== Gerar Relatório de Falhas ===
Data Início (dd/MM/yyyy):
Hora Início (HH:mm):
Data Fim (dd/MM/yyyy):
Hora Fim (HH:mm):
Caminho do arquivo de relatório (ex: reports/relatorio.csv):
```
- Se datas válidas, o relatório CSV é criado.

### 8. Digite “3” para Simular Erro:
```bash
=== Simulação de Erros ===
1) Simular data inválida
2) Simular campo vazio
3) Simular número inválido
4) Voltar ao menu principal
```

### 9. Digite “4” para Sair, encerrando a aplicação.

## 7. Integrantes
<table>
  <tr>
    <td align="center">
        <sub>
          <b>João Pedro Marques Rodrigues - RM98307</b>
          <br>
        </sub>
        <sub>
          <b>Natan Eguchi dos Santos - RM98720</b>
          <br>
        </sub>
        <sub>
          <b>Kayky Paschoal Ribeiro - RM99929</b>
          <br>
        </sub>
    </td>
  </tr>
</table>