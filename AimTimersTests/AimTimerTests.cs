using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Utils;
using Moq;
using Xunit;

namespace AimTimersTests
{
    public class AimTimerTests
    {
        [Fact]
        public void Start_Method_Creates_AimTimerItem_With_Correct_AimTimerInterval()
        {
            var now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.GetNow()).Returns(now);
            var aimTimerModel = new AimTimerModel();
            var aimTimer = new AimTimer(aimTimerModel, dateTimeProvider.Object);

            aimTimer.Start();

            Assert.Single(aimTimerModel.AimTimerItemModels);
            var intervals = aimTimerModel.AimTimerItemModels.Single().AimTimerIntervals;
            Assert.Single(intervals);
            Assert.Equal(now, intervals.Single().StartDate);
            Assert.Null(intervals.Single().EndDate);
        }

        [Fact]
        public void Start_Method_Continues_Existing_AimTimerItem()
        {
            var now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.GetNow()).Returns(now);

            var aimTimerItemModel = new AimTimerItemModel(now.AddHours(-1), now.AddHours(1));

            var aimTimerModel = new AimTimerModel
            {
                AimTimerItemModels = new List<AimTimerItemModel> { aimTimerItemModel }
            };

            var aimTimer = new AimTimer(aimTimerModel, dateTimeProvider.Object);

            aimTimer.Start();

            Assert.Single(aimTimerModel.AimTimerItemModels);
            Assert.Equal(aimTimerItemModel, aimTimerModel.AimTimerItemModels.Single());
            Assert.Single(aimTimerItemModel.AimTimerIntervals);
            Assert.Equal(now, aimTimerItemModel.AimTimerIntervals.Single().StartDate);
            Assert.Null(aimTimerItemModel.AimTimerIntervals.Single().EndDate);
        }

        [Fact]
        public void Start_Method_Does_Nothing_When_Timer_Is_Run()
        {
            var now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.GetNow()).Returns(now);

            var aimTimerIntervalModel = new AimTimerIntervalModel { StartDate = now };

            var aimTimerItemModel = new AimTimerItemModel(now.AddHours(-2), now.AddHours(2))
            {
                AimTimerIntervals = new List<AimTimerIntervalModel> { aimTimerIntervalModel }
            };

            var aimTimerModel = new AimTimerModel
            {
                AimTimerItemModels = new List<AimTimerItemModel> { aimTimerItemModel }
            };

            var aimTimer = new AimTimer(aimTimerModel, dateTimeProvider.Object);

            aimTimer.Start();

            Assert.Single(aimTimerModel.AimTimerItemModels);
            Assert.Equal(aimTimerItemModel, aimTimerModel.AimTimerItemModels.Single());
            Assert.Single(aimTimerItemModel.AimTimerIntervals);
            Assert.Equal(now, aimTimerItemModel.AimTimerIntervals.Single().StartDate);
            Assert.Single(aimTimerItemModel.AimTimerIntervals);
            Assert.Equal(aimTimerIntervalModel, aimTimerItemModel.AimTimerIntervals.Single());
            Assert.Null(aimTimerItemModel.AimTimerIntervals.Single().EndDate);
        }

        [Fact]
        public void Stop_Method_Sets_EndTime_For_Current_Interval()
        {
            var now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.GetNow()).Returns(now);

            var aimTimerIntervalModel = new AimTimerIntervalModel { StartDate = now.AddHours(-1), EndDate = null };
            var aimTimerItemModel = new AimTimerItemModel(now.AddHours(-2), now.AddHours(1))
            {
                AimTimerIntervals = new List<AimTimerIntervalModel> { aimTimerIntervalModel }
            };
            var aimTimerModel = new AimTimerModel
            {
                AimTimerItemModels = new List<AimTimerItemModel> { aimTimerItemModel }
            };

            var aimTimer = new AimTimer(aimTimerModel, dateTimeProvider.Object);

            aimTimer.Stop();

            Assert.Single(aimTimerItemModel.AimTimerIntervals);
            Assert.Equal(now, aimTimerIntervalModel.EndDate);
        }

        [Fact]
        public void Stop_Method_Does_Nothing_When_Timer_Is_Stopped()
        {
            var now = DateTime.Now;
            var expectedEndDate = now.AddHours(1);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.GetNow()).Returns(now);

            var aimTimerIntervalModel = new AimTimerIntervalModel { StartDate = now.AddHours(-1), EndDate = expectedEndDate };
            var aimTimerItemModel = new AimTimerItemModel(now.AddHours(-2), now.AddHours(2))
            {
                AimTimerIntervals = new List<AimTimerIntervalModel> { aimTimerIntervalModel }
            };
            var aimTimerModel = new AimTimerModel
            {
                AimTimerItemModels = new List<AimTimerItemModel> { aimTimerItemModel }
            };

            var aimTimer = new AimTimer(aimTimerModel, dateTimeProvider.Object);

            aimTimer.Stop();

            Assert.Single(aimTimerItemModel.AimTimerIntervals);
            Assert.Equal(expectedEndDate, aimTimerIntervalModel.EndDate);
        }

        [Fact]
        public void RefreshTimeLeft_Method_Set_TimeLeft_Equals_Initial_Time_When_Timer_Is_Not_Started()
        {
            var time = new TimeSpan(0, 10, 0);
            var now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.GetNow()).Returns(now);

            var aimTimerModel = new AimTimerModel
            {
                Ticks = time.Ticks
            };

            var aimTimer = new AimTimer(aimTimerModel, dateTimeProvider.Object);
            aimTimer.RefreshTimeLeft();

            Assert.Equal(time, aimTimer.TimeLeft);
        }
    }
}
