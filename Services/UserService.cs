using GS_CSHARP.Models;
using GS_CSHARP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GS_CSHARP.Services
{
    public class UserService
    {
        private readonly IRepositorio<User> _repoUsers;
        private readonly LogService _logService;

        public UserService(IRepositorio<User> repoUsers, LogService logService)
        {
            _repoUsers = repoUsers;
            _logService = logService;
        }

        /// Retorna o objeto User se username + password baterem com algum registro.
        /// Caso contrário, retorna null.
        public User Authenticate(string username, string password)
        {
            try
            {
                var todos = _repoUsers.ObterTodos();
                // Busca usuário pelo username (case-insensitive) e compara a senha
                var user = todos.FirstOrDefault(u =>
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                    && u.Password == password);

                if (user != null)
                {
                    _logService.RegistrarEvento(
                        TipoEvento.GeraAlerta, // Ou outro TipoEvento que faça sentido, ex: “LoginSucesso”
                        $"Login bem-sucedido para usuário '{username}'"
                    );
                    return user;
                }
                else
                {
                    _logService.RegistrarEvento(
                        TipoEvento.ErroValidacao,
                        $"Falha de login: credenciais inválidas para '{username}'"
                    );
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logService.RegistrarEvento(
                    TipoEvento.ErroValidacao,
                    $"Erro durante autenticação de '{username}': {ex.Message}"
                );
                return null;
            }
        }

        /// Retorna true se não existir nenhum usuário; caso contrário, false.
        /// Serve para criarmos um “usuário padrão” caso o arquivo de usuários esteja vazio.
        public bool IsEmpty()
        {
            var todos = _repoUsers.ObterTodos();
            return todos.Count == 0;
        }

        /// Cria um usuário e grava no arquivo. Retorna o objeto criado.
        public User CreateUser(int id, string username, string password)
        {
            var novo = new User(id, username, password);
            _repoUsers.Adicionar(novo);
            _logService.RegistrarEvento(
                TipoEvento.GeraAlerta, // Ou outro tipo, ex: “NovoUsuario”
                $"Usuário criado: '{username}' (ID={id})"
            );
            return novo;
        }
    }
}
