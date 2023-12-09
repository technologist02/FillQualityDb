using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Добавление_данных_в_базу_из_Excel.Data;
using Добавление_данных_в_базу_из_Excel.Models;

namespace Добавление_данных_в_базу_из_Excel.DbMethods
{
    internal class DbMethods
    {
        public async static Task AddRangeEntitiesAsync<T>(List<T> entities)
        {
            
            using var db = new QualityV3Context();
            await db.AddRangeAsync(entities);
            await db.SaveChangesAsync();
        }
        
        public async static Task AddOrdersToDatabase(List<OrdersQuality> orders)
        {
            using var db = new QualityV3Context();
            {
                await db.OrdersQualities.AddRangeAsync(orders);
                await db.SaveChangesAsync();
            }
        }

        public async static Task AddStandartQualityToDatabase(List<StandartQualityFilm> standartQualities)
        {
            using var db = new QualityV3Context();
            await db.StandartQualityFilms.AddRangeAsync(standartQualities);
            await db.SaveChangesAsync();
        }
        public async static Task AddFilmsToDatabase(List<Film> films)
        {
            using var db = new QualityV3Context();
            {
                var filmsFromDb = await db.Films.ToArrayAsync();
                foreach (var film in films)
                {
                    if (!filmsFromDb.Contains(film))
                    {
                        await db.Films.AddAsync(film);
                        Console.WriteLine(film.ToString());
                    }
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
