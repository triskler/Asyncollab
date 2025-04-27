using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF.Models
{
    class PaginaInvalidaException : FileNotFoundException
    {
        public new string Message;
        public PaginaInvalidaException(int index)
        {
            this.Message = string.Format("Não foi possível obter a página nº'{0}'. Certifique-se de que a página existe. Erro ao abrir página.", index);
        }
    }
}
