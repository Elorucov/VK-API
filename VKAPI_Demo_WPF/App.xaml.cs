using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace VKAPI_Demo_WPF {
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += (a, b) => {
                Exception ex = b.ExceptionObject as Exception;
                MessageBox.Show($"HResult: {ex.HResult.ToString("x8")}\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
}
