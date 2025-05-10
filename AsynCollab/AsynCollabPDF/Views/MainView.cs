using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AsynCollabPDF.Interfaces;

/// <summary>
/// Class View que mantém a comunicação por eventos com o controller e o model. Tive de alterar alguns métodos
/// e mudar outros de sitio, mas assim já funciona de acordo com o esperado na UC. 
/// A implementação dos métodos anteriores mantém-se no ficheiro "FormMain", que era o antigo ficheiro "MainView".
/// 
/// Qualquer dúvida pergutem-me. AM
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

        public delegate string SolicitacaoErrorLogHandler();
        public event SolicitacaoErrorLogHandler SolicitarErrorLog;

        // Novos eventos para o segundo PDF e concatenação
        public delegate void SegundoFicheiroAbertoHandler(string nomeFicheiro);
        public event SegundoFicheiroAbertoHandler OnClickAbrirSegundoFicheiro;

        public delegate void ConcatenarPDFsHandler(string primeiroPdf, string segundoPdf);
        public event ConcatenarPDFsHandler OnClickConcatenarPDFs;

        public MainView()
        {
            viewLog = new ViewLog(janela);
            paginaAberta = null;
        }

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

        public void AtivarViewLog()
        {
            viewLog.AtivarViewLog();
        }

        public void OnLogAlterado()
        {
            viewLog.LogUpdate(SolicitarErrorLog());
        }

        public void UtilizadorClicouEmAbrirFicheiro(string caminho)
        {
            OnClickAbrirFicheiro?.Invoke(caminho);
        }

        public void OnFicheiroDisponivel(object sender, EventArgs e)
        {
            // Como é a primeira vez que o ficheiro é aberto, abrimos a primeira página (index = 0)
            SolicitarPagina?.Invoke(0, ref paginaAberta);
        }

        public void UtilizadorClicouEmMudarPagina(int indexPagina)
        {
            OnClickMudarPagina?.Invoke(indexPagina);
        }

        public void OnPaginaDisponivel(object sender, int index)
        {
            SolicitarPagina?.Invoke(index, ref paginaAberta);
        }

        public void OnPaginaEnviada(object sender, EventArgs e)
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

        public void UtilizadorClicouEmAbrirSegundoFicheiro(string caminho)
        {
            OnClickAbrirSegundoFicheiro?.Invoke(caminho);
        }

        public void UtilizadorClicouEmConcatenarPDFs(string primeiroPdf, string segundoPdf)
        {
            OnClickConcatenarPDFs?.Invoke(primeiroPdf, segundoPdf);
        }
    }
}
