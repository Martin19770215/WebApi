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

namespace WebApi.com.logicnx.ws.mysql {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WS_MYSQLSoap", Namespace="http://tempuri.org/")]
    public partial class WS_MYSQL : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ExecuteDataSetOperationCompleted;
        
        private System.Threading.SendOrPostCallback ExecuteNonQueryOperationCompleted;
        
        private System.Threading.SendOrPostCallback ExecuteScalarOperationCompleted;
        
        private System.Threading.SendOrPostCallback ExecuteTransactionOperationCompleted;
        
        private System.Threading.SendOrPostCallback ExecuteDataSetBySQLOperationCompleted;
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback WriteBLOBValueOperationCompleted;
        
        private System.Threading.SendOrPostCallback ConnectionTestOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WS_MYSQL() {
            this.Url = global::WebApi.Properties.Settings.Default.WebApi_com_logicnx_ws_WS_MYSQL;
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
        public event ExecuteDataSetCompletedEventHandler ExecuteDataSetCompleted;
        
        /// <remarks/>
        public event ExecuteNonQueryCompletedEventHandler ExecuteNonQueryCompleted;
        
        /// <remarks/>
        public event ExecuteScalarCompletedEventHandler ExecuteScalarCompleted;
        
        /// <remarks/>
        public event ExecuteTransactionCompletedEventHandler ExecuteTransactionCompleted;
        
        /// <remarks/>
        public event ExecuteDataSetBySQLCompletedEventHandler ExecuteDataSetBySQLCompleted;
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event WriteBLOBValueCompletedEventHandler WriteBLOBValueCompleted;
        
        /// <remarks/>
        public event ConnectionTestCompletedEventHandler ConnectionTestCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExecuteDataSet", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet ExecuteDataSet(string[] paras, string commandType, string commandText, string Schemas) {
            object[] results = this.Invoke("ExecuteDataSet", new object[] {
                        paras,
                        commandType,
                        commandText,
                        Schemas});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteDataSetAsync(string[] paras, string commandType, string commandText, string Schemas) {
            this.ExecuteDataSetAsync(paras, commandType, commandText, Schemas, null);
        }
        
        /// <remarks/>
        public void ExecuteDataSetAsync(string[] paras, string commandType, string commandText, string Schemas, object userState) {
            if ((this.ExecuteDataSetOperationCompleted == null)) {
                this.ExecuteDataSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteDataSetOperationCompleted);
            }
            this.InvokeAsync("ExecuteDataSet", new object[] {
                        paras,
                        commandType,
                        commandText,
                        Schemas}, this.ExecuteDataSetOperationCompleted, userState);
        }
        
