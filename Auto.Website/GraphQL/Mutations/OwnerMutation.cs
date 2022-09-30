using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Mutations
{
    public class OwnerMutation : ObjectGraphType
    {
        private readonly IAutoDatabase _db;

        public OwnerMutation(IAutoDatabase db)
        {
            this._db = db;

            Field<OwnerGraphType>(
                "createOwner",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "name"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "numberPhone"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "address"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "registrationCode"}
                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    var numberPhone = context.GetArgument<string>("numberPhone");
                    var address = context.GetArgument<string>("address");
                    var registrationCode = context.GetArgument<string>("registrationCode");

                    var vehicleCode = db.FindVehicle(registrationCode);
                    var owner = new Owner
                    {
                        Name = name,
                        NumberPhone = numberPhone,
                        Address = address,
                        VehicleCode = vehicleCode,
                        RegistrationCode = vehicleCode.Registration
                    };
                    _db.CreateOwner(owner);
                    return owner;
                }
            );
        }
    }
}