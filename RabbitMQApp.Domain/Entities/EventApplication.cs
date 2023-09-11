namespace RabbitMQApp.Domain.Entities
{
    public class EventApplication
    {
        public EventApplication(string fileName, DateTime? dateTime)
        {
            FileName = fileName;
            DateTime = dateTime;
        }

        public string FileName { get; }
        public DateTime? DateTime { get; }
    }
}
