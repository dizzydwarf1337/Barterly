using Application.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PromotionService : IPromotionService
    {
        public Task BuyPromotion(Guid ProductId, PostPromotionType postPromotionType, DateTime until)
        {
            throw new NotImplementedException();
        }

        public Task CancelPromotion(Guid PromotionId)
        {
            throw new NotImplementedException();
        }
    }
}
