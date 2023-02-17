using System;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace Парсер
{
    class Program
    {
        static void DownloadPhoto(WebClient wc)
        {
            string path = "https://student.mirea.ru/media/photo/";
            string forPhotoLoad = "https://student.mirea.ru/";
            wc.Encoding = System.Text.Encoding.UTF8;
            string dataForAlbuns = wc.DownloadString(path); ;
            MatchCollection albumLinks = GetAlbumLinks(dataForAlbuns);
            for (int i = 0; i < 6; i++)
            {
                string dataForPhoto = wc.DownloadString(path + albumLinks[i].Groups[1].Value);
                string nameOfAlbum = GetAlbumName(dataForPhoto);
                MatchCollection nameOfPhoto = GetPhotoName(dataForPhoto);
                MatchCollection photoLinks = GetPhotoLinks(dataForPhoto);
                int photoCounter = photoLinks.Count;
                Console.WriteLine("Создание папки " + nameOfAlbum);
                Directory.CreateDirectory(@"C:\Users\dexte\Documents\практика\Парсер\Download\" + nameOfAlbum);
                for (int j = 0; j < photoCounter; j++)
                {
                    try
                    {
                        wc.DownloadFile(forPhotoLoad + photoLinks[j].Groups[1].Value, @"C:\Users\dexte\Documents\практика\Парсер\Download\" + nameOfAlbum + @"\" + nameOfPhoto[j].Groups[2].Value + ".jpg");
                        Console.WriteLine("файл " + nameOfPhoto[j].Groups[2].Value + " загружен");
                    }
                    catch
                    {
                        Console.WriteLine("ошибка при загрузке файла " + nameOfPhoto[j].Groups[2].Value);
                    }
                }
            }
        }

        static MatchCollection GetAlbumLinks(string dataForPhoto)
        {
            MatchCollection macthes = Regex.Matches(dataForPhoto, @"<a class=""u-link-v2"" href=""/media/photo/(.*?)""></a>");
            return macthes;
        }

        static MatchCollection GetPhotoLinks(string dataForPhoto)
        {
            MatchCollection macthes = Regex.Matches(dataForPhoto, @"<img class=""img-fluid u-block-hover__main--grayscale u-block-hover__img"" src=""(/upload/iblock/.*?/.*?.jpg)"">");
            return macthes;
        }

        static string GetAlbumName(string dataForPhoto)
        {
            Match macth = Regex.Match(dataForPhoto, @"<h2 class=""text-uppercase u-heading-v2__title g-mb-5"">(.*?)</h2>");
            return macth.Groups[1].Value;
        }

        static MatchCollection GetPhotoName(string dataForPhoto)
        {
            MatchCollection macthes = Regex.Matches(dataForPhoto, @"<img class=""img-fluid u-block-hover__main--grayscale u-block-hover__img"" src=""/upload/iblock/(.*?)/(.*?).jpg"">");
            return macthes;
        }

        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            DownloadPhoto(wc);
            Console.ReadLine();
        }
    }
}
