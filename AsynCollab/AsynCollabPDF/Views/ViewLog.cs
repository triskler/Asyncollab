using System;
using System.Windows.Forms;

namespace AsynCollabPDF.Views
{
    /// <summary>
    /// Classe respons�vel por exibir o log de erros quando este � alterado
    /// </summary>
    public class ViewLog
    {
        FormLog janelaLog;
        Form pai;

        public ViewLog(Form origem)
        {
            pai = origem;
        }

        internal void AtivarViewLog()
        {
            if (!janelaLog)
            {
                janelaLog = new FormLog();
            }
        }

        internal void LogUpdate(string log)
        {
            if (janelaLog != null)
            {
                janelaLog.EscreverLog(log);
                janelaLog.ShowDialog(pai);
            }
        }
    }
}