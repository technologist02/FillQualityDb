using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class StandartQualityTitle
    {
        public StandartQualityTitle()
        {
            OrdersQualities = new HashSet<OrdersQuality>();
            StandartQualityFilms = new HashSet<StandartQualityFilm>();
        }

        public int StandartQualityTitleId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<OrdersQuality> OrdersQualities { get; set; }
        public virtual ICollection<StandartQualityFilm> StandartQualityFilms { get; set; }
    }
}
