using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF
{
    public class Interfaces
    {
        // Interface para abstrair uma página PDF, permite que os componentes interajam com ela sem depender de implementações específicas
        public interface IPagina
        {
            int IndexPaginaAtual { get; }
            IFicheiroPDF Ficheiro { get; }
        }

        // Interface para abstrair a API utilizada para renderizar um ficheiro (se quisermos mudar do pdfiumViewer),
        // utilizada nas classes que operam sobre ficheiros, também inclui um "getter" para o número de páginas 
        public interface IFicheiroPDF
        {
            Stream ConverterPaginaParaStream(int indexPagina, int altura, int largura);

            int NumeroPaginas { get; }
        }

        public interface ILogItem
        {
            int ID { get; }
            DateTime TimeStamp { get; }
            string Message { get; }
        }
    }
}
