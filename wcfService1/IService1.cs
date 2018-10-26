namespace wcfService1
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using wcfService1.Models;

    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)] //Post Method for gtting data according to the parameter  
        ResponseData Login(RequestData data);
    }
}
