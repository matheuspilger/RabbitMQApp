using RabbitMQApp.Domain.Entities.Payloads;
using RabbitMQApp.Domain.Helpers;

namespace RabbitMQApp.Domain.Entities
{
    public class EventFile
    {
        public EventFile(IEnumerable<Application> applications)
        {
            Applications = applications;
        }

        public int Count => Applications.Sum(a => a.Count);
        public IEnumerable<Application> Applications { get; }

        public static EventFile Build(IReadOnlyList<MessagePayload> payloads)
        {
            var applications = new List<Application>();

            foreach (var payload in payloads)
            {
                var fileInfos = payload.FileName.Replace(".txt", string.Empty).Split("_");
                var applicationName = fileInfos.GetValue(1)!.ToString();
                var dateTime = fileInfos.GetValue(2)!.ToString()!.Parse();

                var application = applications.FirstOrDefault(a => a.Name == applicationName);
                application ??= new Application(applicationName!);
                application.AddEvent(payload.FileName, dateTime);
                
                if(!applications.Contains(application)) applications.Add(application);
            }

            return new(applications);
        }
    }
}
