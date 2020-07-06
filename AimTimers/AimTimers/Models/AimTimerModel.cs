using System;
using System.Collections.Generic;

namespace AimTimers.Models
{
    public class AimTimerModel : BaseModel, IModel
    {
        public string Title { get; set; }
        public long? Ticks { get; set; }
        [Obsolete("Remove after all app instances hotfixed", false)]
        public List<AimTimerItemModel> AimTimerItemModels { get; set; } = new List<AimTimerItemModel>();
        public DateTime OriginDate { get; set; }
    }
}