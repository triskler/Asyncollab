using System;
using System.Drawing;
using System.IO;
using PdfiumViewer;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfDocumentSharp = PdfSharp.Pdf.PdfDocument;
using static AsynCollabPDF.Interfaces;
using System.CodeDom;

namespace AsynCollabPDF.Models
{
    /// <summary>
    /// Classe que representa um documento PDF.
    /// Usa PdfSharp para leitura e PdfiumViewer para renderização.
    /// </summary>
    public class Document : IDisposable
    {
        ModelLog modelLog;
        private PdfDocumentSharp documentoSharp;
        private FicheiroPdfium documentoPdfium;
        private string caminhoAtual;

        // Evento que notifica quando um ficheiro é carregado com sucesso e quando é enviado por referência
        public event EventHandler FicheiroDisponivel;
        public event EventHandler FicheiroEnviado;

        // Evento que notifica quando uma nova página é solicitada
        public event EventHandler<int> PaginaAlterada;
        public event EventHandler PaginaEnviada;

        public delegate void FicheirosConcatenadosHandler(string caminhoFicheiro);
        public event FicheirosConcatenadosHandler FicheirosConcatenados;

        public Document()
        {
            documentoSharp = null;
            documentoPdfium = null;
            caminhoAtual = string.Empty;
        }

        public ModelLog ModelLog { get => modelLog; set => modelLog = value; }

        /// <summary>
        /// Indica se há um documento carregado.
        /// </summary>
        public bool DocumentoPdfiumCarregado => documentoPdfium != null;

        /// <summary>
        /// Retorna o número total de páginas do documento carregado.
        /// </summary>
        public int NumeroPaginas => documentoPdfium.NumeroPaginas;

        /// <summary>
        /// Abre um ficheiro PDF utilizando PdfSharp e PdfiumViewer.
        /// </summary>
        /// <param name="localizacaoFicheiro">Caminho completo para o ficheiro PDF</param>
        /// <returns>True se o ficheiro for carregado com sucesso, false caso contrário</returns>
        public bool AbrirFicheiro(string localizacaoFicheiro)
        {
            if (!File.Exists(localizacaoFicheiro))
            {
                throw new FicheiroInvalidoException(localizacaoFicheiro);
                //return false;
            }
            try
            {
                FecharDocumento();

                documentoPdfium = new FicheiroPdfium(localizacaoFicheiro);
                caminhoAtual = localizacaoFicheiro;

                FicheiroDisponivel?.Invoke(this, EventArgs.Empty);
                return true;
            }
            
            catch (PdfReaderException ex)
            {
                RegistarLog(ex.Message);
                return false;
            }
            catch (IOException ex)
            {
                RegistarLog(ex.Message);
                // handle file access issues
                return false;
            }
            catch (Exception ex)
            {
                RegistarLog(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Renderiza a primeira página do documento.
        /// </summary>
        /// <param name="pagina">Imagem onde o conteúdo será colocado</param>
        /// <returns>True se renderizado com sucesso</returns>
        /*public void EnviarFicheiro(ref Image pagina)
        {
            //Em vez de bool, deve lançar excepções
            //EnviarPagina(0, ref pagina);
            pagina = documentoPdfium.Render(0, 800, 1000, true);
            FicheiroEnviado?.Invoke(this, EventArgs.Empty);
        }*/

        /// <summary>
        /// Dispara evento para notificar que uma página foi pedida.
        /// </summary>
        /// <param name="index">Índice da página</param>
        /// <returns>True se válida e evento disparado</returns>
        public void ObterPagina(int index)
        {
            if (!DocumentoPdfiumCarregado)
                throw new FicheiroInvalidoException(caminhoAtual);
            if (index < 0 || index >= NumeroPaginas)
                throw new PaginaInvalidaException(index);

            PaginaAlterada?.Invoke(this, index);
        }

        /// <summary>
        /// Renderiza uma página como imagem.
        /// </summary>
        /// <param name="index">Índice da página (começa em 0)</param>
        /// <param name="pagina">Imagem onde o conteúdo será desenhado</param>
        /// <returns>True se a imagem for gerada com sucesso</returns>
        public void EnviarPagina(int index, ref IPagina pagina)
        {
            if (!DocumentoPdfiumCarregado)
                throw new FicheiroInvalidoException(caminhoAtual);
            if (index < 0 || index >= NumeroPaginas)
                throw new PaginaInvalidaException(index);

            // Coloca um novo objeto "PaginaPDF" na variável "pagina" passada por referência
            // Damos o index pretendido e o tipo de documento ao novo objeto (a view vai tratar da renderização)
            pagina = new PaginaPDF(index, documentoPdfium);
            PaginaEnviada?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Concatenar dois ficheiros PDF com a API PDFSharp.
        ///  - Caminho do ficheiro aberto é a variável "caminhoAtual", o método só recebe o segundo caminho.
        /// 
        /// Fecha qualquer documento aberto, realiza a concatenação e guarda o ficheiro concatenado no caminho indicado.
        /// Abre logo o ficheiro concatenado e comunica à view de que o ficheiro está disponível.
        /// </summary>
        public bool ConcatenarFicheiros(string caminho2, string caminhoDestino)
        {
            string caminho1 = caminhoAtual;
            //Fecha o documento aberto
            FecharDocumento();
            PdfDocumentSharp ficheiroDestino = new PdfDocumentSharp();

            // Método auxiliar para adicionar as páginas de um ficheiro ao documento de destino
            void JuntarPaginas(string caminho)
            {
                PdfDocumentSharp ficheiro = PdfReader.Open(caminho, PdfDocumentOpenMode.Import);
                for (int i = 0; i < ficheiro.PageCount; i++)
                {
                    ficheiroDestino.AddPage(ficheiro.Pages[i]);
                }
            }

            JuntarPaginas(caminho1);
            JuntarPaginas(caminho2);

            // Guarda o ficheiro concatenado
            ficheiroDestino.Save(caminhoDestino);

            try
            {
                FecharDocumento();

                documentoPdfium = new FicheiroPdfium(caminhoDestino);
                caminhoAtual = caminhoDestino;

                // Abre o ficheiro concatenado e alerta a view com um evento
                FicheirosConcatenados?.Invoke(caminhoDestino);
                return true;
            }
            catch
            {
                throw new FicheiroInvalidoException(caminhoDestino);
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

        public void RegistarLog(string msg)
        {
            ModelLog.LogErro(msg);
        }
    }
}