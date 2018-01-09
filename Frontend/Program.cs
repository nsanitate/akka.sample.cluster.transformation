using System;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Akka.Util.Internal;
using Sample.Cluster.Transformation.Protocol;

namespace Sample.Cluster.Transformation.Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = args.Length > 0 ? args[0] : "0";
            var config =
              ConfigurationFactory.ParseString("akka.remote.dot-netty.tcp.port=" + port)
              .WithFallback(ConfigurationFactory.ParseString("akka.cluster.roles = [frontend]"))
              .WithFallback(Configuration.Configuration.Fallback);

            var system = ActorSystem.Create("ClusterSystem", config);

            var frontend = system.ActorOf(Props.Create<TransformationFrontend>(), "frontend");
            var interval = TimeSpan.FromSeconds(2);
            var timeout = TimeSpan.FromSeconds(5);
            var counter = new AtomicCounter();
            system.Scheduler.Advanced.ScheduleRepeatedly(interval, interval, () => frontend.Ask(new TransformationJob("hello-" + counter.GetAndIncrement()), timeout)
            .ContinueWith(r => Console.WriteLine(r.Result)));

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
