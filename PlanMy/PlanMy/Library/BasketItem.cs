using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlanMy.Library
{
    public class BasketItem
    {
        public Offers Product { get; set; }
        public BasketItem OrderItem { get; set; }
        public async Task<string> Save(List<BasketItem> items)
        {
            Connect con = new Connect();
            string basket = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            await con.SaveData("Basket", basket);
            return "";
        }
        public async Task<List<BasketItem>> Get()
        {
            List<BasketItem> items = new List<BasketItem>();
            try
            {
                Connect con = new Connect();
                var awaitable = await con.GetData("Basket");
                if (!string.IsNullOrEmpty(awaitable))
                    items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BasketItem>>(awaitable);
            }
            catch { }
            return items;
        }
        public async Task<List<BasketItem>> AddItem(BasketItem item)
        {
            var items = await this.Get();
            items.Add(item);
            await this.Save(items);
            return items;
        }
        public async Task<List<BasketItem>> RemoveItem(BasketItem item)
        {
            var items = await this.Get();
            items.Remove(item);
            await this.Save(items);
            return items;
        }
        public void Clear()
        {
            Connect con = new Connect();
            con.DeleteData("Basket");
        }
    }
}
