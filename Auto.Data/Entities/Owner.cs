using System.Collections.Generic;

namespace Auto.Data.Entities
{
    public class Owner
    {
        public string Name { get; set; }
        
        public string NumberPhone { get; set; }
        
        public string Address { get; set; }
        
        public string RegistrationCode { get; set; }
        
        public virtual Vehicle VehicleCode { get; set; }
       
    }
}