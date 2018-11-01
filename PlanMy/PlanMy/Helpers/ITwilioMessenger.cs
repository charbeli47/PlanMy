using PlanMy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlanMy.Helpers
{
    public interface ITwilioMessenger
    {
        Task<bool> InitializeAsync();

        void SendMessage(string text);

        Action<Message> MessageAdded { get; set; }
    }
}
