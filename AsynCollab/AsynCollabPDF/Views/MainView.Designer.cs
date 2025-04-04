namespace AsynCollabPDF.Views
{
    partial class MainView
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
            SuspendLayout();
            // 
            // btnCarregarPDF
            // 
            btnCarregarPDF.Location = new Point(297, 336);
            btnCarregarPDF.Name = "btnCarregarPDF";
            btnCarregarPDF.Size = new Size(177, 31);
            btnCarregarPDF.TabIndex = 0;
            btnCarregarPDF.Text = "Carregar PDF";
            btnCarregarPDF.UseVisualStyleBackColor = true;
            btnCarregarPDF.Click += btnCarregarPDF_Click;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCarregarPDF);
            Name = "MainView";
            Text = "Form1";
            Load += MainView_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnCarregarPDF;
    }
}
