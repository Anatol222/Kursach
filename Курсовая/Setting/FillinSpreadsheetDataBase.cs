using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Курсовая.Setting
{
    internal class FillinSpreadsheetDataBase
    {
        public FillinSpreadsheetDataBase()
        {
            BusSpreadsheet();
        }
        private void BusSpreadsheet()
        {

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://zippybus.com/");

            //Загружаем главную страницу

            //Получаем список городов
            var Sities = doc.DocumentNode.SelectNodes("//h3/a").Where(x => !x.GetAttributeValue("href", null).Contains("region"));

            var busLinks = doc.DocumentNode.SelectNodes("//div//a[contains(@class,'transport-1-md')]");
            for (int i = 0; i < Sities.Count(); i++)
            {
                //    //Выводим название города
                sities.Add(Sities.ElementAt(i).InnerText);

                //    //Получаем получаем ссылки на номера автобусов в городе
                string SityLink = busLinks[i].GetAttributeValue("href", null);

                //Загружаем расписание автобусов в городе
                HtmlDocument busNumbersDoc = web.Load(SityLink);

                //Парсим номера автобусов
                var busNumbers = busNumbersDoc.DocumentNode.SelectNodes("//li//a").Where(x => x.GetAttributeValue("title", null) != null
                && x.GetAttributeValue("title", null).Contains("Автобус")
                & !x.GetAttributeValue("title", null).Contains("i")
                & !x.GetAttributeValue("title", null).Contains("Инфо")
                & !x.GetAttributeValue("title", null).Contains("!"));


                foreach (var busNumber in busNumbers)
                {
                    BusNumber.Add(busNumber.InnerText);
                    string busNumberLink = busNumber.GetAttributeValue("href", null);
                    HtmlDocument BusNumberShedule = web.Load(busNumberLink);
                    var firstDirection = BusNumberShedule.DocumentNode.SelectNodes("//div[3]/h4");
                    Console.WriteLine(firstDirection[0].InnerText);
                    var Stations = BusNumberShedule.DocumentNode.SelectNodes("//div[3]/div//a");
                    foreach (var station in Stations)
                    {
                        Console.WriteLine(station.InnerText);
                    }
                    var secondDirection = BusNumberShedule.DocumentNode.SelectNodes("//div[4]/h4");
                    //TODO удалить ненужные символы
                    if (secondDirection != null)
                    {
                        Console.WriteLine(secondDirection[0].InnerText);
                        foreach (var station in Stations)
                        {
                            Console.WriteLine(station.InnerText);
                        }
                    }
                }
            }
        }

        public List<string> BusNumber { get; set; } = new List<string>();
        public List<string> sities { get; set; } = new List<string>() { };
    }
}
