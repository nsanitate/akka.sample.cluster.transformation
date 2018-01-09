using System;
using Akka;
using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using Sample.Cluster.Transformation.Protocol;
using static Akka.Cluster.ClusterEvent;

namespace Sample.Cluster.Transformation.Backend
{
    public class TransformationBackend : UntypedActor
    {
        ILoggingAdapter Log;
        Akka.Cluster.Cluster Cluster;

        public TransformationBackend()
        {
            Log = Logging.GetLogger(Context.System, this);
            Cluster = Akka.Cluster.Cluster.Get(Context.System);
        }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, typeof(MemberUp));
        }

        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case TransformationJob job:
                    Log.Info("Working on message: {0}", job.Text);
                    Sender.Tell(new TransformationResult(job.Text.ToUpper()), Self);
                    break;
                case CurrentClusterState state:
                    foreach (var member in state.Members)
                    {
                        if (member.Status == MemberStatus.Up)
                        {
                            Register(member);
                        }
                    }
                    break;
                case MemberUp mUp:
                    Register(mUp.Member);
                    break;
            }
        }

        private void Register(Member member)
        {
            if (member.HasRole("frontend"))
            {
                Context.ActorSelection(member.Address + "/user/frontend")
                    .Tell(Strings.BACKEND_REGISTRATION, Self);
            }
        }
    }
}