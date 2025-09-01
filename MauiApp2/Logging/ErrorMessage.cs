using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiApp2.Logging
{
    public class ErrorMessage(string title, string message) : ValueChangedMessage<string>(message)
    {
        public string Title { get; } = title;
    }
}
