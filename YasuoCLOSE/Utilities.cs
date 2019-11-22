using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using RiotNet;
using RiotNet.Models;
using YasuoCLOSE;

namespace LoLInfo
{
    public static class Utilities
    {
        public static void SetStartup(bool startWithWindows)
        {
            var startupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

            if (startWithWindows)
            {
                //Set the application to run at startup
                RegistryKey key = Registry.CurrentUser.OpenSubKey(startupKey, true);
                key?.SetValue(Assembly.GetEntryAssembly().GetName().FullName, Assembly.GetEntryAssembly().Location);
            }
            else
            {
                //Remove the application to run at startup
                RegistryKey key = Registry.CurrentUser.OpenSubKey(startupKey, true);
                key?.DeleteValue(Assembly.GetEntryAssembly().GetName().FullName, false);
            }
        }

        public static void CloseGame(Process[] processes)
        {
            MessageBoxResult result = MessageBoxResult.None;
            while (result != MessageBoxResult.OK)
            {
                if (result != MessageBoxResult.Cancel)
                    result = MessageBox.Show("YasuCLOSE has detected a Yasuo in your game!\nThe game will now close.", "YasuoCLOSE", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
                else
                    result = MessageBox.Show("The game will now close anyway.", "YasuoCLOSE", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }

            try
            {
                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch { }

        }

        public static void YasuoCLOSE(RiotClient client, Settings settings, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                Process[] lolProcess = null;

                try
                {
                    lolProcess = Process.GetProcessesByName(settings.ProcessName);
                    if (lolProcess?.Any() == true)
                    {
                        if (GetData.YasuoInGame(client, settings.SummonerName, ct))
                            CloseGame(lolProcess);
                    }
                }
                finally
                {
                    if (lolProcess != null)
                        foreach (var p in lolProcess)
                            p.Dispose();
                }

                Thread.Sleep(500);
            }
        }

        public static Settings GetSettings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string filePath = Path.Combine(Path.GetDirectoryName(assembly.Location), "config.json");

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filePath));
        }

        public static Icon GetNotifyIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string filePath = Path.Combine(Path.GetDirectoryName(assembly.Location), "notifyIcon.ico");

            return Icon.ExtractAssociatedIcon(filePath);
        }
    }
}
