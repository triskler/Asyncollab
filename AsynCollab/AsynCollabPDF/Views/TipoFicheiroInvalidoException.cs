using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF.Views
{
    class TipoFicheiroInvalidoException : IOException
    {
        public new string Message;
        public TipoFicheiroInvalidoException(string caminho, string tipoValido)
        {
            this.Message = string.Format( "O ficheiro '{0}' não é um ficheiro PDF válido. Tipo de ficheiros aceite: '{1}'", caminho, tipoValido);
        }
    }
}
