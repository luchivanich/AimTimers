using System;

namespace AimTimers.Models
{
    public class AimTimerIntervalModel : BaseModel, IModel
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
