using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class UserPaymentToken
    {
        public int Id { get; set; }
        public PaymentSetting PaymentSetting { get; set; }
        public string Token { get; set; }
        public Users User { get; set; }
        public TokenStatus TokenStatus { get; set; }
    }
    public enum TokenStatus
    {
        Valid,
        InValid
    }
}
