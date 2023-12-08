using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class Film
    {
        public Film()
        {
            OrdersQualities = new HashSet<OrdersQuality>();
            StandartQualityFilms = new HashSet<StandartQualityFilm>();
        }

        public int FilmId { get; set; }
        public string Mark { get; set; } = null!;
        public int Thickness { get; set; }
        public string Color { get; set; } = null!;
        public double Density { get; set; }

        public virtual ICollection<OrdersQuality> OrdersQualities { get; set; }
        public virtual ICollection<StandartQualityFilm> StandartQualityFilms { get; set; }
    }
}
