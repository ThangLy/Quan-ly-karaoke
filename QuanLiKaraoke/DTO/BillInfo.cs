using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKaraoke.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int buildID, int menuID, int count1)
        {
            this.ID = id;
            this.buildID = buildID;
            this.menuID = menuID;
            this.count1 = count1;
        }

        public BillInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.buildID = (int)row["idbill"];
            this.menuID = (int)row["idmenu"];
            this.count1 = (int)row["count1"];
        }

        private int count1;

        public int Count
        {
            get { return count1; }
            set { count1 = value; }
        }

        private int menuID;

        public int MenuID
        {
            get { return menuID; }
            set { menuID = value; }
        }

        private int buildID;

        public int BuildID
        {
            get { return buildID; }
            set { buildID = value; }
        }

        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }
}
