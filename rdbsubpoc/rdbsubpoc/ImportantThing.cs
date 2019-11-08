using System;

namespace rdbsubpoc
{
    public class ImportantThing
    {
        public string Name { get; set; }
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Available";
    }
}
