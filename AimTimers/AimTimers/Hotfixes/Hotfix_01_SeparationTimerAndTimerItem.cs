using AimTimers.Models;
using AimTimers.Repository;

namespace AimTimers.Hotfixes
{
    public class Hotfix_01_SeparationTimerAndTimerItem : IHotfix
    {
        public string HotfixId => "Hotfix_01_SeparationTimerAndTimerItem";

        private readonly IRepository _repository;

        public Hotfix_01_SeparationTimerAndTimerItem(IRepository repository)
        {
            _repository = repository;
        }

        public void Apply()
        {
            var aimTimerModels = _repository.LoadAll<AimTimerModel>();
            foreach(var aimTimerModel in aimTimerModels)
            {
                foreach(var aimTimerItemModel in aimTimerModel.AimTimerItemModels)
                {
                    aimTimerItemModel.AimTimerId = aimTimerModel.Id;
                    aimTimerItemModel.Ticks = aimTimerModel.Ticks;
                    _repository.Save(aimTimerItemModel);
                }
                aimTimerModel.AimTimerItemModels.Clear();
                _repository.Save(aimTimerModel);
            }
        }
    }
}
