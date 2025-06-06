using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AsynCollabPDF.Interfaces;

/// <summary>
/// View 
/// 
/// Mantém a comunicação por eventos com o controller e o model. 
/// A implementação dos métodos anteriores mantém-se no ficheiro "FormMain", que era o antigo ficheiro "MainView".
/// </summary>

namespace AsynCollabPDF.Views
{
    public class MainView
    {
        private FormMain janela;
        private ViewLog viewLog;

        //Variável passada por referência ao model, para renderizar uma página
        //O model abre a página e envia-a para a view, a view só envia o index da página
        public IPagina paginaAberta;

        public delegate void FicheiroAbertoHandler(string nomeFicheiro);
        public event FicheiroAbertoHandler OnClickAbrirFicheiro;

        public delegate void SolicitacaoPaginaHandler(int indexPagina, ref IPagina paginaAberta);
        public event SolicitacaoPaginaHandler SolicitarPagina;

        public delegate void PaginaAlteradaHandler(int indexPagina);
        public event PaginaAlteradaHandler OnClickMudarPagina;

        public delegate List<ILogItem> SolicitacaoErrorLogHandler();
        public event SolicitacaoErrorLogHandler SolicitarErrorLog;

        // Novos eventos para o segundo PDF e concatenação
        public delegate void SegundoFicheiroAbertoHandler(string nomeFicheiro);
        public event SegundoFicheiroAbertoHandler OnClickAbrirSegundoFicheiro;

        public delegate void ConcatenarPDFsHandler(string primeiroPdf, string segundoPdf);
        public event ConcatenarPDFsHandler OnClickConcatenarPDFs;

        // Construtor
        public MainView()
        {
            viewLog = new ViewLog(janela);
            paginaAberta = null;
        }

        // Chama os métodos de inicialização da UI
        public void DesenharUI()
        {
            janela = new FormMain();
            janela.View = this;
            janela.ShowDialog();
        }

        public void EncerrarPrograma()
        {
            janela.EncerrarPrograma();
        }

        // Método para inicializar a janela de log de erros
        public void AtivarViewLog()
        {
            viewLog.AtivarViewLog();
        }

        // Método para atualizar o conteúdo na janela de log de erros
        public void OnLogAlterado()
        {
            string logString = "";
            List<ILogItem> listaErros = SolicitarErrorLog(); // Lança evento e chama método que retorna o error log
            foreach (ILogItem erro in listaErros)
                logString += erro.TimeStamp + " Erro ID " + erro.ID + ": " + erro.Message + Environment.NewLine;

            viewLog.LogUpdate(logString);
        }

        // Evento que pede ao model para abrir um ficheiro PDF (aguarda resposta do model para saber se o ficheiro existe e pode ser aberto)
        public void UtilizadorClicouEmAbrirFicheiro(string caminho)
        {
            OnClickAbrirFicheiro?.Invoke(caminho);
        }

        // Evento que envia a referência da página a abrir quando o model confirma que o ficheiro existe e pode ser aberto
        public void OnFicheiroDisponivel()
        {
            // Como é a primeira vez que o ficheiro é aberto, abrimos a primeira página (index = 0)
            SolicitarPagina?.Invoke(0, ref paginaAberta);
        }

        // Evento que pede ao model para abrir uma página PDF (aguarda resposta do model para saber se a página existe e pode ser aberta)
        public void UtilizadorClicouEmMudarPagina(int indexPagina)
        {
            OnClickMudarPagina?.Invoke(indexPagina);
        }

        // Evento que envia a referência da página a abrir quando o model confirma que a página existe e pode ser renderizada
        public void OnPaginaDisponivel(int index)
        {
            SolicitarPagina?.Invoke(index, ref paginaAberta);
        }

        // Método que renderiza a página enviada por referência ao model, que já foi aberta e está pronta para ser exibida
        public void OnPaginaEnviada()
        {
            if (paginaAberta != null)
            {
                janela.RenderizarPagina(paginaAberta);
            }
            else
            {
                MessageBox.Show("A página não foi carregada corretamente!");
            }
        }

        // Evento que pede ao model para abrir um segundo ficheiro PDF (aguarda resposta do model para saber se o ficheiro existe e pode ser aberto)
        public void UtilizadorClicouEmAbrirSegundoFicheiro(string caminho)
        {
            OnClickAbrirSegundoFicheiro?.Invoke(caminho);
        }

        // Evento que envia os nomes dos ficheiros PDF a serem concatenados, e pede ao model para realizar a concatenação
        public void UtilizadorClicouEmConcatenarPDFs(string primeiroPdf, string segundoPdf)
        {
            OnClickConcatenarPDFs?.Invoke(primeiroPdf, segundoPdf);
        }
    }
}
