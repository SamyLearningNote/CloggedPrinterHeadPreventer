using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CloggedPrinterHeadPreventer
{
    /// <summary>
    /// BGWorker.xaml 的互動邏輯
    /// </summary>
    public partial class BGWorker : Window
    {
        System.Windows.Forms.ContextMenu cm = new System.Windows.Forms.ContextMenu();
        System.Windows.Forms.ContextMenu rm = new System.Windows.Forms.ContextMenu();
        NotifyIcon nIcon = new NotifyIcon();
        CommonSet commonSet = new CommonSet();

        // init the objects of other window
        ConfigurationWindow configurationWindow = new ConfigurationWindow(0);

        public BGWorker()
        {
            //InitializeComponent();
            //System.Windows.Forms.MessageBox.Show("001");
            // hide the window
            this.Width = 0;
            this.Height = 0;
            this.WindowStyle = WindowStyle.None;
            this.ShowInTaskbar = false;
            this.ShowActivated = false;

            //System.Windows.Forms.MessageBox.Show("002");
            // init context menu
            System.Windows.Forms.MenuItem mwMenuItem = new System.Windows.Forms.MenuItem();
            mwMenuItem.Text = "Configuration";
            cm.MenuItems.Add(mwMenuItem);
            mwMenuItem.Click += cwMenuItem_Click;

            System.Windows.Forms.MenuItem rwMenuItem = new System.Windows.Forms.MenuItem();
            rwMenuItem.Text = "Print now";
            cm.MenuItems.Add(rwMenuItem);
            rwMenuItem.Click += rwMenuItem_Click;

            System.Windows.Forms.MenuItem eMenuItem = new System.Windows.Forms.MenuItem();
            eMenuItem.Text = "Exit";
            cm.MenuItems.Add(eMenuItem);
            eMenuItem.Click += eMenuItem_Click;
            //System.Windows.Forms.MessageBox.Show("003");

            // init the notify icon
            //System.Windows.MessageBox.Show(System.AppDomain.CurrentDomain.BaseDirectory + "icon.ico");
            nIcon.Icon = new Icon(System.AppDomain.CurrentDomain.BaseDirectory + "icon.ico");
            nIcon.Visible = true;
            nIcon.ContextMenu = cm;
            //System.Windows.Forms.MessageBox.Show("004");

            // load the configuration
            commonSet.LoadConfiguration();
            //System.Windows.Forms.MessageBox.Show("005");

            CheckIsTimeToPrint();
        }

        public void CheckIsTimeToPrint()
        {

            Task.Factory.StartNew(() =>
            {
                /*
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    ReminderWindow autoShownReminderWindow = new ReminderWindow();
                }));
                */
                while (true)
                {
                    if (commonSet.IsTimeToPrint())
                    {
                        ShowPrintingWindow();
                    }
                    Thread.Sleep(60000);
                }
            });
        }

        private void cwMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfigurationWindow();
        }

        private void rwMenuItem_Click(object sender, EventArgs e)
        {
            ShowPrintingWindow();
        }

        public void ShowPrintingWindow()
        {
            commonSet.OpenReminderWindow();
        }

        public void ShowConfigurationWindow()
        {
            configurationWindow.Close();
            configurationWindow = new ConfigurationWindow(0);
            configurationWindow.InitConfigurationWindow();
            configurationWindow.Show();
        }

        private void eMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

    }
}
