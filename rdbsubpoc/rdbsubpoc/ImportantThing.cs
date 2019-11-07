using System;

namespace rdbsubpoc
{
    public class ImportantThing
    {
        public ImportantThing() { CreatedOnUtc = DateTime.UtcNow; }

        public string Name { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
