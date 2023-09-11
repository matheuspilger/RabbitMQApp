namespace RabbitMQApp.Domain.Entities.Payloads
{
    public class MessagePayload
    {
        public MessagePayload(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }

        public static IEnumerable<MessagePayload> Build(IEnumerable<string> fileNames) =>
            fileNames.Select(fileName => new MessagePayload(fileName));
    }
}
