using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AsynCollabPDF.Interfaces;

namespace AsynCollabPDF.Models
{
    class PaginaPDF: IPagina
    {
        public int IndexPaginaAtual { get; }
        private IFicheiroPDF ficheiro;
        public PaginaPDF(int indexPaginaAtual, IFicheiroPDF ficheiro)
        {
            IndexPaginaAtual = indexPaginaAtual;
            this.ficheiro = ficheiro;
        }

        public IFicheiroPDF Ficheiro => ficheiro;
    }
}
