
using System.Collections.Generic;

public class Rootobject
{
	public List<expense> Property1 { get; set; }
}

public class expense
{
	public string category_id { get; set; }
	public string category_user_id { get; set; }
	public string category_name { get; set; }
	public string category_date { get; set; }
}
