using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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

        static void Main(string[] args)
        {
            var certPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", String.Empty), 
                "free.jakebcodes.client.certificate.pfx");
            // var cert = Encoding.UTF8.GetBytes(File.ReadAllText(certPath));
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

            using(IDocumentSession session = RavenStore.OpenSession())
            {
                var it1 = new ImportantThing { Name = "Whiskers on Kittens " };
                session.Store(it1);

                Console.WriteLine($"Item '{it1.Name}' created at {FormatTimeOfDay(it1.CreatedOnUtc)}");
            }

            Console.ReadLine();

        }
    }
}
