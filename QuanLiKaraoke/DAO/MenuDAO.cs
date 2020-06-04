using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiKaraoke.DTO;

namespace QuanLiKaraoke.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }

        private MenuDAO() { }

        public List<Menu1> GetListMenuByRoom(int id)
        {
            List<Menu1> listMenu = new List<Menu1>();

            string query = "SELECT f.name, bi.count1, f.price, f.price*bi.count1 AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Menu AS f WHERE bi.idBill = b.id and bi.idMenu = f.id AND b.status1 = 0 AND b.idRoom = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Menu1 menu = new Menu1(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }

    }
}
