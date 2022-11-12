using System;
using System.Threading.Tasks;
using Auto.AmounterEngine;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Auto.AmounterServer.Services
{
    public class AmounterService : Amounter.AmounterBase {
        private readonly ILogger<AmounterService> logger;
        public AmounterService(ILogger<AmounterService> logger) {
            this.logger = logger;
        }
        public override Task<AmountReply> GetAmount(AmountRequest request, ServerCallContext context) {
            Random rnd = new Random();
            int value = rnd.Next(1, 10);
            return Task.FromResult(new AmountReply() { CurrencyCode = "units", Amount = value });
        }
    }
}