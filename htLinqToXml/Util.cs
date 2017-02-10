using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htLinqToXml
{
    public static class Util
    {
        public static double ReturnTotal(this XElement xE)
        {
            double total = 0;
            foreach (XElement e in xE.Descendants("total"))
            {
                total += Convert.ToDouble(e.Value);
            }
            return total;
        }
    }
}
