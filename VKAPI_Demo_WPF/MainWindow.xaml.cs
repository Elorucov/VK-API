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
    }
}
