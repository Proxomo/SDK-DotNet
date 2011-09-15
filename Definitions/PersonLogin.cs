using System.Runtime.Serialization;

namespace Proxomo
{

    [DataContract(Namespace = "")]
    public partial class PersonLogin
    {
        [DataMember(Name = "PersonID")]
        public string PersonID { get; set; }

        [DataMember(Name = "ApplicationID")]
        public string ApplicationID { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "Role")]
        public string Role { get; set; }
    }

}

