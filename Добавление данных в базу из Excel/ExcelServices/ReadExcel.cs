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
        public static ExcelWorksheet GetExcelSheet(string filePath, string key)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var data = new ExcelPackage(filePath);
            var sheet = data.Workbook.Worksheets[key];
            return sheet;
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

        public async static Task<List<StandartQualityFilm>> GetStandartsQualityFromExcelAsync(ExcelWorksheet sheet)
        {
            var result = new List<StandartQualityFilm>();
            using var db = new QualityV3Context();
            for (int row = 2; row < 160; row++)
            {
                var films = await db.Films
                    .Where(x => x.Mark == (string)sheet.Cells[row, 1].Value
                    && x.Thickness == Convert.ToInt32(sheet.Cells[row, 2].Value)).ToArrayAsync();
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
    
        public async static Task<List<OrdersQuality>> GetOrdersFromExcelAsync(ExcelWorksheet sheet)
        {

            using var db = new QualityV3Context();
            var list = new List<OrdersQuality>();
            for (int i = 2; i < 1181; i++)
            {
                var film = new Film
                {
                    Mark = Convert.ToString(sheet.Cells[i, 7].Value),
                    Thickness = Convert.ToInt32(sheet.Cells[i, 10].Value),
                    Color = (string)sheet.Cells[i, 8].Value
                };
                var filmDb = await db.Films.SingleOrDefaultAsync(x => x.Mark == film.Mark && x.Thickness == film.Thickness && x.Color == film.Color.ToLower());
                if (filmDb == null) continue;
                var order = new OrdersQuality();
                var extruderDb = await db.Extruders.SingleOrDefaultAsync(x => x.Name == (string)sheet.Cells[i, 6].Value);
                if (filmDb != null) order.FilmId = filmDb.FilmId;
                if (extruderDb != null) order.ExtruderId = extruderDb.ExtruderId;

                string ordNum = Convert.ToString(sheet.Cells[i, 1].Value);
                var temp = string.Join("", ordNum.TakeWhile(x => char.IsDigit(x)));
                if (temp.Length > 0)
                {
                    order.OrderNumber = int.Parse(temp);
                }
                var date = Convert.ToDateTime(sheet.Cells[i, 3].Value);
                order.ProductionDate = new DateOnly(date.Year, date.Month, date.Day);
                order.Customer = (string)sheet.Cells[i, 2].Value;
                order.BrigadeNumber = Convert.ToInt32(sheet.Cells[i, 4].Value);
                //order.ExtruderId = (int)sheet.Cells[i, 6].Value; //найти в базе экструдер

                order.Width = Convert.ToInt32(sheet.Cells[i, 9].Value);
                order.MinThickness = Convert.ToInt32(sheet.Cells[i, 11].Value);
                order.MaxThickness = Convert.ToInt32(sheet.Cells[i, 12].Value);
                order.TensileStrengthMd = Convert.ToDouble(sheet.Cells[i, 14].Value);
                order.TensileStrengthTd = Convert.ToDouble(sheet.Cells[i, 15].Value);
                order.ElongationAtBreakMd = Convert.ToInt32(sheet.Cells[i, 16].Value);
                order.ElongationAtBreakTd = Convert.ToInt32(sheet.Cells[i, 17].Value);
                order.CoefficientOfFrictionS = Convert.ToDouble(sheet.Cells[i, 18].Value);
                order.CoefficientOfFrictionD = Convert.ToDouble(sheet.Cells[i, 19].Value);
                order.CoronaTreatment = Convert.ToInt32(sheet.Cells[i, 22].Value);
                order.LightTransmission = Convert.ToInt32(sheet.Cells[i, 20].Value);
                order.StandartQualityTitleId = 1;
                order.InspectorId = 1;
                order.CreationDate = DateTime.Now.ToUniversalTime();
                list.Add(order);
            }
            return list;
        }
    }
}
