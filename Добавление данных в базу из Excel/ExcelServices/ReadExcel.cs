using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Добавление_данных_в_базу_из_Excel.Models;
using Добавление_данных_в_базу_из_Excel.Data;

namespace FillQualityDb.ExcelServices
{
    internal static class ReadExcel
    {
        public static List<StandartQualityFilm> GetStandartsQualityFromExcel(ExcelWorksheet sheet)
        {
            var result = new List<StandartQualityFilm>();
            using var db = new QualityV3Context();
            for (int row = 2; row < 160; row++)
            {
                var films = db.Films
                    .Where(x => x.Mark == (string)sheet.Cells[row, 1].Value
                    && x.Thickness == Convert.ToInt32(sheet.Cells[row, 2].Value)).ToArray();
                for (int j = 0; j < films.Length; j++)
                {
                    var standart = new StandartQualityFilm
                    {
                        FilmId = films[j].FilmId,
                        ThicknessVariation = Convert.ToDouble(sheet.Cells[row, 3].Value),
                        TensileStrengthMd = Convert.ToDouble(sheet.Cells[row, 4].Value),
                        TensileStrengthTd = Convert.ToDouble(sheet.Cells[row, 5].Value),
                        ElongationAtBreakMd = Convert.ToInt32(sheet.Cells[row, 6].Value),
                        ElongationAtBreakTd = Convert.ToInt32(sheet.Cells[row, 7].Value),
                        CoefficientOfFrictionS = Convert.ToDouble(sheet.Cells[row, 8].Value),
                        CoefficientOfFrictionD = Convert.ToDouble(sheet.Cells[row, 9].Value),
                        LightTransmission = films[j].Color == "белый" ? Convert.ToInt32(sheet.Cells[row, 10].Value) : 0,
                        CoronaTreatment = 38,
                        StandartQualityTitleId = 1
                    };
                    result.Add(standart);
                }
            }
            return result;
        }
    public static List<Film> GetFilmsFromExcel(ExcelWorksheet sheet)
        {
            var films = new List<Film>();
            for (int row = 2; row < 160; row++)
            {
                var filmTransperent = new Film(); var filmWhite = new Film();
                filmTransperent.Mark = filmWhite.Mark = (string)sheet.Cells[row, 1].Value;
                filmTransperent.Thickness = filmWhite.Thickness = Convert.ToInt32(sheet.Cells[row, 2].Value);
                filmTransperent.Color = "прозрачный";
                filmWhite.Color = "белый";
                filmTransperent.Density = 0.92;
                filmWhite.Density = Convert.ToDouble(sheet.Cells[row, 13].Value);
                films.Add(filmTransperent);
                films.Add(filmWhite);
            }
            return films;
        }

        public static void AddStandartQualityToDatabase(List<StandartQualityFilm> standartQualities)
        {
            using var db = new QualityV3Context();
            db.StandartQualityFilms.AddRange(standartQualities);
            db.SaveChanges();
        }
        public static void AddFilmsToDatabase(List<Film> films)
        {
            using var db = new QualityV3Context();
            {
                var filmsFromDb = db.Films.ToArray();
                foreach (var film in films)
                {
                    if (!filmsFromDb.Contains(film))
                    {
                        db.Films.Add(film);
                        Console.WriteLine(film.ToString());
                    }
                }
                db.SaveChanges();
            }
        }

        public static ExcelWorksheet GetExcelSheet(string filePath, string key)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var data = new ExcelPackage(filePath);
            var sheet = data.Workbook.Worksheets[key];
            return sheet;
        }
    }
}
