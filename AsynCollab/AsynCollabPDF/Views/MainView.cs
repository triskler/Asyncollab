using System;
using System.IO;
using System.Windows.Forms;
using PdfiumViewer; 

namespace AsynCollabPDF.Views
{
    public partial class MainView : Form
    {
        

        public MainView()
        {
            InitializeComponent();

            
            btnCarregarPDF.Text = "Carregar PDF"; 
            btnCarregarPDF.Click += new EventHandler(btnCarregarPDF_Click); // Associa o evento Click ao método
            this.Load += new EventHandler(MainView_Load);
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            // Lógica de inicialização, se necessário
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
                        MessageBox.Show("Erro: O arquivo selecionado não é um PDF. Por favor, selecione um arquivo PDF.", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Interrompe a execução se o arquivo não for PDF
                    }

                    MessageBox.Show($"Arquivo selecionado: {caminhoArquivo}");

                    // Lógica para abrir e exibir o PDF
                    using (var pdfViewerForm = new Form())
                    {
                        var pdfViewer = new PdfViewer();
                        pdfViewer.Dock = DockStyle.Fill;
                        pdfViewerForm.Controls.Add(pdfViewer);
                        pdfViewerForm.Text = "Visualizador de PDF";
                        pdfViewerForm.WindowState = FormWindowState.Maximized;
                        pdfViewerForm.ShowDialog();
                    }
                }
            }
        }
    }
}