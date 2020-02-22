/*
 * MIT License
 * 
 * Copyright (c) 2020 SamyLearningNote
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * GitHub:
 * https://github.com/SamyLearningNote
 */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace PrintingReminderWindow
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = "";
        int numberOfMinute = 0;
        CommonSet commonSet = new CommonSet();
        PrinterFunctions printerFunctions = new PrinterFunctions();
        public MainWindow()
        {
            InitializeComponent();

            // set default file path
            SetDefaultPath();

            // show scheduled time
            UpdateScheduledTime();
        }

        void UpdateScheduledTime()
        {

            commonSet.LoadNextTime();
            ScheduledTimeTextBlock.Text = commonSet.loadedNextPrintingTime.ToString();
        }

        private void ImagePathBrowsingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image|*.jpg;*.png|All files|*";
            fileDialog.Multiselect = false;

            DialogResult result = fileDialog.ShowDialog(); // Show the dialog.
            if (result.ToString().Equals("OK"))
            {
                filePath = fileDialog.FileName;
                FilePathTextBox.Text = filePath;
            }
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            // get the number of minute
            if (PostponeComboBox.SelectedItem.Equals(OneMinItem))
            {
                numberOfMinute = 1;
            }
            else if (PostponeComboBox.SelectedItem.Equals(FiveMinItem))
            {
                numberOfMinute = 5;
            }
            else if (PostponeComboBox.SelectedItem.Equals(TenMinItem))
            {
                numberOfMinute = 10;
            }
            else if (PostponeComboBox.SelectedItem.Equals(ThirtyMinItem))
            {
                numberOfMinute = 30;
            }
            else if (PostponeComboBox.SelectedItem.Equals(OneHourItem))
            {
                numberOfMinute = 60;
            }
            else if (PostponeComboBox.SelectedItem.Equals(EightHourItem))
            {
                numberOfMinute = 480;
            }
            else if (PostponeComboBox.SelectedItem.Equals(OneDayItem))
            {
                numberOfMinute = 1440;
            }
            commonSet.PostponePrintingTime(numberOfMinute);
            commonSet.SaveNextTime();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to cancel the arranged printing process?", "Cancel confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes){
                commonSet.ComputeNextTime();
                if (commonSet.SaveNextTime())
                {
                    this.Close();
                }
            }
        }

        private void DefaultPrintButton_Click(object sender, RoutedEventArgs e)
        {
            printerFunctions.SetImagePath(FilePathTextBox.Text);
            if (MessageBox.Show("Do you want to print with Windows default setting?", "Default print confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (printerFunctions.CheckFileReadable())
                {
                    printerFunctions.DefaultPrint();
                    commonSet.ComputeNextTime();
                    commonSet.SaveNextTime();
                    UpdateScheduledTime();
                }
                else
                {
                    MessageBox.Show("Your file cannot be readed\nPlease check the availability of the choosen file");
                }
            }
        }

        private void AdvancePritnButton_Click(object sender, RoutedEventArgs e)
        {
            printerFunctions.SetImagePath(FilePathTextBox.Text);
            if (printerFunctions.CheckFileReadable())
            {
                printerFunctions.AdvancePrint();
                commonSet.ComputeNextTime();
                commonSet.SaveNextTime();
                UpdateScheduledTime();
            }
            else
            {
                MessageBox.Show("Your file cannot be readed\nPlease check the availability of the choosen file");
            }
        }
        
        private void DefaultPathButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultPath();
        }

        private void SetDefaultPath()
        {
            FilePathTextBox.Text = commonSet.GetDefaultImagePath();
        }
    }
}
