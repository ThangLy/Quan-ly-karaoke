using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLiKaraoke.DAO;
using QuanLiKaraoke.DTO;

namespace QuanLiKaraoke
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();
            loadRoom();
            LoadCategory();
            LoadComboBoxRoom(cbSwitchRoom);
        }
        #region Method

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadMenuListCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void loadRoom()
        {
            flpTable.Controls.Clear();
            List<Room> tablelist = RoomDAO.Instance.LoadRoomList();
            foreach (Room item in tablelist)
            {
                Button btn = new Button() { Width = RoomDAO.TableWidth, Height = RoomDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }
                //btn.Image = System.Drawing.Image.FromFile(@"C:\Users\user\Desktop\QuanLiKaraoke\QuanLiKaraoke\karaoke.jpg");
                flpTable.Controls.Add(btn);
            }
        }

        void LoadComboBoxRoom(ComboBox cb)
        {
            cb.DataSource = RoomDAO.Instance.LoadRoomList();
            cb.DisplayMember = "Name";
        }

        void showBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu1> listBillInfo = MenuDAO.Instance.GetListMenuByRoom(id);
            float totalPrice = 0;
            foreach (Menu1 item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count1.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }
             lsvTime.Items.Clear();

            List<GetTime> listBillInfoItem = GetTimeDAO.Instance.GetListTimeByRoom(id);
            float totalPriceTime = 0;
            foreach (GetTime item in listBillInfoItem)
            {
                ListViewItem lsvItem = new ListViewItem(item.DateCheckIn.ToString());
                lsvItem.SubItems.Add(item.DateCheckOut.ToString());
                lsvItem.SubItems.Add(item.IdRoom.ToString());
                lsvItem.SubItems.Add(item.TotalTime.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPriceTime.ToString());
                totalPriceTime += item.TotalPriceTime;


                lsvTime.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            float totalPriceBill = totalPriceTime + totalPrice;
            txbTotalPrice.Text = totalPriceBill.ToString("c", culture);

        }
         /*void showTime(int id)
        {
            lsvTime.Items.Clear();
            List<GetTime> listBillInfo = GetTimeDAO.Instance.GetListTimeByRoom(id);
            float totalPriceTime = 0;
            foreach (GetTime item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.DateCheckIn.ToString());
                lsvItem.SubItems.Add(item.DateCheckOut.ToString());
                lsvItem.SubItems.Add(item.IdRoom.ToString());
                lsvItem.SubItems.Add(item.TotalTime.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPriceTime.ToString());
                totalPriceTime += item.TotalPriceTime;


                lsvTime.Items.Add(lsvItem);
            }

        }*/

        #endregion

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int RoomID = ((sender as Button).Tag as Room).ID;
            lsvBill.Tag = (sender as Button).Tag;
            showBill(RoomID);
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }

        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadMenuListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Room).ID);
            loadRoom();
        }

        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadMenuListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Room).ID);
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadMenuListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Room).ID);
        }


        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadMenuListCategoryID(id);
        }
        private void btnAddMenu_Click(object sender, EventArgs e)
        {
            Room room = lsvBill.Tag as Room;

            if (room == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckIDByRoomID(room.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count1 = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(room.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count1);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count1);
            }
            loadRoom();
            showBill(room.ID);
        }
        private void btnCheckOutTime_Click(object sender, EventArgs e)
        {
            Room room = lsvBill.Tag as Room;


            if (MessageBox.Show("Bạn có chắc muôn lấy thời gian kết thúc cho " + room.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                GetTimeDAO.Instance.CheckOutTime(room.ID);
                showBill(room.ID);
            }
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Room room = lsvBill.Tag as Room;

            int idBill = BillDAO.Instance.GetUnCheckIDByRoomID(room.ID);
            int discount = (int)nmDiscount.Value;

            double totalPriceBill = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", ""));
            double finalTotalPrice = totalPriceBill - (totalPriceBill / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", room.Name, totalPriceBill, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    lsvBill.Items.Clear();
                    lsvTime.Items.Clear();
                    txbTotalPrice.Text = "0";
                }
                loadRoom();
            }
        }

        private void fTableManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnRoomMove_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Room).ID;
            int id2 = (cbSwitchRoom.SelectedItem as Room).ID;

            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Room).Name, (cbSwitchRoom.SelectedItem as Room).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                RoomDAO.Instance.SwitchRoom(id1, id2);
                loadRoom();
            }
        }

       #endregion

 

        }


}

