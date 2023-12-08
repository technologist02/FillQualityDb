using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class Extruder
    {
        public Extruder()
        {
            OrdersQualities = new HashSet<OrdersQuality>();
        }

        public int ExtruderId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<OrdersQuality> OrdersQualities { get; set; }
    }
}
