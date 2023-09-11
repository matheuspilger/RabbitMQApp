using RabbitMQApp.Domain.Entities;
using RabbitMQApp.Domain.Entities.Payloads;

namespace RabbitMQApp.UnitTests
{
    public class EventFileTests
    {
        [Fact]
        public void Count_ReturnsCorrectEventAndApplicationCount()
        {
            // Arrange
            var payloads = BuildPayloads();

            // Act
            var eventFile = EventFile.Build(payloads);

            // Asserts
            Assert.NotNull(eventFile);
            Assert.NotEmpty(eventFile.Applications);
            Assert.Equal(payloads.Count, eventFile.Count);
            Assert.Equal(3, eventFile.Applications.Count());
        }

        private IReadOnlyList<MessagePayload> BuildPayloads()
        {
            return new List<MessagePayload>()
            {
                new MessagePayload("ABCD_SMSSender_20230911091559.txt"),
                new MessagePayload("ABCD_SMSSender_20230911091559.txt"),
                new MessagePayload("ABCD_RecargaIVR_20230911091559.txt"),
                new MessagePayload("ABCD_RecargaIVR_20230911091559.txt"),
                new MessagePayload("ABCD_RecargaSMS_20230911091559.txt"),
                new MessagePayload("ABCD_RecargaSMS_20230911091559.txt")
            };
        }
    }
}