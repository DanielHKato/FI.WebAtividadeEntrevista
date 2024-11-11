using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Interfaces
{
    public interface IDalCRUDBasico<T> where T : class
    {
        long Incluir(T modelo);
        T Consultar(long Id);
        List<T> Pesquisa(
            int iniciarEm, 
            int quantidade, 
            string campoOrdenacao, 
            bool crescente, 
            out int qtd, 
            Dictionary<string, string> parametrosAdicionais = null);
        List<T> Listar();
        void Alterar(T modelo);
        void Excluir(long Id);
    }
}
