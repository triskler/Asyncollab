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
            //vou deixar caso precisem de testar a view enquanto n�o houver classe controller 
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cria uma inst�ncia do Controller
            Controllers.MainController controller = new Controllers.MainController();
            // Passa o Controller para a MainView
            MainView mainView = new MainView(controller);
            // Inicia a aplica��o com a MainView
            Application.Run(mainView);



            /* Quando tivermos a classe controller, � esta que vai chamar a view para iniciar a abertura da primeira janela
            Controller controller = new Controller();
            controller.IniciarPrograma();
            */
        }
    }
}