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
        public int IndexPaginaAtual { get; set; }
        private IFicheiroPDF ficheiro;
        public PaginaPDF(int indexPaginaAtual, IFicheiroPDF ficheiro)
        {
            IndexPaginaAtual = indexPaginaAtual;
            this.ficheiro = ficheiro;
        }

        /*public void Renderizar(IRenderizador renderizador)
        {
            renderizador.RenderizarPagina(this);
        }*/

        public IFicheiroPDF Ficheiro => ficheiro;
    }
}
