using System;
using Auto.Messages;

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

public class NewOwnerRegMessage : NewOwnerMessage
{
    public NewOwnerRegMessage(){}

    public NewOwnerRegMessage(NewOwnerMessage owner)
    {
        this.Name = owner.Name;
        this.NumberPhone = owner.NumberPhone;
        this.Address = owner.Address;
        this.RegistrationCode = owner.RegistrationCode;
    }
}


