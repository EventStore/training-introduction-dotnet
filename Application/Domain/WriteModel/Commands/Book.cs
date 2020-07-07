using Application.EventSourcing;

namespace Application.Domain.Commands
{
    public class Book : Command
    {
        public string Id { get; }
        public string PatientId { get; }

        public Book(string id, string patientId)
        {
            Id = id;
            PatientId = patientId;
        }
    }
}