using System;
using System.IO;
using System.Windows.Forms;
using PdfiumViewer;

namespace AsynCollabPDF.Views
{
    public partial class FormMain : Form
    {
        private MainView view;
        private PdfiumViewer.PdfDocument pdfDocument; // Documento PDF
        private int currentPage = 0; // Página atual

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e){}

        public MainView View { get => view; set => view = value; }

        public void btnCarregarPDF_Click(object sender, EventArgs e)
        {
            // A view trata de abrir o file browser e obter o nome / caminho do ficheiro
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
                        //Se o ficheio não for PDF, lança uma excepção própria criada no ficheiro TipoFicheiroInvalidoException.cs
                        throw new TipoFicheiroInvalidoException(caminhoArquivo, "*.pdf");

                        //controller.FicheiroInvalido?.Invoke("Erro: O arquivo selecionado não é um PDF. Por favor, selecione um arquivo PDF.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    view.UtilizadorClicouEmAbrirFicheiro(caminhoArquivo);
                }
            }
        }

        /* Método definido no novo ficheiro View.cs AM
         * Método que solicita o arquivo quando o evento FicheiroDisponivel é disparado
        public void SolicitarFicheiro(string nome_ficheiro)
        {
            MessageBox.Show($"O arquivo '{nome_ficheiro}' está disponível para ser solicitado.");
            //controller.SolicitarFicheiro(nome_ficheiro); // Chama o método do Controller para solicitar o arquivo
        }*/

        // Método que renderiza o arquivo quando o evento EnviarFicheiro é disparado
        public void RenderizarFicheiro(string ficheiro)
        {
            MessageBox.Show($"Renderizando o arquivo: {ficheiro}");

            // Cria um novo formulário para o visualizador de PDF
            using (var pdfViewerForm = new Form())
            {
                pdfViewerForm.Text = "Visualizador de PDF";
                pdfViewerForm.WindowState = FormWindowState.Maximized;

                // Cria um PictureBox para exibir a página do PDF
                PictureBox pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Fill;
                pdfViewerForm.Controls.Add(pictureBox);

                // Carrega o documento PDF
                pdfDocument = PdfiumViewer.PdfDocument.Load(ficheiro);

                // Renderiza a página atual em uma imagem
                RenderizarPagina(currentPage, pictureBox);

                // Adiciona botões para mudar de página
                Button btnAnterior = new Button { Text = "Página Anterior", Dock = DockStyle.Top };
                Button btnProxima = new Button { Text = "Próxima Página", Dock = DockStyle.Top };
                Label lblPaginaAtual = new Label { Text = $"Página: {currentPage + 1}", Dock = DockStyle.Top, TextAlign = System.Drawing.ContentAlignment.MiddleCenter };

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
            // Lógica para mudar a página
            currentPage += direcao;

            // Limita a página atual
            if (currentPage < 0) currentPage = 0;
            if (currentPage >= pdfDocument.PageCount) currentPage = pdfDocument.PageCount - 1;

            lblPaginaAtual.Text = $"Página: {currentPage + 1}";

            // Renderiza a nova página no PictureBox
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
        public void RenderizarPagina(System.Drawing.Image imagem)
        {
            // Cria um novo formulário para exibir a imagem recebida (1 página)
            using (var form = new Form())
            {
                form.Text = "Página PDF";
                form.WindowState = FormWindowState.Maximized;

                PictureBox pictureBox = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    Image = imagem,
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                form.Controls.Add(pictureBox);
                form.ShowDialog();
            }
        }

        public void EncerrarPrograma()
        {
            Application.Exit();
        }
    }
}