        private void OnExecuteDataSetOperationCompleted(object arg) {
            if ((this.ExecuteDataSetCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteDataSetCompleted(this, new ExecuteDataSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExecuteNonQuery", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int ExecuteNonQuery(string[] paras, string commandType, string commandText, string Schemas) {
            object[] results = this.Invoke("ExecuteNonQuery", new object[] {
                        paras,
                        commandType,
                        commandText,
                        Schemas});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteNonQueryAsync(string[] paras, string commandType, string commandText, string Schemas) {
            this.ExecuteNonQueryAsync(paras, commandType, commandText, Schemas, null);
        }
        
        /// <remarks/>
        public void ExecuteNonQueryAsync(string[] paras, string commandType, string commandText, string Schemas, object userState) {
            if ((this.ExecuteNonQueryOperationCompleted == null)) {
                this.ExecuteNonQueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteNonQueryOperationCompleted);
            }
            this.InvokeAsync("ExecuteNonQuery", new object[] {
                        paras,
                        commandType,
                        commandText,
                        Schemas}, this.ExecuteNonQueryOperationCompleted, userState);
        }
        
        private void OnExecuteNonQueryOperationCompleted(object arg) {
            if ((this.ExecuteNonQueryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteNonQueryCompleted(this, new ExecuteNonQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExecuteScalar", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ExecuteScalar(string[] paras, string commandType, string commandText, string Schemas) {
            object[] results = this.Invoke("ExecuteScalar", new object[] {
                        paras,
                        commandType,
                        commandText,
                        Schemas});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteScalarAsync(string[] paras, string commandType, string commandText, string Schemas) {
            this.ExecuteScalarAsync(paras, commandType, commandText, Schemas, null);
        }
        
        /// <remarks/>
        public void ExecuteScalarAsync(string[] paras, string commandType, string commandText, string Schemas, object userState) {
            if ((this.ExecuteScalarOperationCompleted == null)) {
                this.ExecuteScalarOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteScalarOperationCompleted);
            }
            this.InvokeAsync("ExecuteScalar", new object[] {
                        paras,
                        commandType,
                        commandText,
                        Schemas}, this.ExecuteScalarOperationCompleted, userState);
        }
        
        private void OnExecuteScalarOperationCompleted(object arg) {
            if ((this.ExecuteScalarCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteScalarCompleted(this, new ExecuteScalarCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExecuteTransaction", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool ExecuteTransaction([System.Xml.Serialization.XmlArrayItemAttribute("ArrayOfString")] [System.Xml.Serialization.XmlArrayItemAttribute(NestingLevel=1)] string[][] param, string[] commandText, string Schemas) {
            object[] results = this.Invoke("ExecuteTransaction", new object[] {
                        param,
                        commandText,
                        Schemas});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteTransactionAsync(string[][] param, string[] commandText, string Schemas) {
            this.ExecuteTransactionAsync(param, commandText, Schemas, null);
        }
        
        /// <remarks/>
        public void ExecuteTransactionAsync(string[][] param, string[] commandText, string Schemas, object userState) {
            if ((this.ExecuteTransactionOperationCompleted == null)) {
                this.ExecuteTransactionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteTransactionOperationCompleted);
            }
            this.InvokeAsync("ExecuteTransaction", new object[] {
                        param,
                        commandText,
                        Schemas}, this.ExecuteTransactionOperationCompleted, userState);
        }
        
        private void OnExecuteTransactionOperationCompleted(object arg) {
            if ((this.ExecuteTransactionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteTransactionCompleted(this, new ExecuteTransactionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ExecuteDataSetBySQL", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet ExecuteDataSetBySQL(string strSql, string Schemas) {
            object[] results = this.Invoke("ExecuteDataSetBySQL", new object[] {
                        strSql,
                        Schemas});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteDataSetBySQLAsync(string strSql, string Schemas) {
            this.ExecuteDataSetBySQLAsync(strSql, Schemas, null);
        }
        
        /// <remarks/>
        public void ExecuteDataSetBySQLAsync(string strSql, string Schemas, object userState) {
            if ((this.ExecuteDataSetBySQLOperationCompleted == null)) {
                this.ExecuteDataSetBySQLOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteDataSetBySQLOperationCompleted);
            }
            this.InvokeAsync("ExecuteDataSetBySQL", new object[] {
                        strSql,
                        Schemas}, this.ExecuteDataSetBySQLOperationCompleted, userState);
        }
        
        private void OnExecuteDataSetBySQLOperationCompleted(object arg) {
            if ((this.ExecuteDataSetBySQLCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteDataSetBySQLCompleted(this, new ExecuteDataSetBySQLCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld(string DataBase) {
            object[] results = this.Invoke("HelloWorld", new object[] {
                        DataBase});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync(string DataBase) {
            this.HelloWorldAsync(DataBase, null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(string DataBase, object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[] {
                        DataBase}, this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/WriteBLOBValue", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool WriteBLOBValue([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] KeyValue, string commandText, string Schemas) {
            object[] results = this.Invoke("WriteBLOBValue", new object[] {
                        KeyValue,
                        commandText,
                        Schemas});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void WriteBLOBValueAsync(byte[] KeyValue, string commandText, string Schemas) {
            this.WriteBLOBValueAsync(KeyValue, commandText, Schemas, null);
        }
        
        /// <remarks/>
        public void WriteBLOBValueAsync(byte[] KeyValue, string commandText, string Schemas, object userState) {
            if ((this.WriteBLOBValueOperationCompleted == null)) {
                this.WriteBLOBValueOperationCompleted = new System.Threading.SendOrPostCallback(this.OnWriteBLOBValueOperationCompleted);
            }
            this.InvokeAsync("WriteBLOBValue", new object[] {
                        KeyValue,
                        commandText,
                        Schemas}, this.WriteBLOBValueOperationCompleted, userState);
        }
        
        private void OnWriteBLOBValueOperationCompleted(object arg) {
            if ((this.WriteBLOBValueCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.WriteBLOBValueCompleted(this, new WriteBLOBValueCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ConnectionTest", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ConnectionTest(string Schemas, string Tables) {
            object[] results = this.Invoke("ConnectionTest", new object[] {
                        Schemas,
                        Tables});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ConnectionTestAsync(string Schemas, string Tables) {
            this.ConnectionTestAsync(Schemas, Tables, null);
        }
        
        /// <remarks/>
        public void ConnectionTestAsync(string Schemas, string Tables, object userState) {
            if ((this.ConnectionTestOperationCompleted == null)) {
                this.ConnectionTestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConnectionTestOperationCompleted);
            }
            this.InvokeAsync("ConnectionTest", new object[] {
                        Schemas,
                        Tables}, this.ConnectionTestOperationCompleted, userState);
        }
        
        private void OnConnectionTestOperationCompleted(object arg) {
            if ((this.ConnectionTestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConnectionTestCompleted(this, new ConnectionTestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void ExecuteDataSetCompletedEventHandler(object sender, ExecuteDataSetCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteDataSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteDataSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void ExecuteNonQueryCompletedEventHandler(object sender, ExecuteNonQueryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteNonQueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteNonQueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void ExecuteScalarCompletedEventHandler(object sender, ExecuteScalarCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteScalarCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteScalarCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void ExecuteTransactionCompletedEventHandler(object sender, ExecuteTransactionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteTransactionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteTransactionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void ExecuteDataSetBySQLCompletedEventHandler(object sender, ExecuteDataSetBySQLCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteDataSetBySQLCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteDataSetBySQLCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void WriteBLOBValueCompletedEventHandler(object sender, WriteBLOBValueCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class WriteBLOBValueCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal WriteBLOBValueCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    public delegate void ConnectionTestCompletedEventHandler(object sender, ConnectionTestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConnectionTestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConnectionTestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591