using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKaraoke.DTO
{
    public class GetTime
    {
        public GetTime(int idRoom, float totalTime, DateTime? dateCheckIn, DateTime? dateCheckOut, float totalPriceTime, float price)
        {
            this.IdRoom = idRoom;
            this.TotalTime = totalTime;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.TotalPriceTime = totalPriceTime;
            this.Price = price;
        }

         public GetTime(DataRow row)
        {
            this.Price = (float)Convert.ToDouble(row["price"].ToString());

            var totalPriceTimeTemp = row["totalPrice"];
            if(totalPriceTimeTemp.ToString() != "")
                this.TotalPriceTime = (float)Convert.ToInt32(row["totalPrice"].ToString());
            this.IdRoom = (int)row["idRoom"];
            var totalTimeTemp = row["time1"];
             if(totalTimeTemp.ToString() != "")
            this.TotalTime = (float)Convert.ToDouble(row["time1"].ToString());
            this.dateCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.dateCheckOut = (DateTime?)dateCheckOutTemp;
        }
         private float price;

         public float Price
         {
             get { return price; }
             set { price = value; }
         }

         private float totalPriceTime;

         public float TotalPriceTime
         {
             get { return totalPriceTime; }
             set { totalPriceTime = value; }
         }



         private float totalTime;

         internal float TotalTime
         {
             get { return totalTime; }
             set { totalTime = value; }
         }



         private int idRoom;

         public int IdRoom
         {
             get { return idRoom; }
             set { idRoom = value; }
         }

        private DateTime? dateCheckOut;

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }

        private DateTime? dateCheckIn;

        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }
    }
}
