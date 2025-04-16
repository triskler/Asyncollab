using System;
using System.IO;
using System.Windows.Forms;
using PdfiumViewer;

namespace AsynCollabPDF.Views
{
    public partial class MainView : Form
    {
        private Controllers.MainController controller;
        private PdfiumViewer.PdfDocument pdfDocument; // Documento PDF
        private int currentPage = 0; // P�gina atual

        public MainView(Controllers.MainController controller)
        {
            InitializeComponent();
            this.controller = controller;
            //this.controller.FicheiroInvalido += FicheiroInvalidoHandler; // Inscreve-se no evento de erro
            //this.controller.FicheiroDisponivel += SolicitarFicheiro; // Inscreve-se no evento FicheiroDisponivel
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            
        }

        public void btnCarregarPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Selecione um arquivo PDF";
                openFileDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string caminhoArquivo = openFileDialog.FileName;
                    string extensao = Path.GetExtension(caminhoArquivo).ToLower();

                    if (extensao != ".pdf")
                    {
                        //controller.FicheiroInvalido?.Invoke("Erro: O arquivo selecionado n�o � um PDF. Por favor, selecione um arquivo PDF.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Interrompe a execu��o se o arquivo n�o for PDF
                    }

                    MessageBox.Show($"Arquivo selecionado: {caminhoArquivo}");

                    //controller.ProcessarFicheiro(caminhoArquivo);
                }
            }
        }

        // M�todo que solicita o arquivo quando o evento FicheiroDisponivel � disparado
        public void SolicitarFicheiro(string nome_ficheiro)
        {
            MessageBox.Show($"O arquivo '{nome_ficheiro}' est� dispon�vel para ser solicitado.");
            //controller.SolicitarFicheiro(nome_ficheiro); // Chama o m�todo do Controller para solicitar o arquivo
        }

        // M�todo que renderiza o arquivo quando o evento EnviarFicheiro � disparado
        public void RenderizarFicheiro(string ficheiro)
        {
            MessageBox.Show($"Renderizando o arquivo: {ficheiro}");

            // Cria um novo formul�rio para o visualizador de PDF
            using (var pdfViewerForm = new Form())
            {
                pdfViewerForm.Text = "Visualizador de PDF";
                pdfViewerForm.WindowState = FormWindowState.Maximized;

                // Cria um PictureBox para exibir a p�gina do PDF
                PictureBox pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Fill;
                pdfViewerForm.Controls.Add(pictureBox);

                // Carrega o documento PDF
                pdfDocument = PdfiumViewer.PdfDocument.Load(ficheiro);

                // Renderiza a p�gina atual em uma imagem
                RenderizarPagina(currentPage, pictureBox);

                // Adiciona bot�es para mudar de p�gina
                Button btnAnterior = new Button { Text = "P�gina Anterior", Dock = DockStyle.Top };
                Button btnProxima = new Button { Text = "Pr�xima P�gina", Dock = DockStyle.Top };
                Label lblPaginaAtual = new Label { Text = $"P�gina: {currentPage + 1}", Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

                btnAnterior.Click += (s, e) => MudarPagina(-1, lblPaginaAtual, pictureBox);
                btnProxima.Click += (s, e) => MudarPagina(1, lblPaginaAtual, pictureBox);

                pdfViewerForm.Controls.Add(btnAnterior);
                pdfViewerForm.Controls.Add(lblPaginaAtual);
                pdfViewerForm.Controls.Add(btnProxima);

                pdfViewerForm.ShowDialog();
            }
        }

        private void MudarPagina(int direcao, Label lblPaginaAtual, PictureBox pictureBox)
        {
            // L�gica para mudar a p�gina
            currentPage += direcao;

            // Limita a p�gina atual
            if (currentPage < 0) currentPage = 0;
            if (currentPage >= pdfDocument.PageCount) currentPage = pdfDocument.PageCount - 1;

            lblPaginaAtual.Text = $"P�gina: {currentPage + 1}";

            // Renderiza a nova p�gina no PictureBox
            RenderizarPagina(currentPage, pictureBox);
        }

        private void RenderizarPagina(int pagina, PictureBox pictureBox)
        {
            using (var image = pdfDocument.Render(pagina, 300, 300, PdfRenderFlags.Annotations))
            {
                pictureBox.Image = image; // Atualiza a imagem no PictureBox
            }
        }

        private void FicheiroInvalidoHandler(string mensagem, string titulo, MessageBoxButtons botoes, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, botoes, icone);
        }
    }
}