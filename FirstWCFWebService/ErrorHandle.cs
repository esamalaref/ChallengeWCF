using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;

namespace FirstWCFWebService
{    
    public class GlobalErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        { return true;}

        public void ProvideFault(System.Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message message)
        {
            var fault = new Fault();

            if (error is FaultException)
            {
                // extract the our FaultContract object from the exception object.
                var detail = error.GetType().GetProperty("Detail").GetGetMethod().Invoke(error, null);

                // create a fault message containing our FaultContract object
                message = Message.CreateMessage(version, "", detail, new DataContractJsonSerializer(detail.GetType()));

                // tell WCF to use JSON encoding rather than default XML
                var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);

                message.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

                // return custom error code.
                var rmp = new HttpResponseMessageProperty();
                rmp.StatusCode = System.Net.HttpStatusCode.BadRequest;

                // put appropraite description here..
                rmp.StatusDescription = "See fault object for more information.";

                message.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
            else if (error is SerializationException)
            {
                //message = Message.CreateMessage(version, "", "An non-fault exception is occured.", new DataContractJsonSerializer(typeof(string)));
                //var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
                //message.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

                //// return custom error code.
                //var rmp = new HttpResponseMessageProperty();

                //rmp.StatusCode = System.Net.HttpStatusCode.InternalServerError;

                //// put appropraite description here..
                //rmp.StatusDescription = "Uknown exception...";

                //message.Properties.Add(HttpResponseMessageProperty.Name, rmp);

                fault.Error= error.Message;
                fault.Error = "Could not decode request: JSON parsing failed";
                toJson(fault, ref message);
            }            
        }

        void toJson(Fault fault, ref System.ServiceModel.Channels.Message message)
        {
            message = WebOperationContext.Current.CreateJsonResponse<Fault>((Fault)fault, new DataContractJsonSerializer(typeof(Fault)));
            var response = WebOperationContext.Current.OutgoingResponse;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            response.StatusDescription = "Custom Fault";            
        }
    }

    public class ErrorHandlerExAttribute : Attribute, IServiceBehavior
    {
        private readonly Type errorHandlerType;
        public ErrorHandlerExAttribute(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {            
            IErrorHandler handler = (IErrorHandler)Activator.CreateInstance(this.errorHandlerType);
            foreach (ChannelDispatcherBase chanelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher chanelDispatcher = chanelDispatcherBase as ChannelDispatcher;
                if (chanelDispatcher != null)
                    chanelDispatcher.ErrorHandlers.Add(handler);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {                        
        }
    }

    public class WebHttpBehaviorEx : WebHttpBehavior
    {
        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // clear default erro handlers.
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();

            // add our own error handler.
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new GlobalErrorHandler());
        }
    }

    [DataContract]
    public class Fault
    {
        [DataMember]
        public string Error { get; set; }        
    }
}