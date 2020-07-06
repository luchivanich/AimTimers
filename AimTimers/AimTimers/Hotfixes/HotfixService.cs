using System.Collections.Generic;
using System.Linq;
using AimTimers.Repository;

namespace AimTimers.Hotfixes
{
    public class HotfixService : IHotfixService
    {
        private readonly IRepository _hotfixRepository;
        private readonly IRepository _aimTimerRepository;

        private List<IHotfix> _hotfixes;

        public HotfixService(IRepository hotfixRepository, IRepository aimTimerRepository)
        {
            _hotfixRepository = hotfixRepository;
            _aimTimerRepository = aimTimerRepository;
        }

        public void Init()
        {
            _hotfixes = new List<IHotfix>
            {
                new Hotfix_01_SeparationTimerAndTimerItem(_aimTimerRepository)
            };
        }

        public void ApplyHotfixes()
        {
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
