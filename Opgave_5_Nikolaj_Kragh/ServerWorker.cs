using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Opgave_1_Nikolaj_Kragh;

namespace Opgave_5_Nikolaj_Kragh
{
    class ServerWorker
    {
        private static List<Book> BookList = new List<Book>()
        {
            new Book("Bogens Titel", "Johan Nilesen", 123, "12345qwertasd"),
            new Book("Titel af bogen", "Nikolaje Krak", 321, "zxcvbnmlkjhgf"),
            new Book("Cool Title", "Jonass Rassmussen", 542, "uyitorpeldkfj"),
            new Book("Original Title", "Copywriter", 999, "qmwnebrvtcyux"),
            new Book("Original Titel", "Kopiskriver", 10, "hgjfkdl657483"),
        };

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 3002);
            server.Start();

            while (true)
            {
                Task.Run(() =>
                {
                    TcpClient socket = server.AcceptTcpClient();
                    DoClient(socket);
                });
            }
        }

        private void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {


                string str = sr.ReadLine();
                string str2 = sr.ReadLine();

                //string[] resarr = str.Split('/');
                if (String.IsNullOrEmpty(str))
                {
                    sw.WriteLine("Input er tomt. Prøv igen.");
                }
                else
                {

                    switch (str.ToLower())
                    {
                        case "hentalle":
                            BookList.ForEach(sw.WriteLine);
                            break;
                        case "hent":
                            sw.WriteLine(BookList.Find(x => x.Isbn13 == str2));
                            break;
                        case "gem":
                            // Gem/Titel/Forfatter/Sidetal/ISBN13
                            JsonConvert.DeserializeObject(str2);
                            BookList.Add(JsonConvert.DeserializeObject<Book>(str2));
                            //BookList.Add(new Book(resarr[1], resarr[2], Int32.Parse(resarr[3]), resarr[4]));
                            sw.WriteLine("Book saved!");
                            break;
                        default:
                            sw.WriteLine("Fejlagtigt input.");
                            break;
                    }
                }
                sw.Flush();
            }
        }
    }
}