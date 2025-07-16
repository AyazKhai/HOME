using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOME.DOMAIN.Enums
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EquipmentType Type { get; set; }
        public Status Status { get; set; }
        public Equipment()
        {
        }

        public Equipment(Equipment other)
        {
            if (other == null) return;

            Id = other.Id;
            Name = other.Name;
            Type = other.Type;
            Status = other.Status;
        }
    }
}
