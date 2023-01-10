using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DurakForms
{
    internal class Client
    {
        private const int port = 2005;
        static TcpClient client;
        static NetworkStream stream;
        static internal IPAddress ip;
        static internal EnteringForm form;

        static internal void StartHosting(string nickname)
        {
            Server.Start();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            // Получение ip-адреса.
            ip = null;
            foreach (var _ip in host.AddressList)
            {
                if (_ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = _ip;
                }
            }
            client = new TcpClient();
            client.Connect(ip, port); //подключение клиента
            stream = client.GetStream(); // получаем поток
            Connection(nickname, ip);
       //     Thread listingThread = new Thread(new ThreadStart(Listening));
        }


        static internal void Connect(string nickhame, IPAddress ip)
        {
         //   Console.WriteLine("Введите ip-адрес сервера для подключения");
            client = new TcpClient();

            try
            {
                client.Connect(IPAddress.Parse("192.168.0.124"), port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                Connection(nickhame, ip);
             //   Listening();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           
        }


        static void Connection(string nickname, IPAddress ip)
        {
            //Отправляем никнейм серверу 
            byte[] data = Encoding.Unicode.GetBytes(nickname);
            stream.Write(data, 0, data.Length);

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIP = host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            data = Encoding.Unicode.GetBytes(localIP.ToString());
            stream.Write(data, 0, data.Length);

            // запускаем новый поток для получения данных
            Thread receiveStream = new Thread(new ThreadStart(ReceiveMessage));
            receiveStream.Start(); //старт потока
        //    Console.WriteLine("Соединение прошло успешно, {0}!", nickname);

        }



        static void Listening()
        {
            while (true)
            {
               // string message = Console.ReadLine();
          //      SendMessage(message);
            }
        }

        // отправка сообщений
        static internal void SendCommand(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        // получение сообщений
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[1000]; // буфер для приёма сообщений
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    
                    string[] messages = builder.ToString().Split(';');

                    form.Invoke(form.UpdateTableDelegat, new object[] { messages });

                    //foreach (var message in messages)
                    //{
                    //    if(message == "/c")
                    //    {
                    //        Console.Clear();
                    //        break;
                    //    }
                    //    Console.Write(message);//вывод сообщения
                    //}

                }
                catch
                {
                    //Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    //Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}