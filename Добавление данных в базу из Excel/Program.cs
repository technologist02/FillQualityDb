using OfficeOpenXml;
using FillQualityDb.ExcelServices;
using Добавление_данных_в_базу_из_Excel.ExcelServices;
using Добавление_данных_в_базу_из_Excel.Data;
using Добавление_данных_в_базу_из_Excel.Models;

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
        static async void Main(string[] args)
        {
            //FillDbImportantData();

            var path = "..\\..\\..\\Data.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var data = new ExcelPackage(path);
            var sheet = data.Workbook.Worksheets["Нормы по ТУ"];
            var sheet2 = data.Workbook.Worksheets["Данные"];
            
            var films = ReadExcel.GetFilmsFromExcel(sheet);
            var standarts = ReadExcel.GetStandartsQualityFromExcel(sheet);
            var orders = CreateOrders.GetOrdersFromExcel(sheet2);

            var task = new Task(() => ReadExcel.AddFilmsToDatabase(films));
            await task;
            var task2 = new Task(() => ReadExcel.AddStandartQualityToDatabase(standarts));
            await Task.WhenAll(task2);
            var task3 = new Task(() => CreateOrders.AddOrdersToDatabase(orders));
            await Task.WhenAll(task3);
                //.ContinueWith(new Task(() => ReadExcel.AddStandartQualityToDatabase(standarts))); ;
            //Не добавляет данные за раз вместе с пленками
            ReadExcel.AddStandartQualityToDatabase(standarts);
            //Не добавляет данные за раз вместе с пленками. Перезапускаю раскомментируя разные куски кода
            //CreateOrders.AddOrdersToDatabase(orders);
            Console.WriteLine("Hello, World!");
        }
    }
}