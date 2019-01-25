using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO
{
    public class Order
    {
        public int Id { get; set; }
        public Users Users { get; set; }
        public string Status { get; set; }
        [Display(Name="Reference nbr.")]
        public string ReferenceNumber { get; set; }
        [Display(Name = "Transaction UUID")]
        public string TransactionUUID { get; set; }
        [Display(Name = "Transaction date")]
        public DateTime TransactionDate { get; set; }
        public decimal Total { get; set; }
        public List<BasketItems> BasketItems { get; set; }
        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }
    }
    public enum OrderStatus
    {
        [Display(Name = "Pending Payment")]
        Pending_Payment,
        Processing,
        OnHold,
        Completed,
        Cancelled,
        Refunded,
        Failed
    }
}
