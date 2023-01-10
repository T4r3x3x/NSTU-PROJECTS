using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DurakForms
{

    class Server
    {
        static ServerObject server; // сервер
        static Thread listenThread; // поток для прослушивания

        static internal void Start()
        {
            try
            {
                server = new ServerObject();              
                listenThread = new Thread(new ThreadStart(server.ConnectionsSearching));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Off();
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class ServerObject
    {
        private const int port = 2005;
        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>(); // лист всех подключений
        int id = 0;      


        //Вносим клиента в список
        public void AddConnection(ClientObject client)
        {
            clients.Add(client);
            Interface.server = this;
        }


        // проверка входящих подключений
        protected internal void ConnectionsSearching()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject client = new ClientObject(tcpClient, this, id++);
                    //Созднание потока под нового клиента
                    Thread clientThread = new Thread(new ThreadStart(client.Listening));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Off();
            }
        }

        protected internal void RecieveCommand(string command, int id)
        {
            if (command == "r" && !GameManager.isPlaying)
                if (clients.Count() > 1)
                {                  
                    GameManager.StartGame();
                    Interface.ShowTable();
                }

            if (GameManager.isPlaying)
            {
                if (command != "r")
                    Interface.ProcessingCommand(command, id);
                
               
            }

        }

        protected internal void SendMessage(string message, int id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message+";");
            clients[id].Stream.Write(data, 0, data.Length); //передача данных
             
        }
        protected internal void BroadcastMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message + ";");
            foreach (var client in clients)
            {
                client.Stream.Write(data, 0, data.Length); //передача данных
            }
           

        }


        // отключение сервера
        protected internal void Off()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }

    public class ClientObject
    {
        protected internal int id { get; }
        protected internal NetworkStream Stream { get; private set; }
        string userName;
        TcpClient client;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject, int id)
        {
            this.id = id;
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Listening()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetCommand();
                userName = message;

               // message = userName + " вошел в чат. IP: " + GetMassage();
                // посылаем сообщение о входе в чат всем подключенным пользователям
               // server.RecieveData(message, this.id);
             //   Console.WriteLine(message);

                //получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetCommand();
                        message = String.Format(message);
                    //    Console.WriteLine("Command"+message);
                        server.RecieveCommand(message, this.id);
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.RecieveCommand(message, this.id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                //  server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetCommand()
        {
            byte[] data = new byte[1000]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }  
}