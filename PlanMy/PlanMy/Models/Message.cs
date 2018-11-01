using Humanizer;
using MvvmHelpers;
using System;


namespace PlanMy.Models
{
    public class WeddexChat
    {
        public string message { get; set; }
        public string type { get; set; }
        public string dateInsert { get; set; }
    }
    public class Message : ObservableObject
    {
        string text;

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        string senderImg;

        public string SenderImg
        {
            get { return senderImg; }
            set { SetProperty(ref senderImg, value); }
        }
        DateTime messageDateTime;

        public DateTime MessageDateTime
        {
            get { return messageDateTime; }
            set { SetProperty(ref messageDateTime, value); }
        }

        public string MessageTimeDisplay => MessageDateTime.Humanize();

        bool isIncoming;

        public bool IsIncoming
        {
            get { return isIncoming; }
            set { SetProperty(ref isIncoming, value); }
        }

        public bool HasAttachement => !string.IsNullOrEmpty(attachementUrl);

        string attachementUrl;

        public string AttachementUrl
        {
            get { return attachementUrl; }
            set { SetProperty(ref attachementUrl, value); }
        }

    }
}
