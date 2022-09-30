using System;
using System.Collections.Generic;
using System.Linq;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Components.Web;

namespace Auto.Website.GraphQL.Queries
{
    public class OwnerQuery : ObjectGraphType
    {
        private readonly IAutoDatabase _db;

        public OwnerQuery(IAutoDatabase db)
        {
            this._db = db;

            Field<ListGraphType<OwnerGraphType>>("Owners", "Query to retrieve all Owners",
                resolve: GetAllOwners);

            Field<OwnerGraphType>("Owner", "Query to retrieve a specific Owner",
                new QueryArguments(MakeNonNullStringArgument("name",
                    "The full name of the Owner")),
                resolve: GetOwner);

            Field<ListGraphType<OwnerGraphType>>("OwnersByManufacturerCode",
                "Query to retrieve all Owners matching the specified model code",
                new QueryArguments(MakeNonNullStringArgument("nameModel", "The name of a model, eg 'kia-rio', 'volkswagen-golf'")),
                resolve: GetOwnersByManufacturerCode);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description)
        {
            return new QueryArgument<NonNullGraphType<StringGraphType>>
            {
                Name = name, Description = description
            };
        }

        private IEnumerable<Owner> GetAllOwners(IResolveFieldContext<object> context) => _db.ListOwners();

        private Owner GetOwner(IResolveFieldContext<object> context)
        {
            var name = context.GetArgument<string>("name");
            return _db.FindOwner(name);
        }

        private IEnumerable<Owner> GetOwnersByManufacturerCode(IResolveFieldContext<object> context)
        {
            var manufName = context.GetArgument<string>("nameModel");
            var owners =
                _db.ListOwners()
                    .Join(_db.ListVehicles(), o => o.RegistrationCode, v => v.Registration,
                        (owner, vehicle) => new
                        {
                            Owner = owner,
                            Vehicle = vehicle
                        }
                    )
                    .Join(_db.ListModels(), ownerVehicle => ownerVehicle.Vehicle.ModelCode, m => m.Code,
                        (ownerVehicle, model) => new
                        {
                            Owner = ownerVehicle.Owner,
                            Vehicle = ownerVehicle.Vehicle,
                            Model = model
                        }
                    )
                    .Where(join => join.Model.ManufacturerCode == manufName)
                    .Select(join => join.Owner)
                    .ToList();

            return owners;
        }
    }
}