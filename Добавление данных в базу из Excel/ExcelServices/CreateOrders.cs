using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Добавление_данных_в_базу_из_Excel.Data;
using Добавление_данных_в_базу_из_Excel.Models;

namespace Добавление_данных_в_базу_из_Excel.ExcelServices
{
    internal class CreateOrders
    {
        public static List<OrdersQuality> GetOrdersFromExcel(ExcelWorksheet sheet)
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
                var filmDb = db.Films.FirstOrDefault(x => x.Mark == film.Mark && x.Thickness == film.Thickness && x.Color == film.Color.ToLower());
                if (filmDb == null) continue;
                var order = new OrdersQuality();
                var extruderDb = db.Extruders.FirstOrDefault(x => x.Name == (string)sheet.Cells[i, 6].Value);
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
        public static void AddOrdersToDatabase(List<OrdersQuality> orders)
        {
            using var db = new QualityV3Context();
            {
                db.OrdersQualities.AddRange(orders);
                db.SaveChanges();
            }
        }
    }
}
