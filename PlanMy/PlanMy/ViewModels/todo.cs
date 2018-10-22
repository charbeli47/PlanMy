
using System.Collections.Generic;

public class Root
{
	public List<todoobj> Property1 { get; set; }
}

public class todoobj
{
	public string todo_id { get; set; }
	public string todo_user { get; set; }
	public string todo_title { get; set; }
	public string todo_date { get; set; }
	public string todo_details { get; set; }
	public string todo_read { get; set; }
	public object is_priority { get; set; }
	public object todo_category { get; set; }
}
