using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynCollabPDF
{
    /// <summary>
    /// Pode não ser necessário, usar a classe original da API PdfSharp a não ser que precisem de saber a página atual
    /// a classe PdfDocument tem os seguintes atributos que podem dar jeito para o projeto:
    ///     - .FullPath: caminho completo do ficheiro
    ///     - .PageCount: número de páginas do ficheiro
    ///     - .Version: versão do documento
    ///     - .FileSize: tamanho do ficheiro
    ///     - .Guid: id único do ficheiro
    ///     - .IsReadOnly: idica se o ficheiro é só de leitura
    /// </summary>
    class FicheiroPDF
    {
        PdfDocument Ficheiro { get; set; }
        public int IndexPaginaAberta { get; set; }
        public bool Modificado { get; set; }
        public DateTime UltimaModificacao { get; set; }
        public FicheiroPDF(PdfDocument doc, int indexPagina)
        {
            Ficheiro = doc;
            IndexPaginaAberta = indexPagina;
            Modificado = false;
            UltimaModificacao = DateTime.Now;
        }
    }
}
