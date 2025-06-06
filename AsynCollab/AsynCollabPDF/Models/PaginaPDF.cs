using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AsynCollabPDF.Interfaces;

/// <summary>
/// Representa uma página PDF, que contém o index da página e o ficheiro PDF associado (com a interface IFicheiroPDF, para ser
/// possivel converter a página num stream)
/// </summary>

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
