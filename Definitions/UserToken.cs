using System;
using System.Runtime.Serialization;

namespace Proxomo
{
    [DataContract(Namespace = "")]
    public partial class UserToken : Token
    {
        [DataMember(Name = "PersonID")]
        public string PersonID { get; set; }

        [DataMember(Name = "Role")]
        public string Role { get; set; }
    }
}
