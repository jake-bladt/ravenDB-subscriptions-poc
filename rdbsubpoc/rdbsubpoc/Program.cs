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

            new ImportantThing_Search().Execute(RavenStore);

            using(IDocumentSession session = RavenStore.OpenSession())
            {
                for(int i = 2233; i <= 23333; i++)
                {
                    var h = new ImportantThing { Name = $"Not-So-Suspicious Item #{i}", Status = "Nothing to see here." };
                    session.Store(h);
                }
                session.SaveChanges();
            }

        }
    }
}
