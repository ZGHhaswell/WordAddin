using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;

namespace WordMonitorApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var serverSettingWindow = new ServerSettingWindow();
            var dialogResult = serverSettingWindow.ShowDialog();

            if (dialogResult != null && dialogResult.Value)
            {
                //setting complete
                var mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                TcpServerCenter.Instance.Close();
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                //nosetting close
                Process.GetCurrentProcess().Kill();
            }
            base.OnStartup(e);
        }
    }
}
