using System;

namespace AsynCollabPDF.Models
{
	/// <summary>
	/// Classe para fazer log de erros capturados na execu��o da aplica��o
	/// Pertence ao componente model que mant�m e altera o log
	/// </summary>
	internal class ModelLog
	{
		//Evento e delegado para alertar que o estado do log foi alterado
		public delegate void LogAlteradoHandler();
		public event LogAlteradoHandler OnLogAlterado;

		private int errorCount;
		private string errorLog;
		
		public ModelLog()
		{
			errorCount = 0;
			errorLog = "";
		}

		public void LogErro(string msg)
		{
			errorCount++;
			errorLog += DateTime.Now + "Erro n� " + errorCount + ": " + msg + System.Environment.NewLine;
			OnLogAlterado?.Invoke();
		}

		public string SolicitarLog()
		{
			return errorLog;
		}
	}
}