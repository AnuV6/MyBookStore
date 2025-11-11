using System.Windows;
using System.Windows.Input;
using System.Linq;
using MyBookStore.Data;
using System.Configuration;
using System.Windows.Controls;


namespace MyBookStore
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameBox.Focus();
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            var savedUsername = ConfigurationManager.AppSettings["SavedUsername"];
            var savedPassword = ConfigurationManager.AppSettings["SavedPassword"];

            if (!string.IsNullOrEmpty(savedUsername))
            {
                UsernameBox.Text = savedUsername;
                RememberMeCheckBox.IsChecked = true;
                if (!string.IsNullOrEmpty(savedPassword))
                {
                    PasswordBox.Password = savedPassword;
                }
            }
        }

        private void SaveCredentials(string username, string password)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (RememberMeCheckBox.IsChecked == true)
            {
                config.AppSettings.Settings["SavedUsername"].Value = username;
                config.AppSettings.Settings["SavedPassword"].Value = password;
            }
            else
            {
                config.AppSettings.Settings.Remove("SavedUsername");
                config.AppSettings.Settings.Remove("SavedPassword");
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void ClearErrors()
        {
            UsernameErrorText.Text = "";
            UsernameErrorText.Visibility = Visibility.Collapsed;
            PasswordErrorText.Text = "";
            PasswordErrorText.Visibility = Visibility.Collapsed;
        }

        private void ShowUsernameError(string message)
        {
            UsernameErrorText.Text = message;
            UsernameErrorText.Visibility = Visibility.Visible;
            UsernameBox.Focus();
        }

        private void ShowPasswordError(string message)
        {
            PasswordErrorText.Text = message;
            PasswordErrorText.Visibility = Visibility.Visible;
            PasswordBox.Focus();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username))
            {
                ShowUsernameError("Please enter your username.");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                ShowPasswordError("Please enter your password.");
                return;
            }

            // Show loading state
            var loginButton = sender as Button ?? FindName("LoginButton") as Button;
            if (loginButton != null)
            {
                loginButton.Content = "Signing In...";
                loginButton.IsEnabled = false;
            }

            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user != null && user.PasswordHash == password)
                {
                    SaveCredentials(username, password);
                    var dashboard = new DashboardWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    ShowPasswordError("Invalid username or password.");
                    PasswordBox.Clear();
                }
            }

            // Reset loading state
            if (loginButton != null)
            {
                loginButton.Content = "Sign In";
                loginButton.IsEnabled = true;
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            var forgotPasswordWindow = new Window
            {
                Title = "Forgot Password",
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = System.Windows.Media.Brushes.White,
                WindowStyle = WindowStyle.None
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Close button
            var closeButton = new Button
            {
                Content = "✕",
                Width = 30,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10),
                Background = System.Windows.Media.Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };
            closeButton.Click += (s, args) => forgotPasswordWindow.Close();
            Grid.SetRow(closeButton, 0);
            grid.Children.Add(closeButton);

            // Content
            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(20)
            };
            Grid.SetRow(stackPanel, 1);

            var title = new TextBlock
            {
                Text = "Reset Password",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);

            var instruction = new TextBlock
            {
                Text = "Enter your username to reset your password",
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            };
            stackPanel.Children.Add(instruction);

            var usernameBox = new TextBox
            {
                Width = 250,
                Height = 35,
                Margin = new Thickness(0, 0, 0, 15),
                Padding = new Thickness(10)
            };
            stackPanel.Children.Add(usernameBox);

            var resetButton = new Button
            {
                Content = "Send Reset Link",
                Width = 120,
                Height = 35,
                Background = System.Windows.Media.Brushes.DodgerBlue,
                Foreground = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0)
            };
            resetButton.Click += (s, args) =>
            {
                if (string.IsNullOrEmpty(usernameBox.Text.Trim()))
                {
                    MessageBox.Show("Please enter your username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Password reset link has been sent to your email.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                forgotPasswordWindow.Close();
            };
            stackPanel.Children.Add(resetButton);

            grid.Children.Add(stackPanel);

            // Border for rounded corners
            var border = new Border
            {
                CornerRadius = new CornerRadius(10),
                Background = System.Windows.Media.Brushes.White,
                Child = grid
            };
            border.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                Color = System.Windows.Media.Colors.Black,
                Direction = 270,
                ShadowDepth = 5,
                BlurRadius = 20,
                Opacity = 0.3
            };

            forgotPasswordWindow.Content = border;
            forgotPasswordWindow.ShowDialog();
        }
    }
}
