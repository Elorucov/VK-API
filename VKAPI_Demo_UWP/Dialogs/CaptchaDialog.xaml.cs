using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace VKAPI_Demo_UWP.Dialogs {
    internal sealed partial class CaptchaDialog : ContentDialog {
        internal string CaptchaText { get; private set; }

        internal CaptchaDialog(Uri imageUri) {
            this.InitializeComponent();
            CaptchaText = "";
            Loaded += (a, b) => {
                img.Source = new BitmapImage(imageUri);
            };
        }

        private void Change(TextBox sender, TextBoxTextChangingEventArgs args) {
            if(!String.IsNullOrEmpty(sender.Text)) CaptchaText = sender.Text;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            CaptchaText = "";
        }
    }
}