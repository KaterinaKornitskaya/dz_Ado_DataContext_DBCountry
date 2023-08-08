using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dz_Ado_DataContext_DBCountry.Model
{
    // Пользовательский класс-модель CountryDataContext для взаимодейтсвия с БД,
    // наследует класс DataContext - класс-посредник между sql-таблицами и кодом кодом
    internal class CountryDataContext : DataContext
    {
        // в конструктор указываем соединение с БД
        public CountryDataContext() : base
            (ConfigurationManager.ConnectionStrings["CountryDb"].ConnectionString)
        { }

        // создаем таблицу Countries, с пимощью метода GetTable() получили все записи
        // таблицы Country из БД в оперативную память
        public Table<Country> Countries => GetTable<Country>();

        // метод для добавления новой страны
        public void AddNewCountry(string name, string cap, float pop,
            float sq, string part)
        {
            // создаем объект Country, инициализируем поля значениями
            Country country = new Country
            {
                Name = name,
                Capital = cap,
                Population = pop,
                Square = sq,
                PartOfWorld = part
            };
            Countries.InsertOnSubmit(country);  // метод для добавления сущности
            SubmitChanges();  // метод сверяет внесенные данные с БД, и создает sql-запрос
                              // которые внесет соответствующие изменения в БД
        }

        // метод для удаления страны
        public void DeleteCountry(string name)
        {
            // создаем пустую ссылку, которая вернется, если в цикле ниже
            // не найдет указанную страну
            Country country = null;
            // в массиве стран ищем нужную страну
            foreach(var item in  Countries)
            {
                if(item.Name == name)  // если есть страна с таким названием
                {
                    country = item;  // инициализируем пустую ссылку науденной страной
                    break;
                }
            }
            if(country != null)  // если страна найдена
            {
                Countries.DeleteOnSubmit(country);  // удаляем эту страну
                SubmitChanges();  // синхронизация изменений с БД
                // вывод сообщения о успешном удалении
                MessageBox.Show($"Страна {name} удалена из базы данных.");
            }
            else if (country == null)  // если страна не найдена
            {
                // вывод сообщения о том, что страны с таким названием нет
                MessageBox.Show($"Страна {name} не найдена!");
            }       
        }

        // метод для редактирования страны
        public void EditCountry(string name, string cap, float pop, float sq, string part)
        {
            Country country = null;
            // ищем страну в таблице стран
            foreach(var item in Countries)
            {
                if (item.Name == name)  // если название найдено
                {
                    country = item;  // инициализируем пустую ссылку
                    break;
                }                  
            }
            if(country != null)  // если страна найдена
            {
                // изменяем поля, заполняем новыми значениями
                country.Capital = cap;
                country.Population = pop;
                country.Square = sq;
                country.PartOfWorld = part;
                SubmitChanges();  // синхронизация информации с БД
                MessageBox.Show($"Информация о стране {name} отредактирована.");
            }
            else if (country == null)
            {
                MessageBox.Show($"Страна {name} не найдена!");
            }
        }

        // Запросы:
        // Топ-5 стран по площади
        public List<string> Top3BySquare()
        {
            // запрос - упорядочить площадь по убыванию и взять топ-5
            var res = (from item in Countries
                      orderby item.Square descending
                      select item.ToString()).Take(5);
            
            // создаем список и инициализируем его результатами запроса
            List<string> list = new List<string>();
            foreach (var item in res)
            {
                    list.Add(item);
            }
            return list;
        }


        // Топ-5 стран по кол-ву жителей
        public List<string> Top3ByPopulation()
        {
            // запрос - упорядочить по населению по убыванию и взять топ-5
            var res = (from item in Countries
                       orderby item.Population descending
                       select item.ToString()).Take(5);

            // создаем список и инициализируем его результатами запроса
            List<string> list = new List<string>();
            foreach (var item in res)
                list.Add(item);
            return list;
        }

        // Страна с наибольшей площадью
        public IQueryable<string> MaxSquare()
        {
            // пыталась типом возвращаемого значения указать string,
            // но посмотрела, что запрос вернул IQueryable, и это оказалось 
            // рабочим вариантом
            var res = (from item in Countries
                       orderby item.Square descending
                       select item.ToString()).Take(1);
            
            return res;
        }

        //Страна Европы с наименьшей площадью
        public IQueryable<string> MinSqEurope()
        {
            var res = (from item in Countries
                      orderby item.Square
                      where item.PartOfWorld.ToLower() == "европа"
                      select item.ToString()).Take(1);
           
            return res;
        }

        // Средняя площадь стран Европы
        public double AvSqEurope()
        {
            // запрос - выбираем площадь в странах, где PartOfWorld = европа
            var res = (from c in Countries
                      where c.PartOfWorld.ToLower()=="европа"
                      select c.Square).ToList();

            // применяем к запросу метод Average(), и инициализируем этим
            // значением переменную av
            double av = res.Average();
            return av;
        }

        // Общее кол-во стран
        public int CountryCount()
        {
            var res = from c in Countries
                      select c;
            int count = res.Count();
            return count;
        }

        // Часть света с наибольшим кол-вом стран
        public List<string> MaxPart()
        {
            var res = from c in Countries
                      group c by c.PartOfWorld into gr
                      orderby gr.Key descending
                      select gr.First();

            List<string> strings = new List<string>()
            {
                res.First().PartOfWorld
            };
            return strings;
        }

        public List<string> PartOfWorldCountriesAmount()
        {
            var res = from c in Countries
                      group c by c.PartOfWorld into gr  // сгрупировали по Части света в группу
                      orderby gr.Count()  // упорядочили по кол-ву стран в группе(части света) 
                      select gr;

            // созлаем список и инициализируем его результатом запроса
            List<string> list = new List<string>();
            // в результате группировки имеем свойство Key - ключ по которому группировали,
            // здесь Часть Света, и метод Count() - кол-во эл-тов в кажоц группе (стран в части света)
            foreach (var c in res)
                list.Add($"Part of world: {c.Key}\t\tCount of countries: {c.Count()}");

            return list;
        }
    }
}
