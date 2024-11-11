using System.Collections.Generic;

namespace FI.AtividadeEntrevista.Interfaces
{
    public interface IBllVerificarExistencia
    {
        bool VerificarExistencia(Dictionary<string, string> parametrosPesquisa);
    }
}
