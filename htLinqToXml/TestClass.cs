using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htLinqToXml
{
    [TestFixture]
    class TestClass
    {
        XElement xElem = XElement.Load(ResourceFile.XmlFilePath);

        [Test]
        public void OrdersSum() //доделать параметризацию
        {
            // 1.Выдайте список всех клиентов, чей суммарный оборот(сумма всех заказов) превосходит некоторую величину X.
            // Продемонстрируйте выполнение запроса с различными X (подумайте, можно ли обойтись без копирования запроса несколько раз)

            var clientList = xElem.Elements("customer").Elements("name").Where(s => s.Parent.ReturnTotal() > Int32.Parse(ResourceFile.OrdersSum));

            foreach (XElement e in clientList)
            {
                Console.WriteLine(e.Value);
            }
        }

        [Test]
        public void GroupByCountry()
        {
            // 2.Сгруппировать клиентов по странам.

            var clientList = xElem.Elements("customer").Elements("name");

            //Console.WriteLine(clientList);

            foreach (string e in clientList)
            {
                Console.WriteLine(e);
            }
        }


        // 3.	Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X
        // 4.	Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами(принять за таковые месяц и год самого первого заказа)
        // 5.	Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента(от максимального к минимальному) и имени клиента
        // 6.	Укажите всех клиентов, у которых указан нецифровой код или не заполнен регион или в телефоне не указан код оператора(для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).
        // 7.	Рассчитайте среднюю прибыльность каждого города(среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность(среднее количество заказов, приходящееся на клиента из каждого города)
        // 8.	Сделайте среднегодовую статистику активности клиентов по месяцам(без учета года), статистику по годам, по годам и месяцам(т.е.когда один месяц в разные годы имеет своё значение).

    }
}
