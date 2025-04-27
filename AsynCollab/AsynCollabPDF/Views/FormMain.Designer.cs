
namespace AsynCollabPDF.Views
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCarregarPDF = new Button();
            btnAnterior = new Button();
            btnProxima = new Button();
            lblPaginaAtual = new Label();
            SuspendLayout();
            // 
            // btnCarregarPDF
            // 
            btnCarregarPDF.BackColor = Color.Silver;
            btnCarregarPDF.Cursor = Cursors.Hand;
            btnCarregarPDF.Location = new Point(242, 328);
            btnCarregarPDF.Name = "btnCarregarPDF";
            btnCarregarPDF.Size = new Size(310, 30);
            btnCarregarPDF.TabIndex = 0;
            btnCarregarPDF.Text = "Carregar PDF";
            btnCarregarPDF.UseVisualStyleBackColor = false;
            btnCarregarPDF.Click += btnCarregarPDF_Click;
            // 
            // btnAnterior
            // 
            btnAnterior.BackColor = Color.Silver;
            btnAnterior.Location = new Point(242, 292);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(150, 30);
            btnAnterior.TabIndex = 1;
            btnAnterior.Text = "Página Anterior";
            btnAnterior.UseVisualStyleBackColor = false;
            // 
            // btnProxima
            // 
            btnProxima.BackColor = Color.Silver;
            btnProxima.Location = new Point(402, 292);
            btnProxima.Name = "btnProxima";
            btnProxima.Size = new Size(150, 30);
            btnProxima.TabIndex = 2;
            btnProxima.Text = "Próxima Página";
            btnProxima.UseVisualStyleBackColor = false;
            // 
            // lblPaginaAtual
            // 
            lblPaginaAtual.AutoSize = true;
            lblPaginaAtual.Location = new Point(369, 404);
            lblPaginaAtual.Name = "lblPaginaAtual";
            lblPaginaAtual.Size = new Size(52, 21);
            lblPaginaAtual.TabIndex = 3;
            lblPaginaAtual.Text = "label1";
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(224, 224, 224);
            ClientSize = new Size(800, 450);
            Controls.Add(lblPaginaAtual);
            Controls.Add(btnProxima);
            Controls.Add(btnAnterior);
            Controls.Add(btnCarregarPDF);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "MainView";
            Text = "AsynCollabPDF";
            Load += FormMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCarregarPDF;
        private Button btnAnterior;
        private Button btnProxima;
        private Label lblPaginaAtual;
    }
}