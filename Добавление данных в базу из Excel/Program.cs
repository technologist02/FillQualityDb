using OfficeOpenXml;
using FillQualityDb.ExcelServices;
using Добавление_данных_в_базу_из_Excel.Data;
using Добавление_данных_в_базу_из_Excel.Models;
using Добавление_данных_в_базу_из_Excel.DbMethods;

namespace FillQualityDb
{
    internal class Program
    {
        static void FillDbImportantData()
        {
            using var db = new QualityV3Context();
            var user = new User
            {
                Login = "admin",
                Email = "admin@mail.ru",
                Name = "admin",
                Surname = "adminoff",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin")
            };

            db.Users.Add(user);
            db.SaveChanges();
            var roles = new List<UserRole>()
            {
                new() {Function = "admin"},
                new() {Function = "operator"},
                new() {Function = "inspector"},
                new() {Function = "technologist"}
            };
            db.UserRoles.AddRange(roles);
            db.SaveChanges();
            var title = new StandartQualityTitle
            {
                Title = "ТУ 22.21.30-002-03210778-2022",
                Description = "Технические условия"
            };
            db.StandartQualityTitles.Add(title);
            db.SaveChanges();

            var extruders = new List<Extruder>()
            {
                new() {Name = "Tecom"},
                new() {Name = "KS-1"},
                new() {Name = "KS-2" }
            };
            db.Extruders.AddRange(extruders);
            db.SaveChanges();
        }
        static void Main(string[] args)
        {
            //FillDbImportantData();

            var path = "..\\..\\..\\Data.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var data = new ExcelPackage(path);
            var sheet = data.Workbook.Worksheets["Нормы по ТУ"];
            var sheet2 = data.Workbook.Worksheets["Данные"];
            
            var films = ReadExcel.GetFilmsFromExcel(sheet);
            var addFilmTask = DbMethods.AddFilmsToDatabase(films);
            addFilmTask.Wait();
            List<StandartQualityFilm> standarts;
            List<OrdersQuality> orders;
            var task = Task.Run(() => ReadExcel.GetStandartsQualityFromExcelAsync(sheet));
            var t2 = task.ContinueWith(task =>
            {
                standarts = task.Result;
                DbMethods.AddStandartQualityToDatabase(standarts).Wait();
                return ReadExcel.GetOrdersFromExcelAsync(sheet2);
            });
            t2.Wait();
            var t3 = t2.ContinueWith(task =>
            {
                orders = task.Result.Result;
                DbMethods.AddOrdersToDatabase(orders).Wait();
            }
            );
            t3.Wait();

            Console.WriteLine("Hello, World!");
        }
    }
}