using System;
using System.Drawing;
using AsynCollabPDF.Models;
using AsynCollabPDF.Views;

namespace AsynCollabPDF.Controllers
{
    public class MainController
    {
        private MainView _view;
        private Document _model;

        public MainController()
        {
            _model = new Document();

            // Subscreve aos eventos do model
            _model.FicheiroDisponivel += OnFicheiroDisponivel;
            _model.PaginaAlterada += OnPaginaAlterada;
        }

        public void IniciarPrograma()
        {
            _view = new MainView(this); // a view recebe o controller no construtor
            Application.Run(_view);
        }

        // Chamado quando o utilizador seleciona um ficheiro
        public void UtilizadorClicouEmAbrirFicheiro(object sender, string localizacaoFicheiro)
        {
            bool carregado = _model.AbrirFicheiro(localizacaoFicheiro);
            if (!carregado)
            {
                // aqui poderias mostrar uma mensagem de erro na view
            }
        }

        // Chamado quando o utilizador quer mudar de página
        public void UtilizadorAlterouPagina(object sender, int indexPagina)
        {
            _model.ObterPagina(indexPagina);
        }

        // Quando o modelo avisa que o ficheiro está carregado
        private void OnFicheiroDisponivel(object sender, EventArgs e)
        {
            Image primeiraPagina = null;
            bool sucesso = _model.SolicitarFicheiro(ref primeiraPagina);

            if (sucesso)
                _view.RenderizarPagina(primeiraPagina);
        }

        // Quando o modelo avisa que a página foi alterada
        private void OnPaginaAlterada(object sender, int novaPagina)
        {
            Image pagina = null;
            bool sucesso = _model.SolicitarPagina(novaPagina, ref pagina);

            if (sucesso)
                _view.RenderizarPagina(pagina);
        }

    }
}
