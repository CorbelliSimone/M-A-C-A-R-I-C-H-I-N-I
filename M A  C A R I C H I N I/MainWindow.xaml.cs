using M_A__C_A_R_I_C_H_I_N_I.Model;
using M_A__C_A_R_I_C_H_I_N_I.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace M_A__C_A_R_I_C_H_I_N_I
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void OnInit(object sender, RoutedEventArgs e)
        {
            try
            {
                InitFunction();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void InitFunction()
        {
            //var a = 
            CsvPath.Text = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\File\\dump.csv";

            Process currentProc = Process.GetCurrentProcess();
            //MessageBox.Show(currentProc.PrivateMemorySize64.ToString());
            long memoryUsed = currentProc.WorkingSet64 / 1024 / 1024;
            LblMemoryNow.Content = memoryUsed.ToString() + " mb ram usata";

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();


            String[] Header = { "Titolo", "TorrentLink", "Descrizione" };

            for (int i = 0; i < Header.Length; i++)
            {
                ((GridView)TntModelGrid.View).Columns[i].Header = Header[i];
            }
            try
            {
                LoadCsv();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            (new Thread(() => {
                Process currentProc = Process.GetCurrentProcess();
                long memoryUsed = currentProc.WorkingSet64 / 1024/1024;
                if (memoryUsed > 500) { GC.Collect(); GC.WaitForPendingFinalizers(); }
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    LblMemoryNow.Content = memoryUsed.ToString() + " mb ram usata";
                }));
            })).Start();
        }

        private void TntModelGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TntModelGrid_MouseDoubleClickFunction(sender, e);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void TntModelGrid_MouseDoubleClickFunction(object sender, MouseButtonEventArgs e)
        {
            var obj = (System.Windows.Controls.ListView)sender;
            var tmp = (TntModel)obj.SelectedItem;
            if (tmp == null) return;


            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Source = new Uri(tmp.TorrentLink);
            webBrowser.Dispose();


            //ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\Internet Explorer\iexplore.exe");
            //psi.Arguments = tmp.TorrentLink;

            //Process.Start(psi);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ButtonClicFunction(sender, e);
        //    }
        //    catch(Exception err)
        //    {
        //        MessageBox.Show(err.Message);
        //    }
        //}
        //private void ButtonClicFunction(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.FileOk += SetFilePath;
        //    openFileDialog.ShowDialog();
        //    //if (openFileDialog.ShowDialog() == true) { }
        //    //BrowserPath.Text = openFileDialog.;
        //}
        //private void SetFilePath(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    var obj = (Microsoft.Win32.OpenFileDialog)sender;
        //    BrowserPath.Text = obj.FileName;
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileOk += SetFileCSVPath;
            openFileDialog.Filter = "File csv|*.csv";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.DefaultExt = "csv";
            openFileDialog.ShowDialog(); //
        }
        private void SetFileCSVPath(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var obj = (Microsoft.Win32.OpenFileDialog)sender;
            //if (obj.SafeFileName.Split(".")[1] != "csv") 
            //{
            //    MessageBox.Show("File errato", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return; 
            //}


            CsvPath.Text = obj.FileName;
            try
            {
                LoadCsv();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            //  BtnLoadCsv.IsEnabled = (obj.FileName != "" && obj.FileName != null && obj.SafeFileName.Split(".")[1] == "csv") ? true : false;



        }
        private IList<TntModel> tntData;
        private void LoadCsv()
        {
            TntModelGrid.ItemsSource = null;
            LblFileCsv.Content = "Caricamento in corso...";
            // LblLoadingCsv.Visibility = Visibility.Visible;
            (new Thread(() => {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => { FileUtility.FilePath = CsvPath.Text; }));
                    if (tntData != null) tntData.Clear();
                    tntData = FileUtility.OpenFile();


                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        TntModelGrid.ItemsSource = tntData;
                        LblFileCsv.Content = "File csv";
                        LblTitleSearch.Visibility = Visibility.Visible;
                    }));
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            })).Start();

        }

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    TntModelGrid.ItemsSource = null;
        //    LblLoadingCsv.Visibility = Visibility.Visible;
        //    (new Thread(() => {
        //        Application.Current.Dispatcher.Invoke(new Action(() => { FileUtility.FilePath = CsvPath.Text; }));

        //        tntData = FileUtility.OpenFile();


        //        Application.Current.Dispatcher.Invoke(new Action(() =>
        //        {
        //            TntModelGrid.ItemsSource = tntData;
        //            LblLoadingCsv.Visibility = Visibility.Hidden;
        //            LblTitleSearch.Visibility = Visibility.Visible;
        //        }));
        //    })).Start();
        //}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (System.Windows.Controls.TextBox)sender;
            if (obj.Text == "Search...") return;

            var allItemsItems = tntData;
            //var allItemsTitle = new List<TntModel>();


            var allItemsTitle = allItemsItems.Where(x => x.TITOLO.ToLower().Contains(LblTitleSearch.Text.ToLower())).ToList();


            //foreach (TntModel item in allItemsItems)
            //{
            //    //    var tmpTitle = item.TITOLO;
            //    if (item.TITOLO.ToLower().Contains(LblTitleSearch.Text.ToLower()))
            //        allItemsTitle.Add(item);
            //    //if (tmpTitle.Contains(LblTitleSearch.Text))
            //    //    allItemsTitle.Add(tmpTitle);
            //}
            TntModelGrid.ItemsSource = allItemsTitle;
         
        }

        private void LblTitleSearch_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var obj = (System.Windows.Controls.TextBox)sender;
            if (obj.Text == "Search...")
            {
                obj.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
