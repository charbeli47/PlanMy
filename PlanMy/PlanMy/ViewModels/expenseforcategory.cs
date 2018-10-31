
using System.Collections.Generic;

public class Rootobjectt
{
	public List<expenseforcat> Property1 { get; set; }
}

public class expenseforcat
{
	public string budget_list_id { get; set; }
	public string budget_list_category_id { get; set; }
	public string budget_list_user_id { get; set; }
	public string budget_list_name { get; set; }
	public string budget_list_estimate_cost { get; set; }
	public string budget_list_actual_cost { get; set; }
	public string budget_list_paid_cost { get; set; }
	public string budget_list_date { get; set; }
}
