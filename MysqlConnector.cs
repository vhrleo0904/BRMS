using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows;
using WPF_Project.Domain;
using System.Collections.Generic;

namespace WPF_Project
{
    class MysqlConnector
    {
        private const string STR_SIGNUP = "회원가입";
        private const string STR_SIGNIN = "로그인";

        private const string MSG_SUC = "에 성공하였습니다";
        private const string MSG_FAIL = "에 실패했습니다";
        private const string MSG_ERR = "오류가 발생하였습니다";

        private const string TITLE_INF = "알림";
        private const string TITLE_ERR = "오류";

        private string StrSQL = ConfigurationManager.AppSettings["DBConn"];
        private HashConvert hc = new HashConvert();

        private MessageBoxButton mb_OK = MessageBoxButton.OK;

        public bool SignUp(string id, string password, string name, string email, string phone)
        {
            //try
            //{
                var Conn = new MySqlConnection(StrSQL);
                Conn.Open();

                string Sql = "insert into member(member_id, member_password, member_name, member_email, member_phone)";
                Sql += " values('" + id + "', '" + hc.ConvertSha256(password)
                            + "', '" + name + "', '" + email + "', '" + phone + "')";

                var Comm = new MySqlCommand(Sql, Conn);
                int i = Comm.ExecuteNonQuery();

                if (i == 1)
                {
                    MessageBox.Show(STR_SIGNUP + MSG_SUC, TITLE_INF, mb_OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show(STR_SIGNUP + MSG_FAIL, TITLE_INF, mb_OK, MessageBoxImage.Information);
                }
                Conn.Close();
                return false;
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show(MSG_ERR, TITLE_ERR, MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}
        }

        public bool IDCheck(string id)
        {
            try
            {
                var Conn = new MySqlConnection(StrSQL);
                Conn.Open();

                string sql = "select * from member where member_id='" + id + "'";

                var Comm = new MySqlCommand(sql, Conn);
                var ReadDB = Comm.ExecuteReader();
                // System.Diagnostics.Debug.WriteLine(ReadDB[0]);

                while (ReadDB.Read())
                {
                    MessageBox.Show("동일한 아이디가 존재합니다", TITLE_INF, mb_OK, MessageBoxImage.Information);
                    ReadDB.Close();
                    Conn.Close();
                    return false;
                }

                ReadDB.Close();
                Conn.Close();

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(MSG_ERR, TITLE_ERR, mb_OK, MessageBoxImage.Error);
                return false;
            }
        }

        public Member SignIn(string id, string password)
        {
            try
            {
                var Conn = new MySqlConnection(StrSQL);
                Conn.Open();

                string sql = "select * from member where member_id='" + id + "' and member_password='" + hc.ConvertSha256(password) + "'";

                var Comm = new MySqlCommand(sql, Conn);
                var ReadDB = Comm.ExecuteReader();
                // System.Diagnostics.Debug.WriteLine(ReadDB[0]);

                while (ReadDB.Read())
                {
                    Member member = new Member();
                    member.Sequence = int.Parse(ReadDB[0].ToString());
                    member.Id = ReadDB[1].ToString();
                    member.Password = password;
                    member.Name = ReadDB[3].ToString();
                    member.Email = ReadDB[4].ToString();
                    member.Phone = ReadDB[5].ToString();

                    MessageBox.Show(member.Id + "님, 환영합니다.", TITLE_INF, mb_OK, MessageBoxImage.Information);
                    ReadDB.Close();
                    Conn.Close();
                    return member;
                }

                MessageBox.Show(STR_SIGNIN + MSG_FAIL, TITLE_INF, mb_OK, MessageBoxImage.Information);
                ReadDB.Close();
                Conn.Close();
                return null;
            }
            catch (Exception)
            {
                MessageBox.Show(MSG_ERR, TITLE_ERR, mb_OK, MessageBoxImage.Error);
                return null;
            }
        }

        public List<Book> SelectAllBook()
        {
            List<Book> books = new List<Book>();
            try
            {
                var Conn = new MySqlConnection(StrSQL);
                Conn.Open();

                string sql = "select * from book";

                var Comm = new MySqlCommand(sql, Conn);
                var ReadDB = Comm.ExecuteReader();
                // System.Diagnostics.Debug.WriteLine(ReadDB[0]);

                while (ReadDB.Read())
                {
                    Book book = new Book();
                    book.Id = int.Parse(ReadDB[0].ToString());
                    book.RegistId = ReadDB[1].ToString();
                    book.Name = ReadDB[2].ToString();
                    book.Vol = ReadDB[3].ToString();
                    book.VolExp = ReadDB[4].ToString();
                    book.Writer = ReadDB[5].ToString();
                    book.Publisher = ReadDB[6].ToString();
                    book.PublishYear = int.Parse(ReadDB[7].ToString());
                    book.CallNumber = ReadDB[8].ToString();
                    book.RegistDate = DateTime.Parse(ReadDB[9].ToString());
                    book.State = ReadDB[10].ToString();

                    books.Add(book);
                }

                ReadDB.Close();
                Conn.Close();
                return books;
            }
            catch (Exception)
            {
                MessageBox.Show(MSG_ERR, TITLE_ERR, mb_OK, MessageBoxImage.Error);
                return null;
            }
        }

        public List<Book> Select(string column, string search)
        {
            List<Book> books = new List<Book>();

            if(column.Equals("전체"))
            {
                return SelectAllBook();
            }

            try
            {
                var Conn = new MySqlConnection(StrSQL);
                Conn.Open();

                string sql = "select * from book where " + column + " like '%" + search + "%'";

                var Comm = new MySqlCommand(sql, Conn);
                var ReadDB = Comm.ExecuteReader();

                while (ReadDB.Read())
                {
                    Book book = new Book();
                    book.Id = int.Parse(ReadDB[0].ToString());
                    book.RegistId = ReadDB[1].ToString();
                    book.Name = ReadDB[2].ToString();
                    book.Vol = ReadDB[3].ToString();
                    book.VolExp = ReadDB[4].ToString();
                    book.Writer = ReadDB[5].ToString();
                    book.Publisher = ReadDB[6].ToString();
                    book.PublishYear = int.Parse(ReadDB[7].ToString());
                    book.CallNumber = ReadDB[8].ToString();
                    book.RegistDate = DateTime.Parse(ReadDB[9].ToString());
                    book.State = ReadDB[10].ToString();

                    books.Add(book);
                }

                ReadDB.Close();
                Conn.Close();

                return books;
            }
            catch (Exception)
            {
                MessageBox.Show(MSG_ERR, TITLE_ERR, mb_OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
