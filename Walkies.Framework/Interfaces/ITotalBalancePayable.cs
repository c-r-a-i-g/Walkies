using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies.Framework.Interfaces
{
    public interface ITotalBalancePayable
    {
        decimal? TotalPriceBeforeRegistration { get; set; }
        decimal? StampDuty { get; set; }
        decimal? RegistrationCost { get; set; }
        decimal? CTPCost { get; set; }
        decimal? TradeInPayout { get; set; }
    }
}
