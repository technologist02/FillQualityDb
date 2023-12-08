using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class StandartQualityFilm
    {
        public int StandartQualityFilmId { get; set; }
        public int FilmId { get; set; }
        public double ThicknessVariation { get; set; }
        public double TensileStrengthMd { get; set; }
        public double TensileStrengthTd { get; set; }
        public int ElongationAtBreakMd { get; set; }
        public int ElongationAtBreakTd { get; set; }
        public double CoefficientOfFrictionS { get; set; }
        public double CoefficientOfFrictionD { get; set; }
        public int? LightTransmission { get; set; }
        public int CoronaTreatment { get; set; }
        public int StandartQualityTitleId { get; set; }

        public virtual Film Film { get; set; } = null!;
        public virtual StandartQualityTitle StandartQualityTitle { get; set; } = null!;
    }
}
