using System.Collections.Generic;

namespace AimTimers.Models
{
    public class AimTimerModel : BaseModel, IModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long? Ticks { get; set; }
        public List<AimTimerItemModel> AimTimerItemModels { get; set; } = new List<AimTimerItemModel>();
    }
}