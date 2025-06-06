using GS_CSHARP.Models;
using GS_CSHARP.Repositories;
using GS_CSHARP.Services;
using GS_CSHARP.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace GS_CSHARP
{
    class Program
    {
        static void Main(string[] args)
        {
            // ----------------------------------------------------
            // 1) CONFIGURAÇÃO DE ARQUIVOS E REPOSITÓRIOS
            // ----------------------------------------------------

            // Caminhos para cada tipo de dado
            var pathUsers = "data/users.csv";
            var pathFalhas = "data/falhas.csv";
            var pathAlertas = "data/alertas.csv";
            var pathLogs = "data/logs.txt";

            // 1.1) Repositório para Usuários
            Func<string, User> parseUser = linha =>
            {
                // Formato esperado: "Id;Username;Password"
                var p = linha.Split(';');
                int id = int.Parse(p[0]);
                string username = p[1];
                string password = p[2];
                return new User(id, username, password);
            };
            Func<User, string> toCsvUser = u => u.ToCsv();
            var repoUsers = new ArquivoRepositorio<User>(pathUsers, parseUser, toCsvUser);

            // 1.2) Repositórios já existentes
            Func<string, FalhaEnergia> parseFalha = linha =>
            {
                var partes = linha.Split(';');
                int id = int.Parse(partes[0]);
                DateTime dt = DateTime.ParseExact(partes[1], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                string sensor = partes[2];
                decimal tensao = decimal.Parse(partes[3], CultureInfo.InvariantCulture);
                return new FalhaEnergia(id, dt, sensor, tensao);
            };
            Func<FalhaEnergia, string> toCsvFalha = f => f.ToCsv();
            var repoFalhas = new ArquivoRepositorio<FalhaEnergia>(pathFalhas, parseFalha, toCsvFalha);

            Func<string, Alerta> parseAlerta = linha =>
            {
                var p = linha.Split(';');
                int id = int.Parse(p[0]);
                // Nota: não usamos o DateTime gerado pelo construtor; apenas criamos o objeto
                NivelAlerta nivel = (NivelAlerta)Enum.Parse(typeof(NivelAlerta), p[2]);
                string msg = p[3];
                return new Alerta(id, nivel, msg);
            };
            Func<Alerta, string> toCsvAlerta = a => a.ToCsv();
            var repoAlertas = new ArquivoRepositorio<Alerta>(pathAlertas, parseAlerta, toCsvAlerta);

            Func<string, LogEvento> parseLog = linha =>
            {
                var partes = linha.Split('|');
                var timestamp = DateTime.ParseExact(partes[0].Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                TipoEvento tipo = (TipoEvento)Enum.Parse(typeof(TipoEvento), partes[1].Trim());
                string desc = partes[2].Trim();
                // Criamos o LogEvento “à mão” para preservar o timestamp original
                var log = (LogEvento)Activator.CreateInstance(typeof(LogEvento), new object[] { tipo, desc });
                var propTimestamp = typeof(LogEvento).GetProperty("Timestamp");
                propTimestamp.SetValue(log, timestamp);
                return log;
            };
            Func<LogEvento, string> toCsvLog = l => l.ToString();
            var repoLogs = new ArquivoRepositorio<LogEvento>(pathLogs, parseLog, toCsvLog);

            // ----------------------------------------------------
            // 2) INJEÇÃO DE DEPENDÊNCIAS (SERVIÇOS)
            // ----------------------------------------------------
            var logService = new LogService(repoLogs);
            var userService = new UserService(repoUsers, logService);
            var alertaService = new AlertaService(repoAlertas, logService);
            var falhaService = new FalhaService(repoFalhas, alertaService, logService);
            var simulador = new SimuladorErros(logService);

            // ----------------------------------------------------
            // 3) CRIAR USUÁRIO PADRÃO SE A LISTA ESTIVER VAZIA
            // ----------------------------------------------------
            if (userService.IsEmpty())
            {
                // Você pode escolher outro ID ou senha padrão
                userService.CreateUser(1, "admin", "admin123");
                Console.WriteLine("** Usuário padrão criado: admin / admin123 **");
            }

            // ----------------------------------------------------
            // 4) LOOP DE LOGIN
            // ----------------------------------------------------
            bool autenticado = false;
            int tentativas = 0;
            const int MAX_TENTATIVAS = 3;

            while (!autenticado && tentativas < MAX_TENTATIVAS)
            {
                Console.Clear();
                Console.WriteLine("=== Sistema de Login ===");
                Console.Write("Username: ");
                string inputUser = Console.ReadLine();
                Console.Write("Password: ");
                // Para não mostrar a senha em texto claro no console, poderíamos usar uma função mais avançada,
                // mas, para simplificar, usamos ReadLine(). Em produção, use Console.ReadKey com máscara ou similar.
                string inputPass = Console.ReadLine();

                var user = userService.Authenticate(inputUser, inputPass);
                if (user != null)
                {
                    autenticado = true;
                    Console.WriteLine($"Bem-vindo, {user.Username}!");
                    Thread.Sleep(1000);
                }
                else
                {
                    tentativas++;
                    Console.WriteLine("Credenciais inválidas. Tente novamente.");
                    Console.WriteLine($"Tentativas restantes: {MAX_TENTATIVAS - tentativas}");
                    Thread.Sleep(1500);
                }
            }

            if (!autenticado)
            {
                Console.Clear();
                Console.WriteLine("Número máximo de tentativas excedido. Encerrando aplicação.");
                logService.RegistrarEvento(
                    TipoEvento.ErroValidacao,
                    "Usuário excedeu tentativas de login"
                );
                return; // Sai da aplicação
            }

            // ----------------------------------------------------
            // 5) APÓS LOGIN BEM-SUCEDIDO, EXIBE MENU PRINCIPAL
            // ----------------------------------------------------
            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                MenuPrincipal.Exibir();
                var opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        // Registrar Falha
                        Console.Clear();
                        Console.WriteLine("=== Registrar Falha de Energia ===");
                        MenuFalha.CapturarDadosFalha(out var data, out var hora, out var sensor, out var tensao);
                        falhaService.RegistrarFalha(data, hora, sensor, tensao);
                        Console.WriteLine("Pressione Enter para continuar...");
                        Console.ReadLine();
                        break;

                    case "2":
                        // Gerar Relatório
                        Console.Clear();
                        Console.WriteLine("=== Gerar Relatório de Falhas ===");
                        MenuRelatorio.CapturarPeriodos(out var dIni, out var hIni, out var dFim, out var hFim, out var caminho);
                        try
                        {
                            DateTime inicio = DateTime.ParseExact($"{dIni} {hIni}", "dd/MM/yyyy HH:mm", null);
                            DateTime fim = DateTime.ParseExact($"{dFim} {hFim}", "dd/MM/yyyy HH:mm", null);
                            falhaService.GerarRelatorioFalhas(caminho, inicio, fim);
                        }
                        catch (FormatException fe)
                        {
                            Console.WriteLine($"Data/Hora inválida: {fe.Message}");
                            logService.RegistrarEvento(TipoEvento.ErroValidacao,
                                $"Erro ao converter data/hora para relatório: {fe.Message}");
                        }
                        Console.WriteLine("Pressione Enter para continuar...");
                        Console.ReadLine();
                        break;

                    case "3":
                        // Simular Erro
                        bool voltaSim = false;
                        while (!voltaSim)
                        {
                            Console.Clear();
                            MenuSimulacao.ExibirOpcoes();
                            string opSim = Console.ReadLine();
                            switch (opSim)
                            {
                                case "1":
                                    Console.Write("Digite data inválida (ex: 99/99/9999): ");
                                    var dInv = Console.ReadLine();
                                    simulador.ForcarErroData(dInv);
                                    Console.WriteLine("Pressione Enter para continuar...");
                                    Console.ReadLine();
                                    break;
                                case "2":
                                    Console.Write("Digite texto vazio (simule apertando Enter): ");
                                    var txtVazio = Console.ReadLine();
                                    simulador.ForcarErroCampoVazio(txtVazio);
                                    Console.WriteLine("Pressione Enter para continuar...");
                                    Console.ReadLine();
                                    break;
                                case "3":
                                    Console.Write("Digite número inválido (ex: abc): ");
                                    var numInv = Console.ReadLine();
                                    simulador.ForcarErroNumero(numInv);
                                    Console.WriteLine("Pressione Enter para continuar...");
                                    Console.ReadLine();
                                    break;
                                case "4":
                                    voltaSim = true;
                                    break;
                                default:
                                    Console.WriteLine("Opção inválida.");
                                    Console.ReadLine();
                                    break;
                            }
                        }
                        break;

                    case "4":
                        sair = true;
                        break;

                    default:
                        Console.WriteLine("Opção inválida, tente novamente.");
                        Console.ReadLine();
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("Saindo do sistema. Até logo!");
        }
    }
}
