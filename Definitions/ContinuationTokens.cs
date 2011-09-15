using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace Proxomo
{
    [DataContract(Namespace = "")]
    public partial class ContinuationTokens
    {
        [DataMember(Name = "NextPartitionKey")]
        public string NextPartitionKey { get; set; }

        [DataMember(Name = "NextRowKey")]
        public string NextRowKey { get; set; }

        public ContinuationTokens(string NextPKey, string NextRKey)
        {
            NextPartitionKey = NextPKey;
            NextRowKey = NextRKey;
        }
    }

}
