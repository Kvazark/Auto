using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes
{
    public sealed class OwnerGraphType : ObjectGraphType<Owner> {
        public OwnerGraphType() {
            Name = "owner";
            Field(c => c.Name);
            Field(c => c.NumberPhone);
            Field(c => c.Address);
            Field(c => c.VehicleCode, nullable: false,type: typeof(VehicleGraphType)).Description("Owner's car registration number");
        }
    }
}