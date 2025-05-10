using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF
{
    public class Interfaces
    {
        public interface IPagina
        {
            int IndexPaginaAtual { get; }
            IFicheiroPDF Ficheiro { get; }
        }

        // Interface para abstrair a API utilizada para renderizar um ficheiro (se quisermos mudar do pdfiumViewer)
        public interface IFicheiroPDF
        {
            Stream ConverterPaginaParaStream(int indexPagina, int altura, int largura);

            int NumeroPaginas { get; }
        }
    }
}
