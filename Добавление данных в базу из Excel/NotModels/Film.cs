using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class Film
    {
        public override bool Equals(Object obj)
        {
            if (obj == this) return true;
            if (obj is not Film film) return false;
            else
            {
                var res = film.Mark == this.Mark && film.Thickness == this.Thickness && film.Color == this.Color;
                if (res)
                {
                    Console.WriteLine(res.ToString());
                }
                return res;
            }
        }

        public override string ToString()
        {
            return $"Марка: {Mark} толщина: {Thickness} цвет: {Color}";
            
        }
    }
}
