using System;
using System.Drawing;
using System.Linq.Expressions;
using AsynCollabPDF.Models;
using AsynCollabPDF.Views;

/// <summary>
/// Controller 
/// 
/// Faz a liga��o dos eventos da view com os m�todos do model e vice-versa. Tamb�m gere as excep��es lan�adas
/// pelos outros componentes, com m�todos pr�prios que as recolhem.
/// </summary>

namespace AsynCollabPDF.Controllers
{
    public class MainController
    {
        private MainView _view;
        private Document _model;
        private string _caminhoSegundoFicheiro;
        private string _caminhoPrimeiroFicheiro;
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
            //Mudar de p�gina
            _view.OnClickMudarPagina += UtilizadorAlterouPagina;
            _model.PaginaAlterada += _view.OnPaginaDisponivel;
            //ErrorLog
            _view.SolicitarErrorLog += _modelLog.SolicitarLog;
            _modelLog.OnLogAlterado += _view.OnLogAlterado;
            //Abrir segundo ficheiro
            _view.OnClickAbrirSegundoFicheiro += UtilizadorClicouEmAbrirSegundoFicheiro;
            //Concatenar PDFs
            _view.OnClickConcatenarPDFs += UtilizadorClicouEmConcatenarPDFs;
            //_model.FicheirosConcatenados += _view.OnFicheiroDisponivel; // para mostrar o novo PDF

        }

        // M�todo inicial do programa, chamado pelo m�todo main
        public void IniciarPrograma()
        {
            try
            {
                _view.DesenharUI();
            } 
            catch (TipoFicheiroInvalidoException ex)
            {
                _view.AtivarViewLog();
                _model.RegistarLog(ex.Message);
            }
            catch (FicheiroInvalidoException ex)
            {
                _view.AtivarViewLog();
                _model.RegistarLog(ex.Message);
            }
            catch (PaginaInvalidaException ex)
            {
                _view.AtivarViewLog();
                _model.RegistarLog(ex.Message);
            }
            
        }

        // Chamado quando o utilizador seleciona um ficheiro
        public void UtilizadorClicouEmAbrirFicheiro(string localizacaoFicheiro)
        {
            _caminhoPrimeiroFicheiro = localizacaoFicheiro;

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
                _model.ObterPagina(indexPagina);
            }
            catch (FicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao aceder ao ficheiro aberto. Verifique se o ficheiro n�o est� corrompido.",
                    "Erro ao aceder a ficheiro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

        // Chamado quando o utilizador seleciona um segundo ficheiro para concatenar
        public void UtilizadorClicouEmAbrirSegundoFicheiro(string localizacaoFicheiro)
        {

            if (!File.Exists(localizacaoFicheiro))
            {
                MessageBox.Show("O ficheiro especificado n�o existe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _caminhoSegundoFicheiro = localizacaoFicheiro;

            try
            {
                // Aqui voc� pode armazenar o caminho temporariamente, ou preparar para concatena��o
                // Por exemplo, guardar numa vari�vel ou j� pedir concatena��o se for o caso.
                // Para j�, apenas tenta abrir o segundo ficheiro para teste:

                bool carregado = _model.AbrirFicheiro(localizacaoFicheiro);
            }
            catch (FicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao carregar o segundo ficheiro. Verifique se o caminho est� correto ou se o ficheiro n�o est� corrompido.",
                    "Erro ao Abrir Segundo Ficheiro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Chamado quando o utilizador clica no bot�o para concatenar. Verifica se os caminhos existem e define o caminho 
        // do ficheiro de destino, chamando depois o m�todo de concatena��o do model
        public void UtilizadorClicouEmConcatenarPDFs(string _, string __)
        {
            _model.RegistarLog("Este erro � um teste do log de erros.");
            try
            {
                if (string.IsNullOrEmpty(_caminhoSegundoFicheiro))
                {
                    MessageBox.Show("O segundo ficheiro n�o foi selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string caminho1 = _caminhoPrimeiroFicheiro; // Voc� precisar� expor isso via uma propriedade p�blica.
                string caminho2 = _caminhoSegundoFicheiro;

                string caminhoDestino = Path.Combine(
                    Path.GetDirectoryName(caminho1),
                    "Concatenado_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf"
                );

                bool sucesso = _model.ConcatenarFicheiros(caminho1, caminhoDestino);

                if (sucesso)
                {
                    MessageBox.Show("Ficheiros concatenados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show("Erro ao concatenar ficheiros. Verifique se os ficheiros est�o v�lidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
