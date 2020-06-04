using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLiKaraoke.DAO;
using QuanLiKaraoke.DTO;

namespace QuanLiKaraoke
{
    public partial class Admin : Form
    {
        BindingSource MenuList = new BindingSource();
        BindingSource AccountList = new BindingSource();

        public Account loginAccount;
        public Admin()
        {
            InitializeComponent();
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFormDate.Value, dtpkToDate.Value);
            Load();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFormDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFormDate.Value.AddMonths(1).AddDays(-1);
        }
        void Load()
        {
            dtgvMenu.DataSource = MenuList;
            dtgvAccount.DataSource = AccountList;
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbMenuCategory);
            AddFoodBinding();
            AddAccountBinding();
        }
        
        #region methods
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            txbPassword.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Password", true, DataSourceUpdateMode.Never));
            nmTypeAccount.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type1", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void AddAccount(string userName, string displayName, string password, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, password, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            //if (loginAccount.UserName.Equals(userName))
            //{
            //    MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
            //    return;
            // }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }
        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        void LoadListFood()
        {
            MenuList.DataSource = FoodDAO.Instance.GetListMenu();
        }
        void AddFoodBinding()
        {
            txbMenuName.DataBindings.Add(new Binding("Text", dtgvMenu.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbMenuID.DataBindings.Add(new Binding("Text", dtgvMenu.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmMenuPrice.DataBindings.Add(new Binding("Value", dtgvMenu.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
 

        #endregion

        #region events
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbName.Text;
            string password = txbPassword.Text;
            int type = (int)nmTypeAccount.Value;

            AddAccount(userName, displayName, password, type);
        }

        private void btnResetAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPass(userName);
        }

        private void btnDeteleAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            DeleteAccount(userName);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFormDate.Value, dtpkToDate.Value);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void txbMenuName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvMenu.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvMenu.SelectedCells[0].OwningRow.Cells["Category"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbMenuCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbMenuCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbMenuCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddMenu_Click(object sender, EventArgs e)
        {
            string name = txbMenuName.Text;
            int categoryID = (cbMenuCategory.SelectedItem as Category).ID;
            float price = (float)nmMenuPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnEditMenu_Click(object sender, EventArgs e)
        {
            string name = txbMenuName.Text;
            int categoryID = (cbMenuCategory.SelectedItem as Category).ID;
            float price = (float)nmMenuPrice.Value;
            int id = Convert.ToInt32(txbMenuID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteMenu_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbMenuID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private void btnSearchMenu_Click(object sender, EventArgs e)
        {
           MenuList.DataSource = SearchFoodByName(txbSearchMenu.Text);
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
         #endregion 




    }
}
