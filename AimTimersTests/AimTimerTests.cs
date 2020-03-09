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
        public void StartMethod_Creates_AimTimerItem_With_Correct_AimTimerInterval()
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
        public void StartMethod_Continues_Existing_AimTimerItem()
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
    }
}
