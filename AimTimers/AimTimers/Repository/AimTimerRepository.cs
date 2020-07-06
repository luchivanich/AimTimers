using System;
using System.Collections.Generic;
using System.Text;

namespace AimTimers.Repository
{
    public class AimTimerRepository : BaseRepository
    {
        protected override string GetRepositoryName()
        {
            return "AimTimer";
        }
    }
}
