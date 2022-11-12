using System.Threading.Tasks;
using Auto.PricingEngine;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Auto.PricingServer.Services
{
    public class PricerService : Pricer.PricerBase {
        private readonly ILogger<PricerService> logger;
        public PricerService(ILogger<PricerService> logger) {
            this.logger = logger;
        }
        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            return Task.FromResult(new PriceReply() { CurrencyCode = "RUB", Price = 400 });
        }
    }
}