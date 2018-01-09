using System;

namespace Sample.Cluster.Transformation.Protocol
{
    public class JobFailed
    {
        public String Reason { get; set; }
        public TransformationJob Job { get; set; }

        public JobFailed(String reason, TransformationJob job) {
            Reason = reason;
            Job = job;
        }

        public override String ToString() {
          return $"JobFailed(${Reason})";
        }
    }
}
