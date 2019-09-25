using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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

                string[] resarr = str.Split('/');

                switch (resarr.Length)
                {
                    case 1:
                        BookList.ForEach(sw.WriteLine);
                        break;
                    default:
                        //sw.WriteLine(resarr[0]);
                        switch (resarr[0].ToLower())
                        {
                            case "hent":
                                sw.WriteLine(BookList.Find(x => x.Isbn13 == resarr[1]));
                                break;
                            case "gem":
                                // Gem/Titel/Forfatter/Sidetal/ISBN13
                                BookList.Add(new Book(resarr[1], resarr[2], Int32.Parse(resarr[3]), resarr[4]));
                                sw.WriteLine("This far");
                                break;
                            default:
                                sw.WriteLine("Fejlagtigt input.");
                                break;
                        }
                        break;
                }

                sw.Flush();
            }
        }
    }
}
