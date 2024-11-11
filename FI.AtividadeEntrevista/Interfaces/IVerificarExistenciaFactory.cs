using FI.AtividadeEntrevista.Enums;

namespace FI.AtividadeEntrevista.Interfaces
{
    public interface IVerificarExistenciaFactory
    {
        IBllVerificarExistencia ObterVerificarExistencia(EnumTipoVerificacaoExistencia tipoVerificacaoExistencia = EnumTipoVerificacaoExistencia.Clientes);
    }
}
