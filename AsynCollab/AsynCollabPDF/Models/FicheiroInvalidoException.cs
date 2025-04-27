using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF.Models
{
    class FicheiroInvalidoException : FileNotFoundException
    {
        public new string Message;
        public FicheiroInvalidoException(string nomeFicheiro)
        {
            this.Message = string.Format("Não foi possível abrir o ficheiro '{0}'. Erro ao abrir ficheiro.", nomeFicheiro);
        }
    }
}
