using System.Collections.Generic;

namespace FI.AtividadeEntrevista.Interfaces
{
    public interface IBllCRUDBasico<T> where T : class
    {
        long Incluir(T objeto);
        void Alterar(T objeto);
        T Consultar(long id);
        void Excluir(long id);
        List<T> Listar();
        List<T> Pesquisa(
            int iniciarEm,
            int quantidade,
            string campoOrdenacao,
            bool crescente,
            out int qtd,
            Dictionary<string, string> parametrosAdicionais = null);
    }
}
