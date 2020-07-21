using System;
using System.Collections.Generic;
using System.Linq;
using AimTimers.Bl;
using AimTimers.Models;
using AimTimers.Repository;
using AimTimers.Utils;

namespace AimTimers.Hotfixes
{
    public class HotfixService : IHotfixService
    {
        private readonly IRepository _hotfixRepository;
        private readonly IRepository _aimTimerRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Func<AimTimerModel, IAimTimer> _aimTimerFactory;
        private readonly Func<IAimTimer, AimTimerItemModel, IAimTimerItem> _aimTimerItemFactory;

        private List<IHotfix> _hotfixes;

        public HotfixService(
            IRepository hotfixRepository,
            IRepository aimTimerRepository,
            IDateTimeProvider dateTimeProvider,
            Func<AimTimerModel, IAimTimer> aimTimerFactory,
            Func<IAimTimer, AimTimerItemModel, IAimTimerItem> aimTimerItemFactory)
        {
            _hotfixRepository = hotfixRepository;
            _aimTimerRepository = aimTimerRepository;
            _dateTimeProvider = dateTimeProvider;
            _aimTimerFactory = aimTimerFactory;
            _aimTimerItemFactory = aimTimerItemFactory;
        }

        public void Init()
        {
            _hotfixes = new List<IHotfix>
            {
                new Hotfix_01_SeparationTimerAndTimerItem(_aimTimerRepository),
                new Hotfix_02_SetOriginDateAndIndexes(_aimTimerRepository, _dateTimeProvider, _aimTimerFactory, _aimTimerItemFactory)
            };
        }

        public void ApplyHotfixes()
        {
            //_hotfixRepository.Delete("Hotfix_02_SetOriginDateAndIndexes");

            var allAppliedHotfixes = _hotfixRepository.LoadAll<HotfixModel>();

            foreach (var hotfix in _hotfixes)
            {
                if (allAppliedHotfixes.All(i => i.Id != hotfix.HotfixId))
                {
                    hotfix.Apply();
                    _hotfixRepository.Save(new HotfixModel { Id = hotfix.HotfixId });
                }
            }
        }
    }
}
