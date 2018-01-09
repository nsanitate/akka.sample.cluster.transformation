﻿using System;
using Akka.Configuration;

namespace Sample.Cluster.Transformation.Configuration
{
    public class Configuration
    {
        public static Config Fallback = ConfigurationFactory.ParseString(@"
				akka {
                    actor {
                        provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                    }

                    remote {
                        dot-netty.tcp {
                            hostname = ""127.0.0.1""
                            port = 0
                        }
                    }
                    cluster {
                        seed-nodes = [
                            ""akka.tcp://ClusterSystem@127.0.0.1:2551""
                            ""akka.tcp://ClusterSystem@127.0.0.1:2552""]

                        # auto downing is NOT safe for production deployments.
                        # you may want to use it during development, read more about it in the docs.
                        auto-down-unreachable-after = 10s
                    }
                }
            ");
    }
}