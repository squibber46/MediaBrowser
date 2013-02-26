﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using Path = System.IO.Path;

namespace MediaBrowser.Uninstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //All our work is behind the scenes
            var args = Environment.GetCommandLineArgs();
            var product = args.Length > 1 ? args[1] : "server";
            //copy the real program to a temp location so we can delete everything here (including us)
            var tempExe = Path.Combine(Path.GetTempPath(), "MediaBrowser.Uninstaller.Execute.exe");
            var tempConfig = Path.Combine(Path.GetTempPath(), "MediaBrowser.Uninstaller.Execute.exe.config");
            using (var file = File.Create(tempExe, 4096, FileOptions.DeleteOnClose))
            {
                //copy the real uninstaller to temp location
                var sourceDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) ?? "";
                File.WriteAllBytes(tempExe, File.ReadAllBytes(Path.Combine(sourceDir ,"MediaBrowser.Uninstaller.Execute.exe")));
                File.Copy(tempConfig, Path.Combine(sourceDir ,"MediaBrowser.Uninstaller.Execute.exe.config"));
                //kick off the copy
                MessageBox.Show("About to start " + tempExe);
                Process.Start(tempExe, product);
                //wait for it to start up
                Thread.Sleep(500);
                //and shut down
                Close();
            }
            
            //InitializeComponent();
        }
    }
}