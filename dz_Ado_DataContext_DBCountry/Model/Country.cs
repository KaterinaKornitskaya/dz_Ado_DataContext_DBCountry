using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace dz_Ado_DataContext_DBCountry.Model
{
    [Table]  // атрибуты, позволяют сопоставить модель из БД с данной таблицей
    internal class Country  // класс Страна
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }  // Id

        [Column]
        public string Name { get; set; }  // Название страны

        [Column]
        public string Capital { get; set; }  // Столица

        [Column]
        public float Population { get; set; }  // Население

        [Column]
        public float Square { get; set; }  // Площадь

        [Column]
        public string PartOfWorld { get; set; }  // Часть света

        // переопределенный метод ToString() для вывода информации о стране
        public override string ToString()  
        {
            return $"{Name}   {Capital}   {Population}   {Square}   {PartOfWorld}";
        }
    }
}
