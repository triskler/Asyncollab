using System;
using System.Drawing;
using System.IO;
using PdfiumViewer;

namespace AsynCollabPDF.Models
{
    /// <summary>
    /// Classe que representa um documento PDF.
    /// Responsável por carregar, disponibilizar e renderizar páginas do PDF como imagens.
    /// </summary>
    public class Document : IDisposable
    {
        private PdfiumViewer.PdfDocument documentoAtual;
        private string caminhoAtual;

        // Evento que notifica quando um ficheiro é carregado com sucesso
        public event EventHandler FicheiroDisponivel;

        // Evento que notifica quando uma nova página é solicitada
        public event EventHandler<int> PaginaAlterada;

        // Construtor - inicializa o estado interno do modelo
        public Document()
        {
            documentoAtual = null;
            caminhoAtual = string.Empty;
        }

        /// <summary>
        /// Indica se há um documento carregado.
        /// </summary>
        public bool DocumentoCarregado => documentoAtual != null;

        /// <summary>
        /// Retorna o número total de páginas do documento carregado.
        /// </summary>
        public int NumeroPaginas => documentoAtual?.PageCount ?? 0;

        /// <summary>
        /// Abre um ficheiro PDF a partir do caminho fornecido.
        /// Se já houver um documento carregado, este é libertado antes.
        /// </summary>
        /// <param name="localizacaoFicheiro">Caminho completo para o ficheiro PDF</param>
        /// <returns>True se o ficheiro for carregado com sucesso, false caso contrário</returns>
        public bool AbrirFicheiro(string localizacaoFicheiro)
        {
            if (!File.Exists(localizacaoFicheiro))
                return false;

            try
            {
                // Fecha o documento anterior, se existir
                FecharDocumento();

                // Abre o ficheiro PDF em modo de leitura
                using (var stream = File.OpenRead(localizacaoFicheiro))
                {
                    documentoAtual = PdfiumViewer.PdfDocument.Load(stream);
                }

                caminhoAtual = localizacaoFicheiro;

                // Dispara o evento para avisar que o ficheiro está disponível
                FicheiroDisponivel?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Solicita a primeira página do documento como imagem.
        /// Ideal para ser usada quando o PDF é carregado pela primeira vez.
        /// </summary>
        /// <param name="pagina">Imagem que vai receber o conteúdo da página</param>
        /// <returns>True se a página for renderizada com sucesso</returns>
        public bool SolicitarFicheiro(ref Image pagina)
        {
            return SolicitarPagina(0, ref pagina);
        }

        /// <summary>
        /// Verifica se a página existe e dispara o evento correspondente.
        /// Este método não devolve a imagem, apenas informa que uma nova página foi pedida.
        /// </summary>
        /// <param name="index">Índice da página (começa em 0)</param>
        /// <returns>True se a página existir e o evento for disparado</returns>
        public bool ObterPagina(int index)
        {
            if (!DocumentoCarregado || index < 0 || index >= NumeroPaginas)
                return false;

            PaginaAlterada?.Invoke(this, index);
            return true;
        }

        /// <summary>
        /// Renderiza a página com o índice fornecido e devolve-a como imagem.
        /// </summary>
        /// <param name="index">Índice da página (começa em 0)</param>
        /// <param name="pagina">Imagem onde o conteúdo será colocado</param>
        /// <returns>True se a imagem for gerada com sucesso</returns>
        public bool SolicitarPagina(int index, ref Image pagina)
        {
            if (!DocumentoCarregado || index < 0 || index >= NumeroPaginas)
                return false;

            try
            {
                // Renderiza a página com resolução 800x1000 e fundo branco
                pagina = documentoAtual.Render(index, 800, 1000, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Fecha o documento atual e liberta os recursos utilizados.
        /// </summary>
        public void FecharDocumento()
        {
            documentoAtual?.Dispose();
            documentoAtual = null;
            caminhoAtual = string.Empty;
        }

        /// <summary>
        /// Implementação do padrão IDisposable.
        /// Garante que os recursos são libertados quando o objeto é destruído.
        /// </summary>
        public void Dispose()
        {
            FecharDocumento();
        }
    }
}