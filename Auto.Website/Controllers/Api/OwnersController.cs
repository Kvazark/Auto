using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Messages;
using Auto.Website.Models;
using Castle.Core.Internal;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;


namespace Auto.Website.Controllers.Api {
    [Route("api/[controller]")]
	[ApiController]
	public class OwnersController : ControllerBase {
		private readonly IAutoDatabase db;
		private readonly IBus bus;

		public OwnersController(IAutoDatabase db, IBus bus) {
			this.db = db;
			this.bus = bus;
		}

		private dynamic Paginate(string url, int index, int count, int total) {
			dynamic links = new ExpandoObject();
			links.self = new { href = url };
			links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
			links.first = new { href = $"{url}?index=0&count={count}" };
			if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
			if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
			return links;
		}

		// GET: api/owners
		[HttpGet]
		[Produces("application/hal+json")]
		public IActionResult Get(int index = 0, int count = 10) {
			var items = db.ListOwners().Skip(index).Take(count);
			var total = db.CountOwners();
			var _links = Paginate("/api/owners", index, count, total);
			var _actions = new {
				create = new {
					method = "POST",
					type = "application/json",
					name = "Create a new owner",
					href = "/api/owners"
				},
				delete = new {
					method = "DELETE",
					name = "Delete the owner",
					href = "/api/owners/{id}"
				}
			};
			var result = new {
				_links, _actions, index, count, total, items
			};
			return Ok(result);
		}

		// GET api/owners/Full name
		[HttpGet("{name}")]
		public IActionResult Get(string name) {
			var owner = db.FindOwner(name);
			if (owner == default) return NotFound();
			var json = owner.ToDynamic();
			json._links = new {
				self = new { href = $"/api/owners/{name}" }
			};
			json._actions = new {
				update = new {
					method = "PUT",
					href = $"/api/owners/{name}",
					accept = "application/json"
				},
				delete = new {
					method = "DELETE",
					href = $"/api/owners/{name}"
				}
			};
			return Ok(json);
		}
		// POST api/owners
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] OwnerDto dto) {
			var vehicleCode = db.FindVehicle(dto.RegistrationCode);
			var owner = new Owner {
				Name = dto.Name,
				NumberPhone = dto.NumberPhone,
				Address = dto.Address,
				VehicleCode = vehicleCode
			};
			db.CreateOwner(owner);
			PublishNewOwnerMessage(owner);
			return Ok(dto);
		}
		private void PublishNewOwnerMessage(Owner owner) {
			var message = new NewOwnerMessage() {
				Name = owner.Name,
				NumberPhone = owner.NumberPhone,
				Address = owner.Address,
				RegistrationCode = owner?.VehicleCode?.Registration,
				ListedAtUtc = DateTime.UtcNow
			};
			bus.PubSub.Publish(message);
		}

		

		// PUT api/owners/Fuul name
		[HttpPut("{name}")]
		public IActionResult Put(string name, [FromBody] dynamic dto) {
			var vehicleCodeHref = dto._links.vehicleCode.href;
			var vehicleCodeId = VehiclesController.ParseVehicleId(vehicleCodeHref);
			var vehicleCode = db.FindVehicle(vehicleCodeId);
			
			var owner = new Owner {
				Name = name,
				NumberPhone = dto.numberPhone,
				Address = dto.address,
				RegistrationCode = vehicleCode.Registration,
			};
			db.UpdateOwner(owner);
			return Get(name);
		}

		// DELETE api/owners/Full name
		[HttpDelete("{name}")]
		public IActionResult Delete(string name) {
			var owner = db.FindOwner(name);
			if (owner == default) return NotFound();
			db.DeleteOwner(owner);
			return NoContent();
		}
		
		
		public static string ParseOwnerName(dynamic href) {
			var tokens = ((string)href).Split("/");
			return tokens.Last();
		}
	}
}
