using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace ParserTadawul
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [STAThread]
        static void Main(string[] args)
        {
            logger.Info("НАЧАЛО РАБОТЫ ПАРСЕРА");
            try
            {
                List<string> stocks = new List<string>();
                string url = "https://www.saudiexchange.sa/wps/portal/tadawul/markets/equities/market-watch/market-watch-today?locale=en";
                WebClient wc = new WebClient();
                int attempt = 0;
                string page = null;
                while (attempt != 3 & page == null)
                {
                    try
                    {
                        attempt++;
                        logger.Info("Попытка подключения к сайту (proxy Indonesia Jakarta)...");
                        wc.Proxy = new WebProxy("182.253.3.156:8080");
                        page = wc.DownloadString(url);
                        
                    }
                    catch (Exception ex)
                    {
                        logger.Info("Ошибка подключения к сайту");
                        logger.Debug(ex);
                        try
                        {
                            logger.Info("Попытка подключения к сайту (proxy German)...");
                            wc.Proxy = new WebProxy("159.69.66.224:8080"); // German
                            page = wc.DownloadString(url);
                        }
                        catch (Exception ex1)
                        {
                            logger.Info("Ошибка подключения к сайту");
                            logger.Debug(ex1);
                            try
                            {
                                logger.Info("Попытка подключения к сайту (proxy Brazil Goiânia)...");
                                wc.Proxy = new WebProxy("45.167.30.82:8080"); // Brazil Goiânia
                                page = wc.DownloadString(url);
                            }
                            catch (Exception ex2)
                            {
                                logger.Info("Ошибка подключения к сайту");
                                logger.Debug(ex2);
                            }
                        }
                    }
                }              
                if (page != null)
                {
                    logger.Info("Попытка подключения к сайту прошла успешна");
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(page);
                    HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//tbody/tr/td");
                    HtmlWeb htmlWeb = new HtmlWeb();
                    string line = "";
                    int k = 0;
                    foreach (var item in nodes)
                    {
                        try
                        {
                            switch (k)
                            {
                                case 0:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 1:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 2:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 3:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 4:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 5:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 6:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 7:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".") + ", ";
                                    break;
                                case 8:
                                    k++;
                                    line += item.InnerText.Trim().Replace(",", ".");
                                    break;
                                case 14:
                                    stocks.Add(line);
                                    line = "";
                                    k = 0;
                                    break;
                                default:
                                    k++;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Debug(ex);
                        }
                        logger.Info("Загрузка stocks завершена");
                    }
                    try
                    {
                        using (StreamWriter stream = new StreamWriter(args[0] + "\\" + DateTime.Today.ToString("dd" + "MM" + "yyyy") + ".csv"))
                        {
                            stream.WriteLine("sep=,");
                            stream.WriteLine("Group, Name, Last Price, VOL, Change Value, Perc Change, No. Of Trades, Volume Traded, Price");
                            foreach (var item in stocks)
                            {
                                stream.WriteLine(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Debug(ex);
                    }
                }
                else
                {
                    logger.Info("Не удалось подключиться к сайту");
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
            }                      
            logger.Info("ОКОНЧАНИЕ РАБОТЫ ПАРСЕРА");
        }
    }
}
