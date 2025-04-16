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
            //vou deixar caso precisem de testar a view enquanto não houver classe controller 
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cria uma instância do Controller
            Controllers.MainController controller = new Controllers.MainController();
            // Passa o Controller para a MainView
            MainView mainView = new MainView(controller);
            // Inicia a aplicação com a MainView
            Application.Run(mainView);



            /* Quando tivermos a classe controller, é esta que vai chamar a view para iniciar a abertura da primeira janela
            Controller controller = new Controller();
            controller.IniciarPrograma();
            */
        }
    }
}