﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Safewhere.Samples.STS.Common.ClaimAppService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Claim", Namespace="http://schemas.datacontract.org/2004/07/Safewhere.Samples.STS.ClaimAppService")]
    [System.SerializableAttribute()]
    public partial class Claim : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClaimTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IssuerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueTypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClaimType {
            get {
                return this.ClaimTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ClaimTypeField, value) != true)) {
                    this.ClaimTypeField = value;
                    this.RaisePropertyChanged("ClaimType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Issuer {
            get {
                return this.IssuerField;
            }
            set {
                if ((object.ReferenceEquals(this.IssuerField, value) != true)) {
                    this.IssuerField = value;
                    this.RaisePropertyChanged("Issuer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Subject {
            get {
                return this.SubjectField;
            }
            set {
                if ((object.ReferenceEquals(this.SubjectField, value) != true)) {
                    this.SubjectField = value;
                    this.RaisePropertyChanged("Subject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ValueType {
            get {
                return this.ValueTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueTypeField, value) != true)) {
                    this.ValueTypeField = value;
                    this.RaisePropertyChanged("ValueType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ClaimAppService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetActorClaims", ReplyAction="http://tempuri.org/IService/GetActorClaimsResponse")]
        Safewhere.Samples.STS.Common.ClaimAppService.Claim[] GetActorClaims();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetActorClaims", ReplyAction="http://tempuri.org/IService/GetActorClaimsResponse")]
        System.Threading.Tasks.Task<Safewhere.Samples.STS.Common.ClaimAppService.Claim[]> GetActorClaimsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetClaims", ReplyAction="http://tempuri.org/IService/GetClaimsResponse")]
        Safewhere.Samples.STS.Common.ClaimAppService.Claim[] GetClaims();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetClaims", ReplyAction="http://tempuri.org/IService/GetClaimsResponse")]
        System.Threading.Tasks.Task<Safewhere.Samples.STS.Common.ClaimAppService.Claim[]> GetClaimsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : Safewhere.Samples.STS.Common.ClaimAppService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<Safewhere.Samples.STS.Common.ClaimAppService.IService>, Safewhere.Samples.STS.Common.ClaimAppService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Safewhere.Samples.STS.Common.ClaimAppService.Claim[] GetActorClaims() {
            return base.Channel.GetActorClaims();
        }
        
        public System.Threading.Tasks.Task<Safewhere.Samples.STS.Common.ClaimAppService.Claim[]> GetActorClaimsAsync() {
            return base.Channel.GetActorClaimsAsync();
        }
        
        public Safewhere.Samples.STS.Common.ClaimAppService.Claim[] GetClaims() {
            return base.Channel.GetClaims();
        }
        
        public System.Threading.Tasks.Task<Safewhere.Samples.STS.Common.ClaimAppService.Claim[]> GetClaimsAsync() {
            return base.Channel.GetClaimsAsync();
        }
    }
}
