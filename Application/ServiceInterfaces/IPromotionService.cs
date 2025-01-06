using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IPromotionService
    {
        Task BuyPromotion(Guid ProductId, PostPromotionType postPromotionType, DateTime until);
        Task CancelPromotion(Guid PromotionId);
    }
}
