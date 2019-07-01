using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPF_Project.Domain;

namespace WPF_Project
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Member member;
        private MysqlConnector mysqlConnector = new MysqlConnector();

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public MainWindow()
        {
            InitializeComponent();

            SetListView();
        }

        public void SetMember(Member member)
        {
            this.member = member;
            this.tbxWelcome.Text = this.member.Name + this.tbxWelcome.Text;
            this.Title = this.member.Name + this.Title;
        }

        public void SetListView()
        {
            List<Book> books = mysqlConnector.SelectAllBook();

            this.listViewBooks.ItemsSource = books;
        }

        public void Header_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(this.listViewBooks.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxSearch.Text == "")
            {
                ImageBrush tbxSearchImageBrush = new ImageBrush();
                tbxSearchImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri(@"C:/Users/vhrle/Documents/Visual Studio 2017/" +
                            "Source/WPF_Project/WPF_Project/Img/SEARCH.png", UriKind.Relative)
                    );
                tbxSearchImageBrush.AlignmentX = AlignmentX.Left;
                tbxSearch.Background = tbxSearchImageBrush;
            }
            else
            {
                tbxSearch.Background = null;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(this.cbxSearch.SelectedValue == null)
            {
                MessageBox.Show("카테고리를 선택해주세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (this.tbxSearch.Text.Equals(string.Empty))
            {
                MessageBox.Show("검색할 내용을 입력해주세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string column = this.cbxSearch.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            string search = this.tbxSearch.Text;

            if (column.Equals("등록번호")) column = "book_registId";
            else if (column.Equals("제목")) column = "book_name";
            else if (column.Equals("권")) column = "book_vol";
            else if (column.Equals("권 제목")) column = "book_volExp";
            else if (column.Equals("저자")) column = "book_writer";
            else if (column.Equals("발행처")) column = "book_publisher";
            else if (column.Equals("발행년도")) column = "book_publishYear";
            else if (column.Equals("청구기호")) column = "book_callNumber";
            else if (column.Equals("등록일")) column = "book_registDate";
            else if (column.Equals("자료상태")) column = "book_state";

            List<Book> books = mysqlConnector.Select(column, search);

            this.listViewBooks.ItemsSource = books;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = 
                MessageBox.Show("뒤로 가시겠습니까?", "알림", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
        }
    }
}
