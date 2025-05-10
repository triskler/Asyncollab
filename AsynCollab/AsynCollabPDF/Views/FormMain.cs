using System;
using System.IO;
using System.Windows.Forms;
using static AsynCollabPDF.Interfaces;

namespace AsynCollabPDF.Views
{
    public partial class FormMain : Form
    {
        private MainView view;
        //private PdfiumViewer.PdfDocument pdfDocument; // Documento PDF
        //private PdfiumViewer.PdfDocument segundoPdfDocument; // Segundo documento PDF
        private int currentPage = 0; // Página atual
        private string primeiroPdfPath; // Caminho do primeiro PDF
        private string segundoPdfPath; // Caminho do segundo PDF
        private PictureBox pictureBox; // PictureBox para visualização do PDF
        private Label lblPaginaAtual; // Label para mostrar a página atual

        public FormMain()
        {
            InitializeComponent();
            
            // Configura o formulário
            this.Text = "AsynCollab PDF";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(600, 400);

            // Cria um TableLayoutPanel para organizar os controles
            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };

            // Configura as proporções das linhas
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Painel de botões
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Painel de navegação
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Área de visualização

            // Cria um TableLayoutPanel para os botões principais
            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(5)
            };

            // Configura as colunas do painel de botões
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            // Configura os botões com estilo consistente
            Button btnCarregarPDF = new Button
            {
                Text = "Carregar Primeiro PDF",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCarregarPDF.Click += btnCarregarPDF_Click;

            Button btnCarregarSegundoPDF = new Button
            {
                Text = "Carregar Segundo PDF",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCarregarSegundoPDF.Click += btnCarregarSegundoPDF_Click;

            Button btnConcatenarPDFs = new Button
            {
                Text = "Concatenar PDFs",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnConcatenarPDFs.Click += btnConcatenarPDFs_Click;

            // Adiciona os botões ao painel
            buttonPanel.Controls.Add(btnCarregarPDF, 0, 0);
            buttonPanel.Controls.Add(btnCarregarSegundoPDF, 1, 0);
            buttonPanel.Controls.Add(btnConcatenarPDFs, 2, 0);

            // Cria um TableLayoutPanel para os controles de navegação
            TableLayoutPanel navigationPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(5)
            };

            // Configura as colunas do painel de navegação
            navigationPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            navigationPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            navigationPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            // Adiciona botões de navegação
            Button btnAnterior = new Button
            {
                Text = "Página Anterior",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAnterior.Click += (s, e) => MudarPagina(-1);

            Button btnProxima = new Button
            {
                Text = "Próxima Página",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = System.Drawing.Color.FromArgb(0, 120, 215),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnProxima.Click += (s, e) => MudarPagina(1);

            // Adiciona label para mostrar a página atual
            lblPaginaAtual = new Label
            {
                Text = "Página: 0",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            navigationPanel.Controls.Add(btnAnterior, 0, 0);
            navigationPanel.Controls.Add(lblPaginaAtual, 1, 0);
            navigationPanel.Controls.Add(btnProxima, 2, 0);

            // Adiciona um PictureBox para visualização do PDF
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Adiciona os painéis ao TableLayoutPanel principal
            mainTable.Controls.Add(buttonPanel, 0, 0);
            mainTable.Controls.Add(navigationPanel, 0, 1);
            mainTable.Controls.Add(pictureBox, 0, 2);

            // Adiciona o TableLayoutPanel principal ao formulário
            this.Controls.Add(mainTable);
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
                        //Se o ficheio n�o for PDF, lan�a uma excep��o pr�pria criada no ficheiro TipoFicheiroInvalidoException.cs
                        throw new TipoFicheiroInvalidoException(caminhoArquivo, "*.pdf");

                        //controller.FicheiroInvalido?.Invoke("Erro: O arquivo selecionado n�o � um PDF. Por favor, selecione um arquivo PDF.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    primeiroPdfPath = caminhoArquivo;
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
        /*public void RenderizarFicheiro(string ficheiro)
        {
            // Carrega o documento PDF
            pdfDocument = PdfiumViewer.PdfDocument.Load(ficheiro);
            currentPage = 0;
            lblPaginaAtual.Text = $"Página: {currentPage + 1}";

            // Renderiza a primeira página
            RenderizarPagina(currentPage);
        }*/

        private void MudarPagina(int direcao)
        {
            //if (pdfDocument == null) return;

            // Lógica para mudar a página
            currentPage += direcao;

            // Limita a página atual
            if (currentPage < 0) currentPage = 0;
            //if (currentPage >= pdfDocument.PageCount) currentPage = pdfDocument.PageCount - 1;

            lblPaginaAtual.Text = $"Página: {currentPage + 1}";

            // Renderiza a nova página no PictureBox
            view.UtilizadorClicouEmMudarPagina(currentPage);
        }

        private void FicheiroInvalidoHandler(string mensagem, string titulo, MessageBoxButtons botoes, MessageBoxIcon icone)
        {
            MessageBox.Show(mensagem, titulo, botoes, icone);
        }

        public void RenderizarPagina(IPagina pagina)
        {
            using var stream = pagina.Ficheiro.ConverterImagemParaStream(pagina.IndexPaginaAtual, pictureBox.Height, pictureBox.Width);
            pictureBox.Image = Image.FromStream(stream);

            /* Antiga função implementada pelo Rafael (tentei aproveitar a lógica)
             * // Cria um novo formulário para exibir a imagem recebida (1 página)
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
            }*/
        }

        public void EncerrarPrograma()
        {
            Application.Exit();
        }

        public void btnCarregarSegundoPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Selecione o segundo arquivo PDF";
                openFileDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string caminhoArquivo = openFileDialog.FileName;
                    string extensao = Path.GetExtension(caminhoArquivo).ToLower();

                    if (extensao != ".pdf")
                    {
                        throw new TipoFicheiroInvalidoException(caminhoArquivo, "*.pdf");
                    }

                    segundoPdfPath = caminhoArquivo;
                    view.UtilizadorClicouEmAbrirSegundoFicheiro(caminhoArquivo);
                }
            }
        }

        private void btnConcatenarPDFs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(primeiroPdfPath) || string.IsNullOrEmpty(segundoPdfPath))
            {
                MessageBox.Show("Por favor, carregue ambos os arquivos PDF antes de concatenar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            view.UtilizadorClicouEmConcatenarPDFs(primeiroPdfPath, segundoPdfPath);
        }
    }
}