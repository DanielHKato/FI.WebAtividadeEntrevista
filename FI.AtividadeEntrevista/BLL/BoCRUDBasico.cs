using FI.AtividadeEntrevista.Interfaces;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    /// <summary>
    /// Implementa operações básicas de CRUD definidas para o projeto (Classe genérica).
    /// </summary>
    /// <typeparam name="T">Classe modelo que será a referência para camada DAL</typeparam>
    public class BoCRUDBasico<T> : IBllCRUDBasico<T> where T : class
    {
        protected IDalCRUDBasico<T> _dao;

        /// <summary>
        /// Envia os dados para serem atualizados na base de dados
        /// </summary>
        /// <param name="objeto">Objeto contendo os dados para serem enviados para a base de dados</param>
        public virtual void Alterar(T objeto)
        {
            _dao.Alterar(objeto);
        }

        /// <summary>
        /// Retorna um modelo contendo os registros recuperados do banco de dados, que possuem o ID fornecido
        /// </summary>
        /// <param name="id">ID do registro a ser recuperado</param>
        /// <returns></returns>
        public virtual T Consultar(long id)
        {
            return _dao.Consultar(id);
        }

        /// <summary>
        /// Exclui o registro identificado pelo ID, na base de dados
        /// </summary>
        /// <param name="id">ID do registro a ser removido</param>
        public virtual void Excluir(long id)
        {
            _dao.Excluir(id);
        }

        /// <summary>
        /// Insere os dados do modelo na base de dados
        /// </summary>
        /// <param name="objeto">Objeto contendo os dados que serão inseridos na base de dados</param>
        /// <returns>ID do registro inserido</returns>
        public virtual long Incluir(T objeto)
        {
            return _dao.Incluir(objeto);
        }

        /// <summary>
        /// Retorna uma lista de dados da entidade na base de dados
        /// </summary>
        /// <returns>Lista de dados obtidos</returns>
        public virtual List<T> Listar()
        {
            return _dao.Listar();
        }

        /// <summary>
        /// Retorna os dados da pesquisa na base de dados. Método utilizado para o componente JTable.
        /// </summary>
        /// <param name="iniciarEm">Primeiro registro que deve ser retornado.</param>
        /// <param name="quantidade">Quantos registros devem ser retornados por página.</param>
        /// <param name="campoOrdenacao">Nome da coluna de ordenação.</param>
        /// <param name="crescente">Ordenação crescente ou decrescente.</param>
        /// <param name="qtd">Quantos registros existem na base, no total.</param>
        /// <param name="parametrosAdicionais">Dicionário contendo parâmetros opcionais de pesquisa.</param>
        /// <returns>Lista de dados (que definem o conteúdo de uma página do JTable)</returns>
        public virtual List<T> Pesquisa(
            int iniciarEm, 
            int quantidade, 
            string campoOrdenacao, 
            bool crescente, 
            out int qtd,
            Dictionary<string, string> parametrosAdicionais = null)
        {
            return _dao.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd, parametrosAdicionais);
        }
    }
}
