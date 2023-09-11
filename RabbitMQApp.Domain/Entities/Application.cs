namespace RabbitMQApp.Domain.Entities
{
    public class Application
    {
        public Application(string name)
        {
            Name = name;
            Events = new List<EventApplication>();
        }

        public void AddEvent(string fileName, DateTime? dateTime)
            => Events.Add(new(fileName, dateTime));

        public string Name { get; }
        public int Count => Events.Count;
        public IList<EventApplication> Events { get; }
    }
}
