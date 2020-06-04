using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiKaraoke.DTO;

namespace QuanLiKaraoke.DAO
{
    public class RoomDAO
    {
        private static RoomDAO instance;

        public static RoomDAO Instance
        {
            get { if (instance == null) instance = new RoomDAO(); return RoomDAO.instance; }
            private set { RoomDAO.instance = value; }
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;

        private RoomDAO() { }

        public void SwitchRoom(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTabel2", new object[] { id1, id2 });
        }


        public List<Room> LoadRoomList()
        {
            List<Room> tableList = new List<Room>();

            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC dbo.USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Room table = new Room(item);
                tableList.Add(table);
            }

            return tableList;
        }
    }
}
