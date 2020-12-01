using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Communication
{
    public class Server
    {
        Socket socket;
        IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, 13000);
        bool running = false;
        public event EventHandler<ClientHandler> ClientConnected;
        List<ClientHandler> ClientList;
       
        public Server()
        {
            socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            running = false;
        }

        public void AwaitConnection()
        {
            socket.Bind(ipe);
            socket.Listen(5); // socket waiting for 5 connections in line
            running = true;
            while (running)
            {
                try
                {
                    Socket clientRequest = socket.Accept();
                    ClientHandler newClient = new ClientHandler(clientRequest);
                    ClientList.Add(newClient);                  
                    ClientConnected?.Invoke(this, newClient);
                    newClient.ReceivedMsg += NewClient_ReceivedMsg;

                }
                catch(Exception e)
                {

                }
            }
        }

        private void NewClient_ReceivedMsg(object sender, string e)
        {
            var curClient = sender as ClientHandler;
            foreach (var client in ClientList)
            {
                if(client != curClient)
                {
                    client.SendToClient(curClient.Username + ":" + e);
                }
            }
        }

        public void Stop()
        {
            socket.Close();
            running = false;
        }

        public void DropUser(string name)
        {
            foreach( var client in ClientList)
            {
                if (client.Username == name)
                {
                    ClientList.Remove(client);
                    client.Close();
                    client.SendToClient("@quit");
                }
            }
        }

    }
}
