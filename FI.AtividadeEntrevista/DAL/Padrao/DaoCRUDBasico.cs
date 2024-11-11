using FI.AtividadeEntrevista.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace FI.AtividadeEntrevista.DAL.Padrao
{
    /// <summary>
    /// Classe genérica que implementa uma classe básica (neste projeto) de operações CRUD na base de dados
    /// </summary>
    /// <typeparam name="T">Modelo de referência para os mapeamentos</typeparam>
    public class DaoCRUDBasico<T> : AcessoDados, IDalCRUDBasico<T> where T : class, new()
    {
        protected List<PropertyInfo> _propriedades;

        protected readonly string _nomeProcedureExcluir;
        protected readonly string _nomeProcedureAlterar;
        protected readonly string _nomeProcedureInserir;
        protected readonly string _nomeProcedureRecuperarPorId;
        protected readonly string _nomeProcedureListar;
        protected readonly string _nomeProcedurePesquisar;

        public DaoCRUDBasico(
            string nomeProcedureExcluir, 
            string nomeProcedureAlterar, 
            string nomeProcedureInserir, 
            string nomeProcedureRecuperarPorId, 
            string nomeProcedureListar, 
            string nomeProcedurePesquisar
        )
        {
            _propriedades = typeof(T).GetProperties().ToList();
            _nomeProcedureExcluir = nomeProcedureExcluir;
            _nomeProcedureAlterar = nomeProcedureAlterar;
            _nomeProcedureInserir = nomeProcedureInserir;
            _nomeProcedureRecuperarPorId = nomeProcedureRecuperarPorId;
            _nomeProcedureListar = nomeProcedureListar;
            _nomeProcedurePesquisar = nomeProcedurePesquisar;
        }

        #region CRUD

        /// <summary>
        /// Executa a operação de UPDATE no base de dados
        /// </summary>
        /// <param name="modelo">Objeto contendo os dados que serão enviados para atualização</param>
        public virtual void Alterar(T modelo)
        {
            base.Executar(this._nomeProcedureAlterar, CriarListaParametros(modelo));
        }

        /// <summary>
        /// Executa a operação de CONSULTA por Id na base de dados
        /// </summary>
        /// <param name="Id">ID do registro a ser recuperado</param>
        /// <returns>Instancia de T com os dados obtidos da base (ou uma nova instância não preenchida, em caso de erro)</returns>
        public virtual T Consultar(long Id)
        {
            return ConverterDataSetParaList(base.Consultar(this._nomeProcedureListar, CriarListaParametros(Id))).FirstOrDefault<T>();
        }

        /// <summary>
        /// Executa a operação de DELETE por Id na base de dados
        /// </summary>
        /// <param name="Id">ID do registro a ser removido</param>
        public virtual void Excluir(long Id)
        {
            base.Executar(
                _nomeProcedureExcluir, 
                CriarListaParametros(Id).ToList()
            );
        }

        /// <summary>
        /// Executa a operação de INSERIR na base de dados.
        /// </summary>
        /// <param name="modelo">objeto contendo os dados que serão inseridos</param>
        /// <returns>Id do novo registro</returns>
        public virtual long Incluir(T modelo)
        {
            return RecuperarUnicoValor(base.Consultar(this._nomeProcedureInserir, CriarListaParametros(modelo, true)));
        }

        /// <summary>
        /// Retorna os dados da entidade na base de dados.
        /// </summary>
        /// <returns>Lista de dados da entidade encontrados</returns>
        public virtual List<T> Listar()
        {
            return ConverterDataSetParaList(base.Consultar(this._nomeProcedureListar, CriarListaParametros(0)));
        }

        /// <summary>
        /// Executa uma operação de pesquisa de dados da entidade na base de dados. Método utilizado para o componente JTable.
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
            var parametros = CriarListaParametrosParaPesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, parametrosAdicionais);
            var ds = base.Consultar(this._nomeProcedurePesquisar, parametros);
            var registros = ConverterDataSetParaList(ds, 0);

            qtd = (int) RecuperarUnicoValor(ds, 1);

            return registros;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Tenta recuperar o valor da primeira célula (primeira linha, primeira coluna) de um DataSet e tentar converter o valor para long.
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="indiceTabela">Índice da tabela a ser acessada dentro do DataSet (valor padrão 0)</param>
        /// <returns>Valor da primeira célula (primeira linha, primeira coluna) convertido</returns>
        /// <remarks>
        /// Em caso de erro, ou DataSet vazio, o retorno é o número zero (mesma lógica aplicada na classe DaoCliente).
        /// </remarks>
        protected long RecuperarUnicoValor(DataSet ds, int indiceTabela = 0)
        {
            long ret = 0;
            var dataSetValido = DataSetTemDadosNaTabela(ds, indiceTabela);
            var valorRecuperado = (dataSetValido && 
                    TentarConverterParaLong(ds.Tables[indiceTabela].Rows[0][0].ToString(), out ret)) || 
                SetarValorZero(out ret);

            return ret;
        }

        /// <summary>
        /// Método para tentar converter uma string em long
        /// </summary>
        /// <param name="value">string a ser convertida</param>
        /// <param name="valorRecuperado">valor convertido</param>
        /// <returns>Sucesso na conversão (true) ou não (false)</returns>
        protected bool TentarConverterParaLong(string value, out long valorRecuperado)
        {
            valorRecuperado = 0;

            return long.TryParse(value, out valorRecuperado);
        }

        /// <summary>
        /// Define o valor zero na variável informada no parâmetro valor.
        /// </summary>
        /// <param name="valor">variável que vai receber o valor zero</param>
        /// <returns>Sempre true</returns>
        /// <remarks>Método otimizado para branchless</remarks>
        protected bool SetarValorZero(out long valor)
        {
            valor = 0;

            return true;
        }

        /// <summary>
        /// Converte um DataSet em uma Lista de objetos (do tipo da entidade definida em T).
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="indiceTabela">[OPCIONAL] Índice da tabela do DataSet a ser convertida. Valor padrão: zero</param>
        /// <returns>Lista de objetos (do tipo da entidade definida em T)</returns>
        protected List<T> ConverterDataSetParaList(DataSet ds, int indiceTabela = 0)
        {
            List<T> dadosConvertidos;
            var dataSetValido = DataSetTemDadosNaTabela(ds, indiceTabela);
            var dadosPreenchidosComSucesso = (dataSetValido && 
                    ConverterDataRowCollectionParaList(ds.Tables[indiceTabela].Rows, out dadosConvertidos)) ||
                InicializarListaVazia(out dadosConvertidos);

            return dadosConvertidos;
        }

        /// <summary>
        /// Inicializa uma lista vazia na variável informada no parâmetro lista.
        /// </summary>
        /// <param name="lista">variável que vai receber a nova lista</param>
        /// <returns>Sempre true</returns>
        /// <remarks>Método otimizado para branchless</remarks>
        protected bool InicializarListaVazia(out List<T> lista)
        {
            lista = new List<T>();

            return true;
        }

        /// <summary>
        /// Converte uma DataRowCollection (dados de uma DataTable) para uma Lista de objetos (Tipo T)
        /// </summary>
        /// <param name="linhas">DataRowCollection</param>
        /// <param name="dadosConvertidos">Lista resultante da conversão</param>
        /// <returns>Sempre True</returns>
        /// <remarks>Método otimizado para branchless</remarks>
        protected bool ConverterDataRowCollectionParaList(DataRowCollection linhas, out List<T> dadosConvertidos)
        {
            dadosConvertidos = new List<T>();

            foreach (DataRow linha in linhas)
            {
                dadosConvertidos.Add(ConverterLinha(linha));
            }

            return true;
        }

        /// <summary>
        /// Converte uma DataRow em um objeto do tipo T.
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>objeto do tipo T com os dados obtidos da DataRow</returns>
        protected T ConverterLinha(DataRow dr)
        {
            var objeto = new T();

            foreach (PropertyInfo propriedade in _propriedades)
            {
                propriedade.SetValue(objeto, dr[propriedade.Name]);
            }

            return objeto;
        }

        /// <summary>
        /// Converte todas as propriedades de T em uma lista de parâmetros para as procedures.
        /// </summary>
        /// <param name="modelo">Objeto que será a referência para a conversão</param>
        /// <param name="pularParametroId">[OPCIONAL] Indica se a propriedade Id não deve ser convertida em um parâmetro. Valor padrão false</param>
        /// <returns>Lista de parâmetros</returns>
        protected List<SqlParameter> CriarListaParametros(T modelo, bool pularParametroId = false)
        {
            var parametros = new List<SqlParameter>();
            var propriedadesParaPercorrer = pularParametroId ?
                _propriedades.Where(p => !p.Name.Trim().ToUpper().Equals("ID") ).ToList() :
                _propriedades;

            foreach (PropertyInfo propriedade in propriedadesParaPercorrer)
            {
                parametros.Add(new SqlParameter($"{propriedade.Name}", propriedade.GetValue(modelo)));
            }

            return parametros;
        }

        /// <summary>
        /// Retorna uma lista de SqlParameters, contendo um SqlParameter Id
        /// </summary>
        /// <param name="Id">Id a ser inserido na lista</param>
        /// <returns>Lista de parâmetros</returns>
        /// <remarks>O parâmetro é inserido como ID na lista. Verifique se as novas procedures recebem o campo @ID.</remarks>
        protected List<SqlParameter> CriarListaParametros(long Id)
        {
             return new List<SqlParameter>() {
                new SqlParameter("ID", Id)
            };
        }

        /// <summary>
        /// Retorna uma lista de SqlParameters personalizada para a procedure de pesquisa de dados para o JTable.
        /// </summary>
        /// <param name="iniciarEm">Primeiro registro que deve ser retornado.</param>
        /// <param name="quantidade">Quantos registros devem ser retornados por página.</param>
        /// <param name="campoOrdenacao">Nome da coluna de ordenação.</param>
        /// <param name="crescente">Ordenação crescente ou decrescente.</param>
        /// <param name="qtd">Quantos registros existem na base, no total.</param>
        /// <param name="parametrosAdicionais">Dicionário contendo parâmetros opcionais de pesquisa.</param>
        /// <returns>lista de SqlParameters</returns>
        protected List<SqlParameter> CriarListaParametrosParaPesquisa(
            int iniciarEm, 
            int quantidade, 
            string campoOrdenacao, 
            bool crescente,
            Dictionary<string, string> parametrosAdicionais = null)
        {
            var parametros = new List<SqlParameter>()
            {
                new SqlParameter("iniciarEm", iniciarEm),
                new SqlParameter("quantidade", quantidade),
                new SqlParameter("campoOrdenacao", campoOrdenacao),
                new SqlParameter("crescente", crescente)
            };

            parametros.AddRange(ConverterDictionaryParaListSqlParameter(parametrosAdicionais ?? new Dictionary<string, string>()));

            return parametros;
        }

        /// <summary>
        /// Converte um Dictionary[string, string] para uma lista de SqlParameters.
        /// </summary>
        /// <param name="dicionario">Dicionário a ser convertido</param>
        /// <returns>Lista de SqlParameters</returns>
        protected List<SqlParameter> ConverterDictionaryParaListSqlParameter(Dictionary<string, string> dicionario)
        {
            return dicionario
                .Select(itemDicionario => new SqlParameter(itemDicionario.Key, itemDicionario.Value))
                .ToList();
        }

        /// <summary>
        /// Verifica se o DataSet tem dados na tabela indicada no parâmetro [indiceTabela].
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="indiceTabela">[OPCIONAL] Índice da tabela do DataSet a ser verificada. Valor padrão zero.</param>
        /// <returns>Tem dados (true) ou não (false). Também retorna false se o DataSet for nulo ou não tiver tabelas.</returns>
        /// <remarks>O retorno pode ser false também caso o Índice da tabela seja maior do que a quantidade de tabelas presentes no DataSet</remarks>
        protected bool DataSetTemDadosNaTabela(DataSet ds, int indiceTabela = 0)
        {
            return ds != null &&
                   ds.Tables != null &&
                   ds.Tables.Count >= indiceTabela &&
                   ds.Tables[indiceTabela].Rows.Count > 0;
        }


        /// <summary>
        /// Método que remove parâmetros não utilizados de uma lista de SqlParameters
        /// </summary>
        /// <param name="nomeParametrosDesnecessarios">Lista contendo os nomes dos parâmetros que devem ser removidos</param>
        /// <param name="parameters">Lista de SqlParameters que será manipulada</param>
        /// <returns>Sempre true</returns>
        /// <remarks>Método otimizado para branchless</remarks>
        protected bool RemoverParametrosNaoNecessarios(List<string> nomeParametrosDesnecessarios, List<SqlParameter> parameters)
        {
            foreach (var nomeParametroDesnecessario in nomeParametrosDesnecessarios)
            {
                parameters = parameters
                    .Where(p => !p.ParameterName.Equals(nomeParametroDesnecessario))
                    .ToList();

                //Implementação alternativa, mas pode causar erro se o ParameterName não for encontrado
                //parameters.Remove(parameters.First(p => p.ParameterName.Equals(nomeParametroDesnecessario)));
            }

            return true;
        }

        #endregion
    }
}
