using System.Threading.Tasks;
using Auto.Data;
using Auto.InfoOwnerServer;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Auto.OwnerService.Services
{
    public class OwnerInfoService : OwnerInfo.OwnerInfoBase
    {
        private readonly ILogger<OwnerInfoService> _logger;
        private readonly IAutoDatabase _db;

        public OwnerInfoService(ILogger<OwnerInfoService> logger, IAutoDatabase db)
        {
            _logger = logger;
            _db = db;
        }

        public override Task<OwnerInfoReply> GetOwnerInfo(OwnerInfoRequest request, ServerCallContext context)
        {
            var owner = _db.FindOwnerByNumberPhone(request.Number);
            return Task.FromResult(new OwnerInfoReply()
                {Name = owner.Name, Address = owner.Address, Registration = owner.VehicleCode.Registration});
        }
    }
}