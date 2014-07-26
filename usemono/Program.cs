using Nancy.Hosting.Self;
using System;
using Topshelf;

namespace usemono
{
    internal class Program
    {
        private const int DEFAULT_PORT = 3579;

        private static void Main(string[] args)
        {
            int port;

            var newPort = Environment.GetEnvironmentVariable("PORT");
            if (!int.TryParse(newPort, out port)) port = DEFAULT_PORT;

            HostFactory.Run(x =>
            {
                x.UseLinuxIfAvailable();
                x.Service<NancyApp>(s =>
                {
                    s.ConstructUsing(name => new NancyApp("http://localhost:" + port));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
            });
        }
    }

    internal class NancyApp
    {
        private readonly NancyHost _host;

        public NancyApp(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentNullException("uri");

            _host = new NancyHost(new Uri(uri), new Uri(uri.Replace("http:", "https:")));

        }

        public void Start()
        {
            _host.Start();
        }

        public void Stop()
        {
            _host.Stop();
        }
    }
}