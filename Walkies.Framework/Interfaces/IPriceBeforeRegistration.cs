using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Interfaces
{
    public interface IPriceBeforeRegistration
    {
        decimal? VehicleSalePrice { get; }
        decimal? TotalAccessoriesAndOptions { get; }
        decimal? DealerCharges { get; }
        decimal? FuelCost { get; }
        decimal? DealerDiscount { get; }
    }
}
