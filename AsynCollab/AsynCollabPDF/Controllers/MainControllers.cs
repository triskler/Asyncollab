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
            _view = new MainView();
            _model = new Document();

            _modelLog = new ModelLog();
            _model.ModelLog = _modelLog;

            // Subscreve aos eventos da view e do model
            //Abrir um ficheiro
            _view.OnClickAbrirFicheiro += UtilizadorClicouEmAbrirFicheiro;
            _model.FicheiroDisponivel += _view.OnFicheiroDisponivel;
            _view.SolicitarPagina += _model.EnviarPagina;
            _model.PaginaEnviada += _view.OnPaginaEnviada;
            //Mudar de página
            _view.OnClickMudarPagina += UtilizadorAlterouPagina;
            _model.PaginaAlterada += _view.OnPaginaDisponivel;
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
                    "Erro ao carregar o ficheiro. Verifique se o caminho está correto ou se o ficheiro não está corrompido.",
                    "Erro ao Abrir Ficheiro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Chamado quando o utilizador quer mudar de página
        public void UtilizadorAlterouPagina(int indexPagina)
        {
            try
            {
                _model.ObterPagina(indexPagina);
            }
            catch (FicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao aceder ao ficheiro aberto. Verifique se o ficheiro não está corrompido.",
                    "Erro ao aceder a ficheiro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (PaginaInvalidaException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao carregar a nova página. Verifique se o ficheiro não está corrompido.",
                    "Erro ao mudar a página",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

    }
}
