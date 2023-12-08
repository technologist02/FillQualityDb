using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class OrdersQuality
    {
        public int OrderQualityId { get; set; }
        public int OrderNumber { get; set; }
        public string? Customer { get; set; }
        public DateOnly ProductionDate { get; set; }
        public int BrigadeNumber { get; set; }
        public int RollNumber { get; set; }
        public int ExtruderId { get; set; }
        public int FilmId { get; set; }
        public int Width { get; set; }
        public int MinThickness { get; set; }
        public int MaxThickness { get; set; }
        public double TensileStrengthMd { get; set; }
        public double TensileStrengthTd { get; set; }
        public int ElongationAtBreakMd { get; set; }
        public int ElongationAtBreakTd { get; set; }
        public double CoefficientOfFrictionS { get; set; }
        public double CoefficientOfFrictionD { get; set; }
        public int LightTransmission { get; set; }
        public int CoronaTreatment { get; set; }
        public int StandartQualityTitleId { get; set; }
        public DateTime CreationDate { get; set; }
        public int InspectorId { get; set; }
        public bool IsInspected { get; set; }
        public bool? IsQualityMatches { get; set; }

        public virtual Extruder Extruder { get; set; } = null!;
        public virtual Film Film { get; set; } = null!;
        public virtual User Inspector { get; set; } = null!;
        public virtual StandartQualityTitle StandartQualityTitle { get; set; } = null!;
    }
}
