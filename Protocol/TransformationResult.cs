using System;

namespace Sample.Cluster.Transformation.Protocol
{
    public class TransformationResult
    {
        public String Text { get; set; }

        public TransformationResult(String text) {
            Text = text;
        }

        public override String ToString() {
          return $"TransformationResult(${Text})";
        }
    }
}
