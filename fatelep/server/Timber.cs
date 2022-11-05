using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace server
{
    internal class Timber
    {
        public static List<Timber> TimberList = new List<Timber>();

        public Timber(string Wood, int width, int price)
        {
            this.Wood = Wood;
            Width = width;
            Price = price;
        }

        private string wood;
        public string Wood
        {
            get { return wood; }
            set { wood = value; }
        }
        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int price;
        public int Price
        {
            get { return price; }
            set { price = value; }
        }


        public static void LoadResources(string Filename)
        {
            TimberList.Clear();
            XDocument xml = XDocument.Load(Filename);
            foreach (var res in xml.Descendants("timber"))
            {
                Timber newRes = new Timber(
                    (string) res.Attribute("wood"),
                    (int) res.Attribute("width"),
                    (int) res.Attribute("price")
                    );
                TimberList.Add(newRes);
            }
        }
        public static void SaveResources(string Filename)
        {
            XElement root = new XElement("data");

            foreach (Timber r in TimberList)
            {
                root.Add(
                    new XElement(
                        "timber",
                        new XAttribute((XName)"wood", r.Wood),
                        new XAttribute((XName)"width", r.Width),
                        new XAttribute((XName)"price", r.Price)
                        )
                    );
            }
            XDocument xml = new XDocument(root);
            xml.Save(Filename);
        }
    }
}
