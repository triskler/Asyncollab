using System;
using AsynCollabPDF.Views;
using System.Windows.Forms;


namespace AsynCollabPDF
{
    class EditorPDF
    {
        /// <summary>
        /// O programa começa aqui, mas este ficheiro só chama o método do controller IniciarPrograma(),
        /// o controller comunica com a view para criar a UI
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Usa o método que já tens preparado no controller
            Controllers.MainController controller = new Controllers.MainController();
            controller.IniciarPrograma();
        }
    }
}