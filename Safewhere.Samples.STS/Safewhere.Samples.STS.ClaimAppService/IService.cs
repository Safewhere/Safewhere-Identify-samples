using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Safewhere.Samples.STS.ClaimAppService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        List<Claim> GetActorClaims();
        [OperationContract]
        List<Claim> GetClaims();
    }
    [DataContract]
    public class Claim
    {
        private string _claimType;
        private string _value;
        private string _valueType;
        private string _subject;
        private string _issuer;

        [DataMember]
        public string ClaimType
        {
            get { return _claimType; }
            set { _claimType = value; }
        }

        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [DataMember]
        public string ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        [DataMember]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        [DataMember]
        public string Issuer
        {
            get { return _issuer; }
            set { _issuer = value; }
        }
    }
}
