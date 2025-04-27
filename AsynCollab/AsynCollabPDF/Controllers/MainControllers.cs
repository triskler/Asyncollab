using System;
using System.Drawing;
using System.Linq.Expressions;
using AsynCollabPDF.Models;
using AsynCollabPDF.Views;

namespace AsynCollabPDF.Controllers
{
    public class MainController
    {
        private MainView _view;
        private Document _model;
        ModelLog _modelLog;

        public MainController()
        {
            _view = new MainView(_model);
            _model = new Document(_view);

            _modelLog = new ModelLog();
            _model.ModelLog = _modelLog;

            // Subscreve aos eventos da view e do model
            //Abrir um ficheiro
            _view.OnClickAbrirFicheiro += UtilizadorClicouEmAbrirFicheiro;
            _model.FicheiroDisponivel += _view.OnFicheiroDisponivel;
            _view.SolicitarFicheiro += _model.EnviarFicheiro;
            _model.FicheiroEnviado += _view.OnFicheiroEnviado;
            //Mudar de p�gina
            _view.OnClickMudarPagina += UtilizadorAlterouPagina;
            _model.PaginaAlterada += _view.OnPaginaDisponivel;
            _view.SolicitarPagina += _model.EnviarPagina;
            _model.PaginaEnviada += _view.OnPaginaEnviada;
            //ErrorLog
            _view.SolicitarErrorLog += _modelLog.SolicitarLog;
            _modelLog.OnLogAlterado += _view.OnLogAlterado;
        }

        public void IniciarPrograma()
        {
            try
            {
                _view.DesenharUI();
            } 
            catch (TipoFicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
            }
            
        }

        // Chamado quando o utilizador seleciona um ficheiro
        public void UtilizadorClicouEmAbrirFicheiro(string localizacaoFicheiro)
        {
            try
            {
                bool carregado = _model.AbrirFicheiro(localizacaoFicheiro);
            }
            catch (FicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao carregar o ficheiro. Verifique se o caminho est� correto ou se o ficheiro n�o est� corrompido.",
                    "Erro ao Abrir Ficheiro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Chamado quando o utilizador quer mudar de p�gina
        public void UtilizadorAlterouPagina(int indexPagina)
        {
            try
            {
                bool carregado = _model.ObterPagina(indexPagina);
            }
            catch (PaginaInvalidaException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao carregar a nova p�gina. Verifique se o ficheiro n�o est� corrompido.",
                    "Erro ao mudar a p�gina",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /*A subscri��o � view tornou este m�todo desnecess�rio AM
        // Quando o modelo avisa que o ficheiro est� carregado
        private void OnFicheiroDisponivel(object sender, EventArgs e)
        {
            Image primeiraPagina = null;
            bool sucesso = _model.SolicitarFicheiro(ref primeiraPagina);

            if (sucesso)
                _view.RenderizarPagina(primeiraPagina);
        }
        */

        /*A subscri��o � view tornou este m�todo desnecess�rio AM
        // Quando o modelo avisa que a p�gina foi alterada
        private void OnPaginaAlterada(object sender, int novaPagina)
        {
            Image pagina = null;
            bool sucesso = _model.SolicitarPagina(novaPagina, ref pagina);

            if (sucesso)
                _view.RenderizarPagina(pagina);
        }
        */

    }
}
