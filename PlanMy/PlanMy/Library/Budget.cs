using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class Budget
    {
        public int Id { get; set; }
        public BudgetCategory BudgetCategory { get; set; }
        public int BudgetCategoryId { get; set; }
        public string Description { get; set; }
        public float EstimatedCost { get; set; }
        public float ActualCost { get; set; }
        public float PaidCost { get; set; }
        public string Notes { get; set; }
        public Users User { get; set; }
        public string UserId { get; set; }
    }
}
