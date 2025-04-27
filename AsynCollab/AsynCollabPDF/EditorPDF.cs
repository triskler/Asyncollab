using System;
using AsynCollabPDF.Views;
using System.Windows.Forms;
using AsynCollabPDF.Controllers;


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
            MainController controller = new MainController();
            controller.IniciarPrograma();
        }
    }
}