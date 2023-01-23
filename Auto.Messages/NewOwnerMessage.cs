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

public class NewOwnerVecMessage : NewOwnerMessage
{
    public string Color { get; set; }
    public string Model { get; set; }
    public NewOwnerVecMessage(){}

    public NewOwnerVecMessage(NewOwnerMessage owner, string Color, string Model)
    {
        this.Name = owner.Name;
        this.NumberPhone = owner.NumberPhone;
        this.Address = owner.Address;
        this.RegistrationCode = owner.RegistrationCode;
        this.Color = Color;
        this.Model = Model;
    }
}
// public class NewOwnerRegMessage : NewOwnerMessage
// {
//     public NewOwnerRegMessage(){}
//
//     public NewOwnerRegMessage(NewOwnerMessage owner)
//     {
//         this.Name = owner.Name;
//         this.NumberPhone = owner.NumberPhone;
//         this.Address = owner.Address;
//         this.RegistrationCode = owner.RegistrationCode;
//     }
// }


