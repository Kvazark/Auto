using System;

namespace Auto.Messages
{

    public class NewOwnerMessage
    {
        public string Name { get; set; }
        public string NumberPhone { get; set; }
        public string Address { get; set; }
        public string RegistrationCode { get; set; }
        public DateTime ListedAtUtc { get; set; }
    }
}