using System.Windows;
using System.Linq;
using MyBookStore.Data;


namespace MyBookStore
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameBox.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter your username.");
                UsernameBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your password.");
                PasswordBox.Focus();
                return;
            }

            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user != null && user.PasswordHash == password)
                {
                    var dashboard = new DashboardWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials.");
                    PasswordBox.Clear();
                    PasswordBox.Focus();
                }
            }
        }
    }
}
