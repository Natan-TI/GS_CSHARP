using System.Collections.Generic;

namespace GS_CSHARP.Repositories
{
    public interface IRepositorio<T>
    {
        void Adicionar(T entidade);
        List<T> ObterTodos();
        T ObterPorId(int id);
    }
}
