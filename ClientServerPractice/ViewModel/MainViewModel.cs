using Client.Communication;
using ClientServerPractice;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string chatName;

        public string ChatName
        {
            get { return chatName; }
            set { chatName = value; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private RelayCommand connectBtn;

        public RelayCommand ConnectBtn
        {
            get { return connectBtn; }
            set { connectBtn = value; }
        }

        private RelayCommand sendBtn;

        public RelayCommand SendBtn
        {
            get { return sendBtn; }
            set { sendBtn = value; } 
        } 

        public ObservableCollection<string> MsgDisplay { get; set; }
        public bool isConnected = false;
        private Connection client;

        public MainViewModel()
        {
            Message = "";
            ChatName = "";
            MsgDisplay = new ObservableCollection<string>();
            ConnectBtn = new RelayCommand(Connect,() => !isConnected && ChatName.Length > 0);
            SendBtn = new RelayCommand(SendMsg,() =>isConnected && Message.Length>0);
        }

        private void SendMsg()
        {
            if (Message.StartsWith("@quit"))
            {
                isConnected = false;
            }
            else
            {
                MsgDisplay.Add("YOU: " + Message + "\n");
                client.SendMsg(ChatName + ": " + Message);
            }
            isConnected = true;
        }

        private void Connect()
        {
            try
            {
                client.Connect(ChatName);
                client.ReceivedMsg += Client_ReceivedMsg;
                isConnected = true;
            }
            catch (Exception e)
            {
            }
            
        }

        private void Client_ReceivedMsg(object sender, string e)
        {
            if (e.StartsWith("@quit"))
            {
                isConnected = false;
            }
            else
            {
                App.Current.Dispatcher.Invoke(() => { MsgDisplay.Add(e); });
            }
        }
    }
}
