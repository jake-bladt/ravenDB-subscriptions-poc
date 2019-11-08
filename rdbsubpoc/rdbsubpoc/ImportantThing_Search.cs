using System.Collections.Generic;

using Raven.Client.Documents.Indexes;

namespace rdbsubpoc
{
    public class ImportantThing_Search: AbstractIndexCreationTask<ImportantThing>
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition
            {
                Maps = new HashSet<string>
                {
                    @"FROM importantThing in docs.ImportantThings
                        select new { Name = importantThing.Name, CreatedOnUtc = importantThing.CreatedOnUtc, Status = importantThing.Status }"
                }
            };
        }
    }
}
