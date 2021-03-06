﻿using System;
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

        public int GetIndexByDate(DateTime date)
        {
            return (int)(date.Date - OriginDate.Date).TotalDays;
        }

        public (DateTime startDate, DateTime endDate) GetPeriodByIndex(int index)
        {
            var date = OriginDate.AddDays(index);
            return (date.Date, date.Date.AddDays(1).AddMilliseconds(-1));
        }
    }
}
