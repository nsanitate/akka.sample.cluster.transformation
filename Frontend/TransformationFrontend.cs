using System.Collections.Generic;
using Akka.Actor;
using Sample.Cluster.Transformation.Protocol;

namespace Sample.Cluster.Transformation.Frontend
{
    public class TransformationFrontend : UntypedActor
    {
        List<IActorRef> Backends = new List<IActorRef>();
        int JobCounter = 0;

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case TransformationJob job:
                    if (Backends.Count == 0)
                    {
                        Sender.Tell(new JobFailed("Service unavailable, try again later", job), Self);
                    }
                    else
                    {
                        JobCounter++;
                        Backends[JobCounter % Backends.Count]
                            .Forward(job);
                    }
                    break;
                case Strings.BACKEND_REGISTRATION:
                    Context.Watch(Sender);
                    Backends.Add(Sender);
                    break;
                case Terminated terminated:
                    Backends.Remove(terminated.ActorRef);
                    break;
            }
        }
    }
}