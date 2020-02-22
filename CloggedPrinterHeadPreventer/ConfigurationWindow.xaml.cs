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

using System;
using System.Collections.Generic;
using System.Linq;
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
using Xceed.Wpf.Toolkit;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Navigation;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace CloggedPrinterHeadPreventer
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        CommonSet commonSet = new CommonSet();
        int intTemp;

        public ConfigurationWindow()
        {
        }

        // open the link
        // reference:
        // https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public void InitConfigurationWindow()
        {
            InitializeComponent();

            // load configuration from the file
            commonSet.LoadConfiguration();
            commonSet.LoadNextTime();

            // set the UI elements accroading to the loaded configuration
            InitUIElement();

        }

        public ConfigurationWindow(int flag)
        {
            if (flag == 0)
            {
                // do nothing if the flag equal to 0
            }
        }

        void InitUIElement()
        {
            if (commonSet.loadedAutoStart.ToLower().Equals("true"))
            {
                AutoStartCheckBox.IsChecked = true;
            }
            else
            {
                AutoStartCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintMon.ToLower().Equals("true"))
            {
                MonCheckBox.IsChecked = true;
                commonSet.loadedDayOfWeek[1] = true;
            }
            else
            {
                MonCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintTue.ToLower().Equals("true"))
            {
                TueCheckBox.IsChecked = true;
            }
            else
            {
                TueCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintWen.ToLower().Equals("true"))
            {
                WenCheckBox.IsChecked = true;
            }
            else
            {
                WenCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintThu.ToLower().Equals("true"))
            {
                ThuCheckBox.IsChecked = true;
            }
            else
            {
                ThuCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintFri.ToLower().Equals("true"))
            {
                FriCheckBox.IsChecked = true;
            }
            else
            {
                FriCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintSat.ToLower().Equals("true"))
            {
                SatCheckBox.IsChecked = true;
            }
            else
            {
                SatCheckBox.IsChecked = false;
            }

            if (commonSet.loadedPrintSun.ToLower().Equals("true"))
            {
                SunCheckBox.IsChecked = true;
            }
            else
            {
                SunCheckBox.IsChecked = false;
            }

            Int32.TryParse(commonSet.loadedPrintEvery, out intTemp);
            DaysUpDownControl.Value = intTemp;

            Int32.TryParse(commonSet.loadedPrintHour, out intTemp);
            HourUpDownControl.Value = intTemp;

            Int32.TryParse(commonSet.loadedPrintMinute, out intTemp);
            MinuteUpDownControl.Value = intTemp;

            Int32.TryParse(commonSet.loadedTimeSetting, out intTemp);
            if (intTemp == 1)
            {
                EveryDayRadioButton.IsChecked = true;
            }
            else if (intTemp == 2)
            {
                WeekDayRadioButton.IsChecked = true;
            }

            // show scheduled time
            ScheduledTimeTextBlock.Text = commonSet.loadedNextPrintingTime.ToString();
            
        }


        #region UI elements control
        private void DaysUpDownControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // set the configuration variable
            commonSet.printEvery = DaysUpDownControl.Value.ToString();
        }

        private void HourUpDownControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // set the configuration variable
            commonSet.printHour = HourUpDownControl.Value.ToString();
        }

        private void MinuteUpDownControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // set the configuration variable
            commonSet.printMinute = MinuteUpDownControl.Value.ToString();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // save the configuration
            commonSet.SaveConfiguration();

            // apply the autostart setting
            commonSet.ApplyAutoStartSetting(AutoStartCheckBox.IsChecked.Value);

            // save the time of next printing
            commonSet.ComputeNextTime();
            commonSet.SaveNextTime();

            MessageBox.Show("Configuration saved");

            // update scheduled time
            commonSet.LoadNextTime();
            ScheduledTimeTextBlock.Text = commonSet.loadedNextPrintingTime.ToString();

        }


        private void EveryDayRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // change the enable set of the UI
            DaysUpDownControl.IsEnabled = true;
            WeekDayRadioButton.IsChecked = false;
            MonCheckBox.IsEnabled = false;
            TueCheckBox.IsEnabled = false;
            WenCheckBox.IsEnabled = false;
            ThuCheckBox.IsEnabled = false;
            FriCheckBox.IsEnabled = false;
            SatCheckBox.IsEnabled = false;
            SunCheckBox.IsEnabled = false;

            // set the configuration variable
            commonSet.timeSetting = "1";
        }

        private void WeekDayRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // change the enable set of the UI
            DaysUpDownControl.IsEnabled = false;
            EveryDayRadioButton.IsChecked = false;
            MonCheckBox.IsEnabled = true;
            TueCheckBox.IsEnabled = true;
            WenCheckBox.IsEnabled = true;
            ThuCheckBox.IsEnabled = true;
            FriCheckBox.IsEnabled = true;
            SatCheckBox.IsEnabled = true;
            SunCheckBox.IsEnabled = true;

            // set the configuration variable
            commonSet.timeSetting = "2";
        }

        private void AutoStartCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // set the configuration variable
            commonSet.autoStart = "true";
        }

        private void AutoStartCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // set the configuration variable
            commonSet.autoStart = "false";
        }

        private void MonCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printMon = "true";
        }

        private void TueCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printTue = "true";

        }

        private void WenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printWen = "true";

        }

        private void ThuCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printThu = "true";

        }

        private void FriCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printFri = "true";

        }

        private void SatCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printSat = "true";

        }

        private void SunCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            commonSet.printSun = "true";

        }

        private void MonCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printMon = "false";

        }

        private void TueCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printTue = "false";

        }

        private void WenCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printWen = "false";

        }

        private void ThuCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printThu = "false";

        }

        private void FriCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printFri = "false";

        }

        private void SatCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printSat = "false";

        }

        private void SunCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            commonSet.printSun = "false";

        }
        #endregion
    }
}
