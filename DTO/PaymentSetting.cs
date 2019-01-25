using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class PaymentSetting
    {
        public int Id { get; set; }
        public string ProfileId { get; set; }
        public string AccessKey { get; set; }
        public string SecuritySign { get; set; }
        public PaymentType PaymentType { get; set; }
        public string RecurringFrequency { get; set; }
    }
    public enum PaymentType
    {
        Sale,
        Tokenization,
        Recurring
    }
}
