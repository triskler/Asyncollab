using System;
using AsynCollabPDF.Views;
using System.Windows.Forms;


namespace AsynCollabPDF
{
    class EditorPDF
    {
        /// <summary>
        /// O programa come�a aqui, mas este ficheiro s� chama o m�todo do controller IniciarPrograma(),
        /// o controller comunica com a view para criar a UI
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Usa o m�todo que j� tens preparado no controller
            Controllers.MainController controller = new Controllers.MainController();
            controller.IniciarPrograma();
        }
    }
}