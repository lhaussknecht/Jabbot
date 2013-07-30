﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Jabbot.Models;
using JabbR.Client.Models;

namespace Jabbot.Core
{
    public interface IBot
    {
        string Name { get; }
        ICredentials Credentials { get; }
        event Action Disconnected;
        event Action<Message, string> MessageReceived;

        void StartUp();
        void ShutDown();
        void JoinRoom(string room);
        void CreateRoom(string room);       
        void PrivateReply(string toName, string message);
        void Send(string message, string room);
        void WriteDebugInfo(string message);
        

        
    }
}
