using System;
using Akka.Actor;
using Akka.Configuration;
using Sample.Cluster.Transformation.Configuration;

namespace Sample.Cluster.Transformation.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            // Override the configuration of the port when specified as program argument
            var port = args.Length > 0 ? args[0] : "0";
            var config =
              ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
              .WithFallback(ConfigurationFactory.ParseString("akka.cluster.roles = [backend]"))
              .WithFallback(Sample.Cluster.Transformation.Configuration.Configuration.Fallback);

            var system = ActorSystem.Create("ClusterSystem", config);

            system.ActorOf(Props.Create<TransformationBackend>(), "backend");
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
