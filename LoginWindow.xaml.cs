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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WPF_Project
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MysqlConnector mysqlConnector = new MysqlConnector();
        private string StrSQL = ConfigurationManager.AppSettings["DBConn"];
        HashConvert hash = new HashConvert();

        private string id = string.Empty;
        private string password = string.Empty;

        private MessageBoxButton mb_OK = MessageBoxButton.OK;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TbxId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbxId.Text == "")
            {
                ImageBrush tbxIdImageBrush = new ImageBrush();
                tbxIdImageBrush.ImageSource = 
                    new BitmapImage(
                        new Uri(@"C:/Users/vhrle/Documents/Visual Studio 2017/" + 
                            "Source/WPF_Project/WPF_Project/Img/ID.png", UriKind.Relative)
                    );
                tbxIdImageBrush.AlignmentX = AlignmentX.Left;
                tbxId.Background = tbxIdImageBrush;
            }
            else
            {
                tbxId.Background = null;
            }
        }

        private void Pbx_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbx.Password == "")
            {
                ImageBrush pbxImageBrush = new ImageBrush();
                pbxImageBrush.ImageSource = 
                    new BitmapImage(
                        new Uri(@"C:/Users/vhrle/Documents/Visual Studio 2017/" + 
                            "Source/WPF_Project/WPF_Project/Img/PASSWORD.png", UriKind.Relative)
                    );
                pbxImageBrush.AlignmentX = AlignmentX.Left;
                pbx.Background = pbxImageBrush;
            }
            else
            {
                pbx.Background = null;
            }
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if(TxtCheck())
            {
                Member member = mysqlConnector.SignIn(id, password);
                if(member != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    mainWindow.SetMember(member);
                    this.Close();
                }
            }
            //if (TxtCheck())
            //{

            //    //var conn = new MySqlConnection(StrSQL);
            //    //conn.Open();

            //    //var comm = new MySqlCommand("select member_id, member_password, member_name, member_email, member_phone "
            //    //                            + "from member where member_id='" + id + "'", conn);
            //    //var ReadDB = comm.ExecuteReader();

            //    //while (ReadDB.Read())
            //    //{
            //    //    String pwd = ReadDB[1].ToString();
            //    //    if (hash.ConvertSha256(password).Equals(pwd))
            //    //    {

            //    //    }
            //    //}
            //}
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignupWindow signup = new SignupWindow();
            signup.ShowDialog();
        }

        private bool TxtCheck()
        {
            id = this.tbxId.Text;
            password = this.pbx.Password;

            String message = string.Empty;

            if(id == "" || password == "")
            {
                if (id == "") message = "아이디";
                else if (password == "") message = "비밀번호";

                MessageBox.Show(message + "를 입력해주세요.", "오류", mb_OK);
                txtEmpty();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtEmpty()
        {
            this.tbxId.Text = "";
            this.pbx.Password = "";
        }
    }
}
