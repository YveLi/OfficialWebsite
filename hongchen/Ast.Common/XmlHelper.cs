using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ast.Common
{
    public class XmlHelper
    {
        public static string ReadXmlValue(string strXml, string node)
        {
            string value = "";
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(strXml);

                XmlNode xmlNode = xmlDocument.SelectSingleNode(node);
                value = xmlNode.InnerText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}
