using System;
using AsynCollabPDF.Views;


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
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainView());
            Application.EnableVisualStyles();
            
            

            /* Quando tivermos a classe controller, � esta que vai chamar a view para iniciar a abertura da primeira janela
            Controller controller = new Controller();
            controller.IniciarPrograma();
            */
        }
    }
}