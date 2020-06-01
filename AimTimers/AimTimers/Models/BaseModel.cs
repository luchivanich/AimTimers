using System;

namespace AimTimers.Models
{
    public class BaseModel : IModel
    {
        public string Id { get; set; }

        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
