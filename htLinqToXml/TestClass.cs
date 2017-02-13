using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htLinqToXml
{
    [TestFixture]
    public class TestClass
    {

        public static XElement xElem = XElement.Load(ResourceFile.XmlFilePath);
        public static IEnumerable<XElement> customers = xElem.Elements();


        [Test]
        public void OrdersSum()
        {
            // 1.Выдайте список всех клиентов, чей суммарный оборот(сумма всех заказов) превосходит некоторую величину X.
            // Продемонстрируйте выполнение запроса с различными X (подумайте, можно ли обойтись без копирования запроса несколько раз)

            IEnumerable<XElement> clientList = customers.Where(e => e.Element("orders").Elements("order").Select(d => Double.Parse(d.Element("total").Value)).Sum() > 10000);

            foreach (XElement e in clientList)
            {
                Console.WriteLine(e.Element("name").Value);
            }
        }

        [Test]
        public void GroupByCountry()
        {
            // 2.Сгруппировать клиентов по странам.

            var clientList = xElem.Elements("customer").GroupBy(e => e.Element("country").Value);

            foreach (IGrouping<string, XElement> s in clientList)
            {
                Console.WriteLine("country  {0}", s.Key);
                foreach (XElement e in s)
                {
                    Console.WriteLine("\t {0}", e.Element("name").Value);
                }
            }
        }

        [Test]
        public void GetMaxOrderValue()
        {
            // 3. Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X

            IEnumerable<XElement> clientList = customers.Where(e => e.Element("orders").Elements("order").Where(d => Double.Parse(d.Element("total").Value) > 10000).Any());

            foreach (XElement e in clientList)
            {
                Console.WriteLine(e.Element("name").Value);
            }
        }

        [Test]
        public void GetDate()
        {
            // 4. Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали
            // клиентами(принять за таковые месяц и год самого первого заказа)

            var clientList = customers
                .Where(e => e.Element("orders").Element("order") != null)
                .Select(e => new Tuple<string, string>(e.Element("name").Value, e.Element("orders").Element("order").Element("orderdate").Value.Substring(0, 7)));

            foreach (Tuple<string, string> e in clientList)
            {
                Console.WriteLine("{0} \t\t {1}", e.Item1, e.Item2);
            }
        }

        [Test]
        public void GetSortedDate()
        {
            // 5. Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам
            // клиента(от максимального к минимальному) и имени клиента

            var clientList = customers
                .Where(e => e.Element("orders").Element("order") != null)
                .Select(e => new Tuple<string, string, double>(e.Element("name").Value, e.Element("orders").Element("order")
                .Element("orderdate").Value.Substring(0, 7), e.Element("orders").Elements("order").Select(d => Double.Parse(d.Element("total").Value)).Sum()))
                .OrderBy(e => e.Item2.Substring(0, 4)).ThenBy(e => e.Item2.Substring(5, 2)).ThenByDescending(e => e.Item3).ThenBy(e => e.Item1);

            foreach (Tuple<string, string, double> e in clientList)
            {
                Console.WriteLine("{0} \t\t\t\t {1} \t\t\t\t {2}", e.Item1, e.Item2, e.Item3);
            }
        }

        [Test]
        public void GetZipCode()
        {
            // 6.	Укажите всех клиентов, у которых указан нецифровой код или не заполнен регион или в телефоне не указан
            // код оператора(для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).

            var clientList = from customer in customers
                             where ((customer.Elements("postalcode").Count() == 0) || ((customer.Elements("postalcode").Count() != 0) &&
                                    (Regex.IsMatch(customer.Element("postalcode").Value, "[A-Za-z]")))
                                    || !Regex.IsMatch(customer.Element("phone").Value, "(\\([^\\)]*\\))"))
                             select customer;

            foreach (XElement e in clientList)
            {
                Console.WriteLine(e.Element("name").Value);
            }
        }

        [Test]
        public void GetAveragePerYear()
        {
            // 7. Рассчитайте среднюю прибыльность каждого города(среднюю сумму заказа по всем клиентам из данного города)
            // и среднюю интенсивность(среднее количество заказов, приходящееся на клиента из каждого города)

            var clientList = customers.GroupBy(g => g.Element("city").Value);
            Dictionary<string, Tuple<double, double>> result = new Dictionary<string, Tuple<double, double>>();
            foreach (var g in clientList)
                result.Add(g.Key, new Tuple<double, double>(g.Average(h => h.Element("orders").Elements("order").Count()),
                    g.Sum(h => h.Element("orders").Elements("order").
                    Sum(k => Double.Parse(k.Element("total").Value))) /
                    g.Average(h => h.Element("orders").Elements("order").Count()) / g.Count()));

            foreach (var e in result)
            {
                Console.WriteLine($"{e.Key}   {Math.Round(e.Value.Item1, 2)}    {Math.Round(e.Value.Item2, 2)}");
            }

        }

        /*[Test]
        public void GetAveragePerMonths()
        {
            // 8. Сделайте среднегодовую статистику активности клиентов по месяцам(без учета года), статистику по годам,
            // по годам и месяцам(т.е.когда один месяц в разные годы имеет своё значение).


        }*/
    }
}
