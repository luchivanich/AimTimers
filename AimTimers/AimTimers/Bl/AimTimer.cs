using System;
using AimTimers.Models;

namespace AimTimers.Bl
{
    public class AimTimer : IAimTimer
    {
        private AimTimerModel _aimTimerModel;

        public string Title { get; set; }

        public long Ticks { get; set; }

        public DateTime OriginDate { get; set; }

        public AimTimer(AimTimerModel aimTimerModel)
        {
            _aimTimerModel = aimTimerModel;
        }

        public void Init()
        {
            Title = _aimTimerModel.Title;
            Ticks = _aimTimerModel.Ticks;
            OriginDate = _aimTimerModel.OriginDate;
        }

        public AimTimerModel GetAimTimerModel()
        {
            return new AimTimerModel
            {
                Id = _aimTimerModel.Id,
                Title = Title,
                Ticks = Ticks,
                OriginDate = OriginDate,
            };
        }
    }
}
