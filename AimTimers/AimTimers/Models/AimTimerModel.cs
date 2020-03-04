using System.Collections.Generic;

namespace AimTimers.Models
{
    public class AimTimerModel : IModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? Ticks { get; set; }
        public List<AimTimerItemModel> AimTimerItemModels { get; set; } = new List<AimTimerItemModel>();
    }
}