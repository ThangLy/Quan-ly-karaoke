using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiKaraoke.DTO;

namespace QuanLiKaraoke.DAO
{
    public class GetTimeDAO
    {
        private static GetTimeDAO instance;

        public static GetTimeDAO Instance
        {
            get { if (instance == null) instance = new GetTimeDAO(); return GetTimeDAO.instance; }
            private set { GetTimeDAO.instance = value; }
        }

        private GetTimeDAO() { }

        public List<GetTime> GetListTimeByRoom(int id)
        {
            List<GetTime> listTime = new List<GetTime>();

            string query = "SELECT b.DateCheckIn , b.DateCheckOut ,b.idRoom, DATEDIFF(Minute ,b.DateCheckIn, b.DateCheckOut) as time1, hm.price, (hm.price/60)*DATEDIFF(Minute ,b.DateCheckIn, b.DateCheckOut) as totalPrice FROM dbo.Bill AS b , dbo.HourMoney AS hm WHERE b.idRoom =  " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                GetTime time = new GetTime(item);
                listTime.Add(time);
            }

            return listTime;
        }

        public void CheckOutTime(int id)
        {
            string query = "UPDATE dbo.Bill SET DateCheckOut = GETDATE() WHERE idRoom =  " + id;
            DataProvider.Instance.ExecuteQuery(query);
        
        }
        public void CheckOut(int id)
        {
            string query = "UPDATE dbo.Bill SET status1  = 1 WHERE id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
    }
}
