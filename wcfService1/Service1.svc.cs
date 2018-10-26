namespace wcfService1
{
    using Jose;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using wcfService1.Models;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private string secureToken;
        public ResponseData Login(RequestData data)
        {
            using (SqlConnection _con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("sp_LogIn", _con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", data.usname);
                cmd.Parameters.AddWithValue("@Password", data.pwd);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt_Login = new DataSet();
                da.Fill(dt_Login, "table");
                DataRow dr = dt_Login.Tables[0].Rows[0];
                secureToken = GetJwt(data.usname, data.pwd);
                var response = new ResponseData
                {
                    token = secureToken,
                    authenticated = true,
                    employeeId = dr["EmpId"].ToString(),
                    firstname = dr["emp_firstname"].ToString(),
                    timestamp = DateTime.Now,
                    userName = data.usname
                };

                return response;
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
