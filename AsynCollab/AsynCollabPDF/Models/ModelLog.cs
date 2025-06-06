using System;
using static AsynCollabPDF.Interfaces;

namespace AsynCollabPDF.Models
{
	/// <summary>
	/// Classe para fazer log de erros capturados na execução da aplicação
	/// Pertence ao componente model que mantém e altera o log
	/// </summary>
	public class ModelLog
	{
		//Evento e delegado para alertar que o estado do log foi alterado
		public delegate void LogAlteradoHandler();
		public event LogAlteradoHandler OnLogAlterado;

		private List<ILogItem> errorLog;
		private int errorCount;

        public ModelLog()
		{
			errorCount = 0;
            errorLog = new List<ILogItem>();
        }

		public void LogErro(string msg)
		{
            LogItem item = new LogItem();
			item.ID = errorCount++;
			item.TimeStamp = DateTime.Now;
            item.Message = msg;
			OnLogAlterado?.Invoke();
		}

		public List<ILogItem> SolicitarLog()
		{
			return errorLog;
		}
	}
}