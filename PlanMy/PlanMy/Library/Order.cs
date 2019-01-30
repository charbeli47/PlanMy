using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class Order
    {
        public int Id { get; set; }
        public Users Users { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string TransactionUUID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Total { get; set; }
        public List<BasketItems> BasketItems { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
    public enum OrderStatus
    {
        Pending_Payment,
        Processing,
        OnHold,
        Completed,
        Cancelled,
        Refunded,
        Failed
    }
}
