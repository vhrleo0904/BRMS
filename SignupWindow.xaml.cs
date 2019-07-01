using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WPF_Project
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SignupWindow : Window, TxtCheck
    {
        private MysqlConnector mysqlConnector = new MysqlConnector();

        private string id = string.Empty;
        private string password = string.Empty;
        private string password_check = string.Empty;
        private string name = string.Empty;
        private string email = string.Empty;
        private string phone = string.Empty;

        private MessageBoxButton mb_OK = MessageBoxButton.OK;

        public SignupWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignup_Click(object sender, RoutedEventArgs e)
        {
            if(TxtCheck())
            {
                if (mysqlConnector.IDCheck(id))
                {
                    if (mysqlConnector.SignUp(id, password, name, email, phone))
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.tbxId.Text = "";
                    this.tbxId.Focus();
                }
            }
        }

        public bool TxtCheck()
        {
            id = this.tbxId.Text;
            password = this.pbxPassword.Password;
            password_check = this.pbxPasswordCheck.Password;
            name = this.tbxName.Text;
            email = this.tbxEmail.Text;
            phone = this.tbxPhone.Text;

            String message = string.Empty;

            if (id == "" || password == "" || password == "" || name == "")
            {
                MessageBox.Show("빈칸을 채워주세요.", "오류", mb_OK);
                return false;
            }
            else if(!PasswordCheck())
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.", "오류", mb_OK);
                this.pbxPassword.Password = "";
                this.pbxPasswordCheck.Password = "";
                this.pbxPassword.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void txtEmpty()
        {
            this.tbxId.Text = "";
            this.pbxPassword.Password = "";
            this.pbxPasswordCheck.Password = "";
        }

        private bool PasswordCheck()
        {
            if(password != password_check)
                return false;

            return true;
        }
    }
}
