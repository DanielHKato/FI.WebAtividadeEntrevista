using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Interfaces
{
    public interface IDalVerificarExistencia
    {
        bool VerificarExistencia(Dictionary<string, string> parametrosPesquisa);
    }
}
