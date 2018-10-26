namespace wcfService1.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ResponseData
    {
        [DataMember(Order = 0)]
        public string token { get; set; }
        [DataMember(Order = 1)]
        public bool authenticated { get; set; }
        [DataMember(Order = 2)]
        public string employeeId { get; set; }
        [DataMember(Order = 3)]
        public string firstname { get; set; }

        [DataMember(Order = 8)]
        public DateTime timestamp { get; set; }
        [DataMember(Order = 9)]
        public string userName { get; set; }
    }
}