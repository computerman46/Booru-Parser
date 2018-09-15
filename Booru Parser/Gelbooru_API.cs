using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Threading.Tasks;

namespace Booru_Parser
{
    class Gelbooru_API : Booru
    {
        int total_pics = 0;
        public Gelbooru_API(string _tag) : base(_tag)
        {
            XmlDocument xml_page = new XmlDocument();
            xml_page.LoadXml(getPage()); // получение xml документа
            if (xml_page.DocumentElement.LastChild == null) error = true;
            XmlElement root = xml_page.DocumentElement;
            total_pics = Convert.ToInt32(root.Attributes[0].Value); // получение общего количества картинок
            total_page = (int)Math.Round((double)total_pics / 42); // деления на 42, 42 было взято из-за удобности, так как в HTML столько же 
        }

        public override string Url
        {
            get
            {
                url = "https://gelbooru.com/index.php?page=dapi&s=post&q=index&tags=" + Tag + "&pid=" + current_page + "&limit=1"; // переопределение запроса 
                return url;
            }
        }
        delegate int getMin();

        int getNumPage() // запросы в Gelbooru осуществляются не по страницам а по колличеству отображения картинок
        {                // причем в API можно не следовать за числом 42, можно запросить от 1 до 100 картинок
            return current_page * 42;
        }

        List<Picture> loopPic(int min, int max) // так как получение одной страницы и всех картинок разом похожи, они были объедены в общий метод 
        {                                      // который вызывается по двум параметрам
            List<Picture> pic_list = new List<Picture>();
            for (int i = min; i < max; i++)
            {
                string _url = "https://gelbooru.com/index.php?page=dapi&s=post&q=index&tags=" + Tag + "&pid=" + i + "&limit=1";
                XmlDocument xml_page = new XmlDocument();
                xml_page.LoadXml(new WebClient().DownloadString(_url));
                if (xml_page.DocumentElement.LastChild != null)
                {
                    pic_list.Add(new Picture(xml_page.DocumentElement.LastChild.Attributes[2].Value, "", "", null));
                }
                   
            }
            return pic_list;
        }

        public override List<Picture> getAll(List<Picture> pics = null)
        {
            return loopPic(0, total_pics);
        }
        public override List<Picture> getPics()
        {
            getMin min = () =>
            {
                int r = (getNumPage() - 42);
                if (r < 0)
                    r = 0;
                return r;
            };
            return loopPic(min(), getNumPage());
        }

    }
}
