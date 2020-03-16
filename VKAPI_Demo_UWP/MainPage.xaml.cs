using ELOR.VKAPILib;
using ELOR.VKAPILib.Objects.HandlerDatas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VKAPI_Demo_UWP.Dialogs;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace VKAPI_Demo_UWP
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        VKAPI API;

        private async void SetToken(object sender, RoutedEventArgs e) {
            int id = 0;
            if(String.IsNullOrEmpty(uid.Text) && Int32.TryParse(uid.Text, out id)) {
                await new MessageDialog("", "Enter valid user id!").ShowAsync();
                return;
            }
            if (String.IsNullOrEmpty(token.Password)) {
                await new MessageDialog("", "Enter token!").ShowAsync();
                return;
            }
            API = new VKAPI(id, token.Password, "ru");
            API.CaptchaHandler = CaptchaHandler;
        }

        private async Task<string> CaptchaHandler(CaptchaHandlerData obj) {
            Debug.WriteLine("Captcha handler.");
            string key = String.Empty;
            CaptchaDialog dlg = new CaptchaDialog(obj.Image);
            var r = await dlg.ShowAsync();
            if (r == ContentDialogResult.Primary) {
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
