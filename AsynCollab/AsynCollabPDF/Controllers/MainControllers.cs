using System;
using System.Drawing;
using System.Linq.Expressions;
using AsynCollabPDF.Models;
using AsynCollabPDF.Views;

/// <summary>
/// Controller 
/// 
/// Faz a ligação dos eventos da view com os métodos do model e vice-versa. Também gere as excepções lançadas
/// pelos outros componentes, com métodos próprios que as recolhem.
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
            //Mudar de página
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

        // Método inicial do programa, chamado pelo método main
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

        // Chamado quando o utilizador seleciona um segundo ficheiro para concatenar
        public void UtilizadorClicouEmAbrirSegundoFicheiro(string localizacaoFicheiro)
        {

            if (!File.Exists(localizacaoFicheiro))
            {
                MessageBox.Show("O ficheiro especificado não existe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _caminhoSegundoFicheiro = localizacaoFicheiro;

            try
            {
                // Aqui você pode armazenar o caminho temporariamente, ou preparar para concatenação
                // Por exemplo, guardar numa variável ou já pedir concatenação se for o caso.
                // Para já, apenas tenta abrir o segundo ficheiro para teste:

                bool carregado = _model.AbrirFicheiro(localizacaoFicheiro);
            }
            catch (FicheiroInvalidoException ex)
            {
                _model.RegistarLog(ex.Message);
                MessageBox.Show(
                    "Erro ao carregar o segundo ficheiro. Verifique se o caminho está correto ou se o ficheiro não está corrompido.",
                    "Erro ao Abrir Segundo Ficheiro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Chamado quando o utilizador clica no botão para concatenar. Verifica se os caminhos existem e define o caminho 
        // do ficheiro de destino, chamando depois o método de concatenação do model
        public void UtilizadorClicouEmConcatenarPDFs(string _, string __)
        {
            _model.RegistarLog("Este erro é um teste do log de erros.");
            try
            {
                if (string.IsNullOrEmpty(_caminhoSegundoFicheiro))
                {
                    MessageBox.Show("O segundo ficheiro não foi selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string caminho1 = _caminhoPrimeiroFicheiro; // Você precisará expor isso via uma propriedade pública.
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
                MessageBox.Show("Erro ao concatenar ficheiros. Verifique se os ficheiros estão válidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
