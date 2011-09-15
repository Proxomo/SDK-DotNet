using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Proxomo
{
    [DataContract(Namespace = "")]
    public abstract class CustomDataStorageBase
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "TableName")]
        public string TableName { get; set; }

        [DataMember(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}