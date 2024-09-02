using SystemBackend.Core.Entities;
using SystemBackend.Core.Enums;

namespace SystemBackend.Core.Dtos.Job
{
    public class JobGetDto
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public JobLevel Level { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
