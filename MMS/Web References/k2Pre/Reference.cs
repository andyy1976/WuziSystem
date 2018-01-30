﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace mms.k2Pre {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="K2WebServiceForMMSPreSoap", Namespace="http://tempuri.org/")]
    public partial class K2WebServiceForMMSPre : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback StartPreparesProgressOperationCompleted;
        
        private System.Threading.SendOrPostCallback DeleteProcessInstOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetApproveHeaderOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetApproveBodyOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public K2WebServiceForMMSPre() {
            this.Url = global::mms.Properties.Settings.Default.mms_k2Pre_K2WebServiceForMMSPre;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event StartPreparesProgressCompletedEventHandler StartPreparesProgressCompleted;
        
        /// <remarks/>
        public event DeleteProcessInstCompletedEventHandler DeleteProcessInstCompleted;
        
        /// <remarks/>
        public event GetApproveHeaderCompletedEventHandler GetApproveHeaderCompleted;
        
        /// <remarks/>
        public event GetApproveBodyCompletedEventHandler GetApproveBodyCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/StartPreparesProgress", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool StartPreparesProgress(PreparesMarerialsHeadModel DataHead, PreparesMarerialsBodyModel[] DataBodyList) {
            object[] results = this.Invoke("StartPreparesProgress", new object[] {
                        DataHead,
                        DataBodyList});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void StartPreparesProgressAsync(PreparesMarerialsHeadModel DataHead, PreparesMarerialsBodyModel[] DataBodyList) {
            this.StartPreparesProgressAsync(DataHead, DataBodyList, null);
        }
        
        /// <remarks/>
        public void StartPreparesProgressAsync(PreparesMarerialsHeadModel DataHead, PreparesMarerialsBodyModel[] DataBodyList, object userState) {
            if ((this.StartPreparesProgressOperationCompleted == null)) {
                this.StartPreparesProgressOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStartPreparesProgressOperationCompleted);
            }
            this.InvokeAsync("StartPreparesProgress", new object[] {
                        DataHead,
                        DataBodyList}, this.StartPreparesProgressOperationCompleted, userState);
        }
        
        private void OnStartPreparesProgressOperationCompleted(object arg) {
            if ((this.StartPreparesProgressCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.StartPreparesProgressCompleted(this, new StartPreparesProgressCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/DeleteProcessInst", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool DeleteProcessInst(PreparesMarerialsHeadModel Head, bool OnlyProcess) {
            object[] results = this.Invoke("DeleteProcessInst", new object[] {
                        Head,
                        OnlyProcess});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteProcessInstAsync(PreparesMarerialsHeadModel Head, bool OnlyProcess) {
            this.DeleteProcessInstAsync(Head, OnlyProcess, null);
        }
        
        /// <remarks/>
        public void DeleteProcessInstAsync(PreparesMarerialsHeadModel Head, bool OnlyProcess, object userState) {
            if ((this.DeleteProcessInstOperationCompleted == null)) {
                this.DeleteProcessInstOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteProcessInstOperationCompleted);
            }
            this.InvokeAsync("DeleteProcessInst", new object[] {
                        Head,
                        OnlyProcess}, this.DeleteProcessInstOperationCompleted, userState);
        }
        
        private void OnDeleteProcessInstOperationCompleted(object arg) {
            if ((this.DeleteProcessInstCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteProcessInstCompleted(this, new DeleteProcessInstCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetApproveHeader", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ApproveInfoHead GetApproveHeader(PreparesMarerialsHeadModel Head) {
            object[] results = this.Invoke("GetApproveHeader", new object[] {
                        Head});
            return ((ApproveInfoHead)(results[0]));
        }
        
        /// <remarks/>
        public void GetApproveHeaderAsync(PreparesMarerialsHeadModel Head) {
            this.GetApproveHeaderAsync(Head, null);
        }
        
        /// <remarks/>
        public void GetApproveHeaderAsync(PreparesMarerialsHeadModel Head, object userState) {
            if ((this.GetApproveHeaderOperationCompleted == null)) {
                this.GetApproveHeaderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetApproveHeaderOperationCompleted);
            }
            this.InvokeAsync("GetApproveHeader", new object[] {
                        Head}, this.GetApproveHeaderOperationCompleted, userState);
        }
        
        private void OnGetApproveHeaderOperationCompleted(object arg) {
            if ((this.GetApproveHeaderCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetApproveHeaderCompleted(this, new GetApproveHeaderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetApproveBody", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ApproveInfoBody[] GetApproveBody(PreparesMarerialsHeadModel Head) {
            object[] results = this.Invoke("GetApproveBody", new object[] {
                        Head});
            return ((ApproveInfoBody[])(results[0]));
        }
        
        /// <remarks/>
        public void GetApproveBodyAsync(PreparesMarerialsHeadModel Head) {
            this.GetApproveBodyAsync(Head, null);
        }
        
        /// <remarks/>
        public void GetApproveBodyAsync(PreparesMarerialsHeadModel Head, object userState) {
            if ((this.GetApproveBodyOperationCompleted == null)) {
                this.GetApproveBodyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetApproveBodyOperationCompleted);
            }
            this.InvokeAsync("GetApproveBody", new object[] {
                        Head}, this.GetApproveBodyOperationCompleted, userState);
        }
        
        private void OnGetApproveBodyOperationCompleted(object arg) {
            if ((this.GetApproveBodyCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetApproveBodyCompleted(this, new GetApproveBodyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class PreparesMarerialsHeadModel {
        
        private int processInstIDField;
        
        private int requestIDField;
        
        private int requestTypeField;
        
        private string userNameField;
        
        private string userAccountField;
        
        private string submitDateField;
        
        private string diaoDuApproveAccountField;
        
        private string keTiApproveAccountField;
        
        private string jiHuaXingHaoApproveAccountField;
        
        private string zongHeApproveAccountField;
        
        private string appStateField;
        
        private bool is_DelField;
        
        /// <remarks/>
        public int ProcessInstID {
            get {
                return this.processInstIDField;
            }
            set {
                this.processInstIDField = value;
            }
        }
        
        /// <remarks/>
        public int RequestID {
            get {
                return this.requestIDField;
            }
            set {
                this.requestIDField = value;
            }
        }
        
        /// <remarks/>
        public int RequestType {
            get {
                return this.requestTypeField;
            }
            set {
                this.requestTypeField = value;
            }
        }
        
        /// <remarks/>
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        public string UserAccount {
            get {
                return this.userAccountField;
            }
            set {
                this.userAccountField = value;
            }
        }
        
        /// <remarks/>
        public string SubmitDate {
            get {
                return this.submitDateField;
            }
            set {
                this.submitDateField = value;
            }
        }
        
        /// <remarks/>
        public string DiaoDuApproveAccount {
            get {
                return this.diaoDuApproveAccountField;
            }
            set {
                this.diaoDuApproveAccountField = value;
            }
        }
        
        /// <remarks/>
        public string KeTiApproveAccount {
            get {
                return this.keTiApproveAccountField;
            }
            set {
                this.keTiApproveAccountField = value;
            }
        }
        
        /// <remarks/>
        public string JiHuaXingHaoApproveAccount {
            get {
                return this.jiHuaXingHaoApproveAccountField;
            }
            set {
                this.jiHuaXingHaoApproveAccountField = value;
            }
        }
        
        /// <remarks/>
        public string ZongHeApproveAccount {
            get {
                return this.zongHeApproveAccountField;
            }
            set {
                this.zongHeApproveAccountField = value;
            }
        }
        
        /// <remarks/>
        public string AppState {
            get {
                return this.appStateField;
            }
            set {
                this.appStateField = value;
            }
        }
        
        /// <remarks/>
        public bool Is_Del {
            get {
                return this.is_DelField;
            }
            set {
                this.is_DelField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ApproveInfoBody {
        
        private string activityNameField;
        
        private string approveLeaderNameField;
        
        private string approveLeaderDeptField;
        
        private string resultField;
        
        private string reasonField;
        
        private System.DateTime approveTimeField;
        
        /// <remarks/>
        public string ActivityName {
            get {
                return this.activityNameField;
            }
            set {
                this.activityNameField = value;
            }
        }
        
        /// <remarks/>
        public string ApproveLeaderName {
            get {
                return this.approveLeaderNameField;
            }
            set {
                this.approveLeaderNameField = value;
            }
        }
        
        /// <remarks/>
        public string ApproveLeaderDept {
            get {
                return this.approveLeaderDeptField;
            }
            set {
                this.approveLeaderDeptField = value;
            }
        }
        
        /// <remarks/>
        public string Result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
            }
        }
        
        /// <remarks/>
        public string Reason {
            get {
                return this.reasonField;
            }
            set {
                this.reasonField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ApproveTime {
            get {
                return this.approveTimeField;
            }
            set {
                this.approveTimeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class ApproveInfoHead {
        
        private int processInstIDField;
        
        private int mMS_IDField;
        
        private int appStateField;
        
        private string approvingActivityField;
        
        private string approvingUserField;
        
        private string approvingUserDeptField;
        
        /// <remarks/>
        public int ProcessInstID {
            get {
                return this.processInstIDField;
            }
            set {
                this.processInstIDField = value;
            }
        }
        
        /// <remarks/>
        public int MMS_ID {
            get {
                return this.mMS_IDField;
            }
            set {
                this.mMS_IDField = value;
            }
        }
        
        /// <remarks/>
        public int AppState {
            get {
                return this.appStateField;
            }
            set {
                this.appStateField = value;
            }
        }
        
        /// <remarks/>
        public string ApprovingActivity {
            get {
                return this.approvingActivityField;
            }
            set {
                this.approvingActivityField = value;
            }
        }
        
        /// <remarks/>
        public string ApprovingUser {
            get {
                return this.approvingUserField;
            }
            set {
                this.approvingUserField = value;
            }
        }
        
        /// <remarks/>
        public string ApprovingUserDept {
            get {
                return this.approvingUserDeptField;
            }
            set {
                this.approvingUserDeptField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class PreparesMarerialsBodyModel {
        
        private int processInstIDField;
        
        private string drawingNumField;
        
        private string taskNumField;
        
        private string subjectNumField;
        
        private string itemCodeField;
        
        private string demandNumSumField;
        
        private string matUnitField;
        
        private string quantityField;
        
        private string roughSizeField;
        
        private string dingeSizeField;
        
        private string roughSpecField;
        
        private string demandDateField;
        
        private string specialNeedsField;
        
        private string urgencyField;
        
        private string secretLevelField;
        
        private string stageField;
        
        private string useDesField;
        
        private string certificationField;
        
        private string materialDeptField;
        
        private string material_NameField;
        
        private string countryField;
        
        /// <remarks/>
        public int ProcessInstID {
            get {
                return this.processInstIDField;
            }
            set {
                this.processInstIDField = value;
            }
        }
        
        /// <remarks/>
        public string DrawingNum {
            get {
                return this.drawingNumField;
            }
            set {
                this.drawingNumField = value;
            }
        }
        
        /// <remarks/>
        public string TaskNum {
            get {
                return this.taskNumField;
            }
            set {
                this.taskNumField = value;
            }
        }
        
        /// <remarks/>
        public string SubjectNum {
            get {
                return this.subjectNumField;
            }
            set {
                this.subjectNumField = value;
            }
        }
        
        /// <remarks/>
        public string ItemCode {
            get {
                return this.itemCodeField;
            }
            set {
                this.itemCodeField = value;
            }
        }
        
        /// <remarks/>
        public string DemandNumSum {
            get {
                return this.demandNumSumField;
            }
            set {
                this.demandNumSumField = value;
            }
        }
        
        /// <remarks/>
        public string MatUnit {
            get {
                return this.matUnitField;
            }
            set {
                this.matUnitField = value;
            }
        }
        
        /// <remarks/>
        public string Quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        public string RoughSize {
            get {
                return this.roughSizeField;
            }
            set {
                this.roughSizeField = value;
            }
        }
        
        /// <remarks/>
        public string DingeSize {
            get {
                return this.dingeSizeField;
            }
            set {
                this.dingeSizeField = value;
            }
        }
        
        /// <remarks/>
        public string RoughSpec {
            get {
                return this.roughSpecField;
            }
            set {
                this.roughSpecField = value;
            }
        }
        
        /// <remarks/>
        public string DemandDate {
            get {
                return this.demandDateField;
            }
            set {
                this.demandDateField = value;
            }
        }
        
        /// <remarks/>
        public string SpecialNeeds {
            get {
                return this.specialNeedsField;
            }
            set {
                this.specialNeedsField = value;
            }
        }
        
        /// <remarks/>
        public string Urgency {
            get {
                return this.urgencyField;
            }
            set {
                this.urgencyField = value;
            }
        }
        
        /// <remarks/>
        public string SecretLevel {
            get {
                return this.secretLevelField;
            }
            set {
                this.secretLevelField = value;
            }
        }
        
        /// <remarks/>
        public string Stage {
            get {
                return this.stageField;
            }
            set {
                this.stageField = value;
            }
        }
        
        /// <remarks/>
        public string UseDes {
            get {
                return this.useDesField;
            }
            set {
                this.useDesField = value;
            }
        }
        
        /// <remarks/>
        public string Certification {
            get {
                return this.certificationField;
            }
            set {
                this.certificationField = value;
            }
        }
        
        /// <remarks/>
        public string MaterialDept {
            get {
                return this.materialDeptField;
            }
            set {
                this.materialDeptField = value;
            }
        }
        
        /// <remarks/>
        public string Material_Name {
            get {
                return this.material_NameField;
            }
            set {
                this.material_NameField = value;
            }
        }
        
        /// <remarks/>
        public string Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    public delegate void StartPreparesProgressCompletedEventHandler(object sender, StartPreparesProgressCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StartPreparesProgressCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal StartPreparesProgressCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    public delegate void DeleteProcessInstCompletedEventHandler(object sender, DeleteProcessInstCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteProcessInstCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteProcessInstCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    public delegate void GetApproveHeaderCompletedEventHandler(object sender, GetApproveHeaderCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetApproveHeaderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetApproveHeaderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ApproveInfoHead Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ApproveInfoHead)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    public delegate void GetApproveBodyCompletedEventHandler(object sender, GetApproveBodyCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetApproveBodyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetApproveBodyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ApproveInfoBody[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ApproveInfoBody[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591