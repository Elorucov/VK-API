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

namespace VKAPI_Demo_WPF.Dialogs {
    /// <summary>
    /// Логика взаимодействия для CaptchaDialog.xaml
    /// </summary>
    public partial class CaptchaDialog : Window {
        public CaptchaDialog(Uri imageUri) {
            InitializeComponent();
            Loaded += (a, b) => {
                img.Source = new BitmapImage(imageUri);
            };
        }

        public string CaptchaText { get; set; }

        private void Send(object sender, RoutedEventArgs e) {
            CaptchaText = cptch.Text;
            Close();
        }
    }
}
