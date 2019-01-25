using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class BudgetCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Users User { get; set; }
        public List<Budget> Budgets { get; set; }
    }
}
