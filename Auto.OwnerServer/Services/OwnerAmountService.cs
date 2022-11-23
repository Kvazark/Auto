using System;
using System.Threading.Tasks;
using Auto.OwnerEngine;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Auto.OwnerServer.Services
{
    public class OwnerAmountService : Amounter.AmounterBase{
            private readonly ILogger<OwnerAmountService> logger;
            public OwnerAmountService(ILogger<OwnerAmountService> logger) {
                this.logger = logger;
            }
        
            public override Task<AmountReply> GetAmount(AmountRequest request, ServerCallContext context) {
                Random rnd = new Random();
                int value = rnd.Next(1, 10);
                return Task.FromResult(new AmountReply() { CurrencyCode = "units", Amount = value });
            }
        }
    }
