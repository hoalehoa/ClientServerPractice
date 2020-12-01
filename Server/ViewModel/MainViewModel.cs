using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Server.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Server.ViewModel
{
   public class MainViewModel:ViewModelBase
    {
        public RelayCommand StartBtn { get; set; }
        public RelayCommand StopBtn { get; set; }
        public RelayCommand DropBtn { get; set; }
        public RelayCommand SaveToLogBtn { get; set; }
        public ObservableCollection<string> ConnectedUsers { get; set; }
        public ObservableCollection<string> ReceivedMsg { get; set; }
        bool running = false;
        Communication.Server server;
        public int CountMsg { get; set; }

        public MainViewModel()
        {
            ConnectedUsers = new ObservableCollection<string>();
            ReceivedMsg = new ObservableCollection<string>();
            StartBtn = new RelayCommand(Start, () => !running);
            StopBtn = new RelayCommand(Stop, () => running);
            DropBtn = new RelayCommand(Drop);
            SaveToLogBtn = new RelayCommand(SaveToLog);
            server = new Communication.Server();
            server.ClientConnected += Server_ClientConnected;
        }

        private void Server_ClientConnected(object sender, Communication.ClientHandler e)
        {
            e.ReceivedMsg += E_ReceivedMsg;
        }

        private void E_ReceivedMsg(object sender, string e)
        {
            if (e.StartsWith("@quit"))
            {
                var client = sender as ClientHandler;
                ConnectedUsers.Remove(client.Username); // remove username 
            }
            else
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var client = sender as ClientHandler;
                    ReceivedMsg.Add(client.Username + ":" + e);
                    RaisePropertyChanged(nameof(CountMsg));
                    if (!ConnectedUsers.Contains(client.Username))
                    {
                        ConnectedUsers.Add(client.Username);
                    }
                });
            }
            
        }

        private void Drop()
        {
            throw new NotImplementedException();
        }

        private void SaveToLog()
        {
            throw new NotImplementedException();
        }

        private void Stop()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            throw new NotImplementedException();
        }
    }
}
