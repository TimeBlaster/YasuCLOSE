using Hardcodet.Wpf.TaskbarNotification;
using LoLInfo;
using RiotNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YasuoCLOSE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Settings settings = Utilities.GetSettings();

            Utilities.SetStartup(settings.StartWithWindows); 

            TaskbarIcon tbi = new TaskbarIcon();
            tbi.Icon = Utilities.GetNotifyIcon();
            tbi.Visibility = Visibility.Visible;

            var client = new RiotClient(settings.ApiKey, settings.PlatformId);
                       
            //TODO: make real CancellationToken
            var ct = new CancellationToken();

            Task.Run(() => Utilities.YasuoCLOSE(client, settings, ct));

            while (!ct.IsCancellationRequested)           
            {
     
            }
        }
    }
}
