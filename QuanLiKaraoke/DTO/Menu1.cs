using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKaraoke.DTO
{
    public class Menu1
    {
        public Menu1(string foodName, int count1, float price, float totalprice = 0)
        {
            this.FoodName = foodName;
            this.Count1 = count1;
            this.Price = price;
            this.TotalPrice = totalprice;
        }

        public Menu1(DataRow row)
        {
            this.FoodName = row["Name"].ToString();
            this.Count1 = (int)row["count1"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }

        private float totalPrice;

        public float TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        private float price;

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        private int count1;

        public int Count1
        {
            get { return count1; }
            set { count1 = value; }
        }

        private string foodName;

        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }
    }
}
