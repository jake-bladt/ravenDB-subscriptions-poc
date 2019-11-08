#pragma warning disable 4014

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace rdbsubpoc
{
    class Program
    {
        public static IDocumentStore RavenStore { get; set; }

        static string FormatTimeOfDay(DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }

        static async Task WatchSubscription()
        {
            var subName = await RavenStore.Subscriptions.CreateAsync<ImportantThing>(it => it.Status == "Quarantined");
            var worker = RavenStore.Subscriptions.GetSubscriptionWorker<ImportantThing>(subName);
            await worker.Run(x => Console.WriteLine($"{x.Items.Count} quarantined items."));
        }

        static void Main(string[] args)
        {
            var certPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", String.Empty), 
                "free.jakebcodes.client.certificate.pfx");
            RavenStore = new DocumentStore
            {
                Urls = new[]
                {
                    "https://a.free.jakebcodes.ravendb.cloud"
                },
                Database = "JunkDrawer",
                Conventions = { },
                Certificate = new X509Certificate2(certPath)
            };
            RavenStore.Initialize();

            new ImportantThing_Search().Execute(RavenStore);
            WatchSubscription();

            using(IDocumentSession session = RavenStore.OpenSession())
            {
                for(int j = 11; j <= 20; j++)
                {
                    for(int i = 1; i <= Math.Pow(j, 2.0); i++)
                    {
                        var h = new ImportantThing { Name = $"Virus #{i * j}", Status = "Quarantined" };
                        session.Store(h);
                    }
                    session.SaveChanges();
                }
            }

            Console.ReadLine();
        }
    }
}
