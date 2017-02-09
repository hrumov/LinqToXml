using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace htLinqToXml
{
    public class XmlRead
    {
        XDocument xDoc = XDocument.Load(ResourceFile.XmlFilePath);
    }
}
