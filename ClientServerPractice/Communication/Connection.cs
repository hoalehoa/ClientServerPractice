using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.Communication
{
   public class Connection
    {
        Socket connection;
        bool running = false;
        public event EventHandler<string> ReceivedMsg;

        public void Connect(string username)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, 13000);
            connection = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                connection.Connect(ipe);
                running = true;
            }
            catch (Exception e)
            {

            }
        }

        public void Disconnect()
        {
            connection.Close();
            running = false;
            
        }

        public void SendMsg(string msg)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            connection.Send(data);
        }
        public void WaitToRecieveData()
        {
            byte[] data = new byte[512];
            running = true;
            while (running)
            {
                try
                {
                    int byteRead = connection.Receive(data);
                    string msg = System.Text.Encoding.UTF8.GetString(data, 0, byteRead);
                    if (msg.StartsWith("@quit"))
                    {
                        Disconnect();
                    }
                    else
                    {
                        ReceivedMsg?.Invoke(this, msg);
                    }
                }
                catch(Exception)
                {

                }
            }
            
        }

    }
}
