using System;

namespace GS_CSHARP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User(int id, string username, string password)
        {
            Id = id;
            DefinirUsername(username);
            DefinirPassword(password);
        }

        private void DefinirUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username não pode ser vazio.");
            Username = username.Trim();
        }

        private void DefinirPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password não pode ser vazia.");
            // Atenção: aqui estamos salvando em texto puro. Em um sistema real, convém
            // armazenar um hash em vez da senha em texto claro.
            Password = password.Trim();
        }

        // Gera linha CSV para o usuário, no formato: Id;Username;Password
        public string ToCsv()
        {
            return $"{Id};{Username};{Password}";
        }
    }
}
