﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Booru_Parser
{
    class Anime_picture : Booru
    {
        public Anime_picture(string _tag) : base(_tag)
        {
            string html_0 = "/pictures/view_posts/"; // html теги, записаны в переменные для удобности и читаемости
            string html_1 = "disable_on_small";
            string html_2 = "data-pubtime";
            string page = getPage();
            if (page.Contains(html_2)) // если есть нужный html код
            {
                while (page.Contains(html_1))
                {
                    page = page.Substring(page.IndexOf(html_0) + html_0.Count());
                }
                total_page = Convert.ToInt32(page.Substring(0, page.IndexOf('?')));
            }
            else { error = true; } // если нет то ошибка
        }

        public override string Url
        {
            get
            {
                url = "https://anime-pictures.net/pictures/view_posts/" + current_page + "?search_tag=" + Tag + "&lang=en"; // переопределение запроса
                return url;
            }
        }

        public override List<Picture> getPics() // переопределение метода на получение листа ссылок
        {
            List<Picture> pic_list = new List<Picture>();
            string html_0 = "data-pubtime"; // html теги, записаны в переменные для удобности и читаемости
            string html_1 = "<a href=" + '"';
            string html_2 = "posts_block";
            string html_3 = "/pictures/get_image/";
            string html_4 = "https://anime-pictures.net"; // данный сайт дает ссылки без оглавления сайта
            string page = getPage();
            page = page.Substring(page.IndexOf(html_2));
            while (page.Contains(html_0)) // в первом этапе собираются ссылки не на сами картинки, а на ссылки со страницами с ними
            {
                string buff = page.Substring(page.IndexOf(html_1) + html_1.Count());
                buff = buff.Substring(0, buff.IndexOf('"'));
                if (buff.Contains("by_tag")) pic_list.Add(new Picture((html_4 + buff), "","", null));
                page = page.Substring(page.IndexOf(buff));
            }
            for (int i = 0; i < pic_list.Count; i++)
            {
                page = new WebClient().DownloadString(pic_list[i].url);
                if (page.Contains(html_3) == false) // такое может случается если картинка +18
                {
                    continue;
                }
                page = page.Substring(page.IndexOf(html_3));
                pic_list[i] = new Picture((html_4 + page.Substring(0, page.IndexOf('"'))), "", "", null); 
            }
            return pic_list;
        }
    }
}
