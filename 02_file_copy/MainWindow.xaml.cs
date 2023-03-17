using IOExtensions;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
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
using Path = System.IO.Path;

namespace _02_file_copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel()
            {
                Source = "D:\\OneDrive - ITSTEP\\MyMaterials\\2 семестр\\ООП\\C++ " +
                "- Exceptions-20210623_094011-Meeting Recording.mp4",
                Destination = "C:\\Users\\helen\\Desktop\\Test",
                Progress = 0

            };
            this.DataContext = model;
        }

        private async void CopyButtonClick(object sender, RoutedEventArgs e)
        {
           
            string filename = Path.GetFileName(model.Source);
            string destFilePath = Path.Combine(model.Destination, filename);

            //add item to the list
            CopyProcessesInfo info = new CopyProcessesInfo(filename);
            model.AddProgress(info);
            await CopyFileAsync(model.Source, destFilePath, info);

            MessageBox.Show("Complited!");
        }

        private Task CopyFileAsync(string src, string dest, CopyProcessesInfo info)
        {
            #region Type1 Type2
            // type 1  - using File class
            //File.Copy(Source, destFilePath, true);

            // type 2  - using FileStream class
            //return Task.Run(() =>
            //{
            //    
            //    using FileStream sourseStream = new FileStream(src, FileMode.Open, FileAccess.Read);//12Kb
            //    using FileStream destStream = new FileStream(dest, FileMode.Create, FileAccess.Write);

            //    byte[] buffer = new byte[1024 * 8];//8Kb
            //    int bytes = 0;
            //    do
            //    {
            //        bytes = sourseStream.Read(buffer, 0, buffer.Length);//0.5

            //        destStream.Write(buffer, 0, bytes); //8
            //                                            // % = total  received 
            //                                            //float percent = (destStream.Length / sourseStream.Length) * 100;
            //        float percent = destStream.Length / (sourseStream.Length / 100);
            //        model.Progress = percent;


            //    } while (bytes > 0);//22
            //});
            #endregion
            // type 3  - using FileTransferManager class
            return FileTransferManager.CopyWithProgressAsync(src, dest, (progress) =>
            {
                model.Progress = progress.Percentage;
                info.Percentage = progress.Percentage;
                info.BytesPerSecond = progress.BytesPerSecond;
            }, false);
        }
        private void OpenSourceClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ( dialog.ShowDialog() == true)
            {
                model.Source = dialog.FileName;
                //srcTexbox.Text = dialog.FileName;
            }
        }
        private void OpenDestClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = dialog.FileName;
            }
        }
    }
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        private ObservableCollection<CopyProcessesInfo> processes ;
        public ViewModel()
        {
            processes = new ObservableCollection<CopyProcessesInfo>();
        }
        public IEnumerable<CopyProcessesInfo> Processes => processes;
        public string Source { get; set; }
        public string Destination { get; set; }
        public double Progress { get; set; }
        public bool IsWaiting  => Progress == 0;
        public void AddProgress(CopyProcessesInfo info)
        {
            processes.Add(info);
        }

    }
    [AddINotifyPropertyChangedInterface]
    class CopyProcessesInfo
    {
        public CopyProcessesInfo(string filename)
        {
            FileName = filename;
        }
        public string FileName { get; set; }
        public double Percentage { get; set; }
        public int PercentageInt => (int)Percentage;
        public double BytesPerSecond { get; set; }
        public double MegaBytesPerSecond => Math.Round(BytesPerSecond / 1024 / 1024 ,1);//Mb/s

    }
}
