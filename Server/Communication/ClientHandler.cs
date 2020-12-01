using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Communication
{
   public class ClientHandler
    {
        Socket connection;
        public string Username { get; private set; }
        bool running = false;
        public event EventHandler<string> ReceivedMsg;
       
        public ClientHandler(Socket connect)
        {
            this.connection = connect;
            Task.Run(WaitforData);

        }

        private void WaitforData()
        {
            byte[] data = new byte[512];
            running = true;
            while (running)
            {
                try // when the connection is forced close --> then here comes an exception (the connection. receive still wait for the data even though the client is not connected)
                {
                    int byteRead = connection.Receive(data); // save received data in the buffer
                    string msg = System.Text.Encoding.UTF8.GetString(data, 0, byteRead);
                    if (msg.StartsWith("@quit"))
                    {
                       Close();
                    }
                    else
                    {
                        int split = msg.IndexOf(":");
                        Username = msg.Substring(0, split);
                        string msgText = msg.Substring(split + 1).Trim();
                        ReceivedMsg?.Invoke(this, msgText);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void SendToClient( string msg)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            connection.Send(data);
        }

        public void Close()
        {
            connection.Close();
            running = false;
        }
    }
}
