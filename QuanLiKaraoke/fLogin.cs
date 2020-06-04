using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLiKaraoke.DAO;

namespace QuanLiKaraoke
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        //public void test()
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        thread.sleep(1000);
        //        debug.writeline(i.tostring());
        //    }
        //}

        private void btnlogin_click(object sender, EventArgs e)
        {
        //    thread thread = new thread(new threadstart(test));
        //    thread.start();
                
            string userName = txbUserName.Text;
            string password = txbPassword.Text;

            if (Login(userName, password))
            {
                fTableManager f = new fTableManager();
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else 
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu");
            }
                 
        }

        bool Login(string userName, string password)
        {

            return AccountDAO.Instance.Login(userName, password);
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
        
        }
    }
}
