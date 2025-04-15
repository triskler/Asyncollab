using System;
using AsynCollabPDF.Views;


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
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainView());
            Application.EnableVisualStyles();
            
            

            /* Quando tivermos a classe controller, é esta que vai chamar a view para iniciar a abertura da primeira janela
            Controller controller = new Controller();
            controller.IniciarPrograma();
            */
        }
    }
}