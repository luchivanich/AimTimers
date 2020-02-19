using System;
using System.Collections.Generic;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Services;
using AimTimers.Utils;
using Moq;
using Xunit;

namespace AimTimersTests
{
    public class AimTimerNotificationServiceTests
    {
        //[Fact]
        //public void TimerDoesNotStartWithoutItems()
        //{
        //    var dateTimeProvider = new Mock<IDateTimeProvider>();

        //    var aimTimerService = new Mock<IAimTimerService>();
        //    aimTimerService.Setup(s => s.GetActiveAimTimers()).Returns(new List<IAimTimer>());

        //    var timer = new Mock<ITimer>();

        //    var aimTimerNotificationService = new AimTimerNotificationService(dateTimeProvider.Object, aimTimerService.Object, timer.Object);

        //    aimTimerNotificationService.Start();

        //    timer.Verify(t => t.Start(), Times.Never);
        //}

        //[Fact]
        //public void TimerIntervalSetCorrectlyForOneTimerUntilItsEnd()
        //{
        //    var expectedResultMilliseconds = 10000;
        //    var expectedResultTicks = (new TimeSpan(0, 0, 0, 0, expectedResultMilliseconds)).Ticks;
        //    var now = new DateTime(2020, 1, 26, 1, 0, 0);

        //    var aimTimers = new List<IAimTimer>
        //    {
        //        new AimTimer(
        //            new AimTimerModel
        //            {
        //                Ticks = expectedResultTicks,
        //                AimTimerItemModels = new List<AimTimerItemModel>
        //                {
        //                    new AimTimerItemModel(now.Date, now.Date.AddDays(1).AddTicks(-1))
        //                    {
        //                        AimTimerIntervals = new List<AimTimerIntervalModel>
        //                        {
        //                            new AimTimerIntervalModel
        //                            {
        //                                StartDate = now,
        //                                EndDate = null
        //                            }
        //                        }
        //                    }
        //                }
        //            })
        //    };

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(p => p.GetNow()).Returns(now);

        //    var aimTimerService = new Mock<IAimTimerService>();
        //    aimTimerService.Setup(s => s.GetActiveAimTimers()).Returns(aimTimers);

        //    var timer = new Mock<ITimer>();

        //    var aimTimerNotificationService = new AimTimerNotificationService(dateTimeProvider.Object, aimTimerService.Object, timer.Object);

        //    aimTimerNotificationService.Start();

        //    timer.VerifySet(t => t.Interval = expectedResultMilliseconds);
        //}

        //[Fact]
        //public void TimerIntervalSetCorrectlyForTwoTimersUntilItsEnd()
        //{
        //    var expectedResultMilliseconds = 10000;
        //    var expectedResultTicks = (new TimeSpan(0, 0, 0, 0, expectedResultMilliseconds)).Ticks;
        //    var now = new DateTime(2020, 1, 26, 1, 0, 0);

        //    var aimTimers = new List<AimTimer>
        //    {
        //        new AimTimer
        //        {
        //            Ticks = expectedResultTicks,
        //            AimTimerItems = new List<AimTimerItem>
        //            {
        //                new AimTimerItem(now.Date, now.Date.AddDays(1).AddTicks(-1))
        //                {
        //                    AimTimerIntervals = new List<AimTimerInterval>
        //                    {
        //                        new AimTimerInterval
        //                        {
        //                            StartDate = now,
        //                            EndDate = null
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    };

        //    var dateTimeProvider = new Mock<IDateTimeProvider>();
        //    dateTimeProvider.Setup(p => p.GetNow()).Returns(now);

        //    var aimTimerService = new Mock<IAimTimerService>();
        //    aimTimerService.Setup(s => s.GetActiveAimTimers()).Returns(aimTimers);

        //    var timer = new Mock<ITimer>();

        //    var aimTimerNotificationService = new AimTimerNotificationService(dateTimeProvider.Object, aimTimerService.Object, timer.Object);

        //    aimTimerNotificationService.Start();

        //    timer.VerifySet(t => t.Interval = expectedResultMilliseconds);
        //}
    }
}
