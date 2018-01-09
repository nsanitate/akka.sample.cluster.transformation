using System;

namespace Sample.Cluster.Transformation.Protocol
{
    public class TransformationJob
    {
        public String Text { get; set; }

        public TransformationJob(String text) {
            Text = text;
        }
    }
}
