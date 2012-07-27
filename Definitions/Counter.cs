using System;
using System.Runtime.Serialization;

namespace Proxomo
{
    [DataContract(Namespace = "")]
    public class Counter
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Value")]
        public int Value { get; set; }
    }
}