namespace wcfService1
{
    using Jose;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using wcfService1.Database;
    using wcfService1.Models;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private string secureToken;
        public ResponseData Login(RequestData data)
        {
            try
            {
                NorthwndDBContext dc = new NorthwndDBContext();
                secureToken = GetJwt(data.usname, data.pwd);
                // Esta es una forma mas practica de usar correctamente el Linq con expresiones landa
                var employee = dc.Employees.SingleOrDefault(e => e.EmployeeID.ToString().Equals(data.pwd));

                //Esta forma es la antigua
                //var employee = (from Employees in dc.Employees where Employees.EmployeeID.ToString() == data.pwd select Employees).First();
                var response = new ResponseData
                {
                    token = secureToken,
                    authenticated = true,
                    employeeId = employee.EmployeeID.ToString(),
                    firstname = employee.LastName,
                    timestamp = DateTime.Now,
                    userName = data.usname
                };

                return response;

            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        // This function is for decoding string to 
        private byte[] Base64UrlDecode(string arg)   
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding  
            s = s.Replace('_', '/'); // 63rd char of encoding  
            switch (s.Length % 4) // Pad with trailing '='s  
            {
                case 0: break; // No pad chars in this case  
                case 2: s += "=="; break; // Two pad chars  
                case 3: s += "="; break; // One pad char  
                default:
                    throw new System.Exception(
                "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder  
        }
        private long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        public string GetJwt(string user, string pass) //function for JWT Token  
        {
            byte[] secretKey = Base64UrlDecode("Hi");//pass key to secure and decode it  
            DateTime issued = DateTime.Now;
            var User = new Dictionary<string, object>()
                    {
                        {"user", user},
                        {"pass", pass},

                         {"iat", ToUnixTime(issued).ToString()}
                    };

            string token = JWT.Encode(User, secretKey, JwsAlgorithm.HS256);
            return token;
        }
    }
}
