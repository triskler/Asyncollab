using static AsynCollabPDF.Interfaces;

/// <summary>
/// Representa um item individual do log de erros
/// </summary>

namespace AsynCollabPDF.Models
{
    class LogItem : ILogItem
    {
        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }

        public LogItem()
        {
            ID = 0;
            TimeStamp = DateTime.Now;
            Message = "";
        }
    }
}