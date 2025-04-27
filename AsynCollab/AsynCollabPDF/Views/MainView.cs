using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AsynCollabPDF.Models;

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
        private Document model;
        private FormMain janela;
        private ViewLog viewLog;

        //Variável passada por referência ao model, para renderizar uma página
        //O model abre a página e envia-a para a view, a view só envia o index da página
        public Image paginaAberta;

        public delegate void FicheiroAbertoHandler(string nomeFicheiro);
        public event FicheiroAbertoHandler OnClickAbrirFicheiro;
        public delegate void SolicitacaoFicheiroHandler(ref Image paginaAberta);
        public event SolicitacaoFicheiroHandler SolicitarFicheiro;

        public delegate void PaginaAlteradaHandler(int indexPagina);
        public event PaginaAlteradaHandler OnClickMudarPagina;
        public delegate void SolicitacaoPaginaHandler(int indexPagina, ref Image paginaAberta);
        public event SolicitacaoPaginaHandler SolicitarPagina;

        public delegate string SolicitacaoErrorLogHandler();
        public event SolicitacaoErrorLogHandler SolicitarErrorLog;

        public MainView(Document m)
        {
            model = m;
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
            SolicitarFicheiro?.Invoke(ref paginaAberta);
        }

        public void OnFicheiroEnviado(object sender, EventArgs e)
        {
            //Devemos chamar sempre o RenderizarPagina e apagamos o RenderizarFicheiro
            //O model é que tem o trabalho de abrir a página e enviar
            if (paginaAberta != null)
            {
                janela.RenderizarPagina(paginaAberta);
            }
            else
            {
                MessageBox.Show("A imagem não foi carregada corretamente!");
            }
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
                MessageBox.Show("A imagem não foi carregada corretamente!");
            }
        }
    }
}
