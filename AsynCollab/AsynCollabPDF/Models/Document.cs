using System;
using System.Drawing;
using System.IO;
using PdfiumViewer;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfDocumentSharp = PdfSharp.Pdf.PdfDocument;
using PdfDocumentPdfium = PdfiumViewer.PdfDocument;

namespace AsynCollabPDF.Models
{
    /// <summary>
    /// Classe que representa um documento PDF.
    /// Usa PdfSharp para leitura e PdfiumViewer para renderização.
    /// </summary>
    public class Document : IDisposable
    {
        private PdfDocumentSharp documentoSharp;
        private PdfDocumentPdfium documentoPdfium;
        private string caminhoAtual;

        // Evento que notifica quando um ficheiro é carregado com sucesso
        public event EventHandler FicheiroDisponivel;

        // Evento que notifica quando uma nova página é solicitada
        public event EventHandler<int> PaginaAlterada;

        public Document()
        {
            documentoSharp = null;
            documentoPdfium = null;
            caminhoAtual = string.Empty;
        }

        /// <summary>
        /// Indica se há um documento carregado.
        /// </summary>
        public bool DocumentoCarregado => documentoSharp != null && documentoPdfium != null;

        /// <summary>
        /// Retorna o número total de páginas do documento carregado.
        /// </summary>
        public int NumeroPaginas => documentoPdfium?.PageCount ?? 0;

        /// <summary>
        /// Abre um ficheiro PDF utilizando PdfSharp e PdfiumViewer.
        /// </summary>
        /// <param name="localizacaoFicheiro">Caminho completo para o ficheiro PDF</param>
        /// <returns>True se o ficheiro for carregado com sucesso, false caso contrário</returns>
        public bool AbrirFicheiro(string localizacaoFicheiro)
        {
            if (!File.Exists(localizacaoFicheiro))
                return false;

            try
            {
                FecharDocumento();

                documentoSharp = PdfReader.Open(localizacaoFicheiro, PdfDocumentOpenMode.ReadOnly);
                documentoPdfium = PdfDocumentPdfium.Load(localizacaoFicheiro);
                caminhoAtual = localizacaoFicheiro;

                FicheiroDisponivel?.Invoke(this, EventArgs.Empty);
                return true;
            }
            
            catch (PdfReaderException ex)
            {
                // log or handle PDFsharp-specific issue
                return false;
            }
            catch (IOException ex)
            {
                // handle file access issues
                return false;
            }
            catch (Exception ex)
            {
                // log unknown exceptions
                return false;
            }
        }

        /// <summary>
        /// Renderiza a primeira página do documento.
        /// </summary>
        /// <param name="pagina">Imagem onde o conteúdo será colocado</param>
        /// <returns>True se renderizado com sucesso</returns>
        public bool SolicitarFicheiro(ref Image pagina)
        {
            return SolicitarPagina(0, ref pagina);
        }

        /// <summary>
        /// Dispara evento para notificar que uma página foi pedida.
        /// </summary>
        /// <param name="index">Índice da página</param>
        /// <returns>True se válida e evento disparado</returns>
        public bool ObterPagina(int index)
        {
            if (!DocumentoCarregado || index < 0 || index >= NumeroPaginas)
                return false;

            PaginaAlterada?.Invoke(this, index);
            return true;
        }

        /// <summary>
        /// Renderiza uma página como imagem.
        /// </summary>
        /// <param name="index">Índice da página (começa em 0)</param>
        /// <param name="pagina">Imagem onde o conteúdo será desenhado</param>
        /// <returns>True se a imagem for gerada com sucesso</returns>
        public bool SolicitarPagina(int index, ref Image pagina)
        {
            if (!DocumentoCarregado || index < 0 || index >= NumeroPaginas)
                return false;

            try
            {
                pagina = documentoPdfium.Render(index, 800, 1000, true);
                return true;
            }
            catch
            {
                pagina = null;
                return false;
            }
        }

        /// <summary>
        /// Fecha documentos e liberta recursos.
        /// </summary>
        public void FecharDocumento()
        {
            documentoSharp?.Close();
            documentoPdfium?.Dispose();

            documentoSharp = null;
            documentoPdfium = null;
            caminhoAtual = string.Empty;
        }

        public void Dispose()
        {
            FecharDocumento();
        }
    }
}