using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ELOR.VKAPILib;
using ELOR.VKAPILib.Objects.HandlerDatas;
using VKAPI_Demo_WPF.Dialogs;

namespace VKAPI_Demo_WPF {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        VKAPI API;

        private void SetToken(object sender, RoutedEventArgs e) {
            int id = 0;
            if(String.IsNullOrEmpty(uid.Text) && Int32.TryParse(uid.Text, out id)) {
                MessageBox.Show("Enter valid user id!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(String.IsNullOrEmpty(token.Password)) {
                MessageBox.Show("Enter token!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            API = new VKAPI(id, token.Password, "ru");
            API.CaptchaHandler = CaptchaHandler;
        }

        private async Task<string> CaptchaHandler(CaptchaHandlerData obj) {
            Debug.WriteLine("Captcha handler.");
            string key = String.Empty;
            CaptchaDialog dlg = new CaptchaDialog(obj.Image);
            dlg.Owner = this;
            var r = dlg.ShowDialog();
            if(r != null) {
                return dlg.CaptchaText;
            }
            return null;
        }

        private async void Captcha_Force(object sender, RoutedEventArgs e) {
            (sender as Button).IsEnabled = false;
            await API?.CallMethodAsync<string>("captcha.force");
            (sender as Button).IsEnabled = true;
        }

        private async void Users_Get(object sender, RoutedEventArgs e) {
            (sender as Button).IsEnabled = false;
            var user = await API.Users.GetAsync(API.UserId, new List<string> { "online", "photo_max_orig" }, ELOR.VKAPILib.Methods.NameCase.Nom);
            MessageBox.Show($"ID: {user.Id}\nName: {user.FirstName}\nLast: {user.LastName}", "Result");
            (sender as Button).IsEnabled = true;
        }

        private async void Polls_Create(object sender, RoutedEventArgs e) {
            (sender as Button).IsEnabled = false;
            var poll = await API.Polls.CreateAsync("Test poll from ELOR's VKAPI lib demo", new List<string> { "1", "2", "3" }, true, true, true, 0, 0, API.UserId);
            MessageBox.Show($"{poll}", "Result");
            (sender as Button).IsEnabled = true;
        }

        private async void ResolveScreenName(object sender, RoutedEventArgs e) {
            (sender as Button).IsEnabled = false;
            var response = await API.Utils.ResolveScreenNameAsync("bagledi");
            MessageBox.Show($"{response.Type}, {response.ObjectId}", "Result");
            (sender as Button).IsEnabled = true;
        }
    }
}
