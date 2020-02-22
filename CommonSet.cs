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
using System.Reflection;
using System.Windows;

public class CommonSet
{
    public CommonSet()
    {
		GetCurrentTime();
    }

	// define the variables for configuration
	public string timeSetting;
	public string printEvery;
	public string printMon;
	public string printTue;
	public string printWen;
	public string printThu;
	public string printFri;
	public string printSat;
	public string printSun;
	public string printHour;
	public string printMinute;
	public string autoStart;

	public bool[] dayOfWeek = new bool[7];

	public string loadedTimeSetting = "1";
	public string loadedPrintEvery = "1";
	public string loadedPrintMon = "false";
	public string loadedPrintTue = "false";
	public string loadedPrintWen = "false";
	public string loadedPrintThu = "false";
	public string loadedPrintFri = "false";
	public string loadedPrintSat = "false";
	public string loadedPrintSun = "false";
	public string loadedPrintHour = "false";
	public string loadedPrintMinute = "false";
	public string loadedAutoStart = "false";

	public bool[] loadedDayOfWeek = new bool[7];
	/*
	public int todayDay;
	public int todayDayOfWeek;
	public int todayMonth;
	public int todayYear;
	public int currentHour;
	public int currentMinute;
	*/
	public DateTime currentTime;

	/*
	public int lastPrintingDay;
	public int lastPrintingMouth;
	public int lastPrintingYear;
	*/
	public DateTime lastPrintingDate;

	/*
	public int nextPrintingDay;
	public int nextPrintingMouth;
	public int nextPrintingYear;
	public int nextPrintingHour;
	public int nextPrintingMinute;
	*/

	public DateTime nextPrintingTime;

	public DateTime loadedNextPrintingTime;

	string configurationPath = @"\configuration";
	string timeSettingPath = @"\timeSetting";
	string defaultImagePath = @"default.jpg";

	string reminderWindowExecutable = @"\PrintingReminderWindow.exe";
	//string mainSystemExecutable = @"\CloggedPrinterHeadPreventer.exe";

	public string GetDefaultImagePath()
	{
		return System.AppDomain.CurrentDomain.BaseDirectory + defaultImagePath;
	}

	void MoveVariables()
	{

		// put the loaded configuration into the configuration variables
		timeSetting = loadedTimeSetting;
		printEvery = loadedPrintEvery;
		printMon = loadedPrintMon;
		printTue = loadedPrintTue;
		printWen = loadedPrintWen;
		printThu = loadedPrintThu;
		printFri = loadedPrintFri;
		printSat = loadedPrintSat;
		printSun = loadedPrintSun;
		printHour = loadedPrintHour;
		printMinute = loadedPrintMinute;
		autoStart = loadedAutoStart;
		dayOfWeek = loadedDayOfWeek;
	}

	public void GetCurrentTime()
	{
		currentTime = DateTime.Now;
		/*
		todayDay = (int)System.DateTime.Now.Day;
		todayDayOfWeek = (int)System.DateTime.Now.DayOfWeek;
		todayMonth = (int)System.DateTime.Now.Month;
		todayYear = (int)System.DateTime.Now.Year;
		currentHour = (int)System.DateTime.Now.Hour;
		currentMinute = (int)System.DateTime.Now.Minute;
		*/
	}


	public bool SaveNextTime()
	{
		// open the configuration file
		try
		{
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + timeSettingPath, false))
			{
				// save the configuration
				file.WriteLine("DAY " + nextPrintingTime.Day);
				file.WriteLine("MOUTH " + nextPrintingTime.Month);
				file.WriteLine("YEAR " + nextPrintingTime.Year);
				file.WriteLine("HOUR " + nextPrintingTime.Hour);
				file.WriteLine("MINUTE " + nextPrintingTime.Minute);

			}
		}
		catch
		{
			MessageBox.Show("The printing task cannot be scheduled\nPlease try it later or move the program folder to another directory");
			return false;
		}

		return true;
	}

	public bool LoadNextTime()
	{
		// open the configuration file
		try
		{
			using (System.IO.StreamReader file = new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + timeSettingPath, false))
			{
				// load the configuration
				// load the setting
				string line;
				string[] content;
				int[] timeTemp = new int[5];
				while ((line = file.ReadLine()) != null)
				{
					content = line.Split(' ');
					if (content.Length < 2)
					{
						return false;
					}
					// load language index
					if (content[0].Equals("YEAR"))
					{
						int.TryParse(content[1], out timeTemp[0]);
					}
					else if (content[0].Equals("MOUTH"))
					{
						int.TryParse(content[1], out timeTemp[1]);
					}
					else if (content[0].Equals("DAY"))
					{
						int.TryParse(content[1], out timeTemp[2]);
					}
					else if (content[0].Equals("HOUR"))
					{
						int.TryParse(content[1], out timeTemp[3]);
					}
					else if (content[0].Equals("MINUTE"))
					{
						int.TryParse(content[1], out timeTemp[4]);
					}
				}
				loadedNextPrintingTime = new DateTime(timeTemp[0], timeTemp[1], timeTemp[2], timeTemp[3], timeTemp[4], 0);
			}
		}
		catch
		{
			MessageBox.Show("The scheduled task cannot be loaded\nPlease check if the status of the files are readable");
			return false;
		}

		// return true if nothing goes wrong
		return true;
	}

	public void StoreNextDayIntoVariable(DateTime dateTemp)
	{
		int hourTemp;
		int minuteTemp;

		LoadNextTime();
		int.TryParse(loadedPrintHour, out hourTemp);
		int.TryParse(loadedPrintMinute, out minuteTemp);
		nextPrintingTime = new DateTime(dateTemp.Year, dateTemp.Month, dateTemp.Day, hourTemp, minuteTemp, 0);
	}

	public bool ComputeNextTime()
	{
		DateTime nextPrintingDate = DateTime.Now;
		if (!LoadConfiguration() || !LoadNextTime())
		{
			return false;
		}

		int intTemp;
		int.TryParse(timeSetting, out intTemp);
		if (intTemp == 1)
		{
			// the every N day setting is choosen
			int everyTemp;
			int.TryParse(loadedPrintEvery, out everyTemp);
			nextPrintingDate = nextPrintingDate.AddDays(everyTemp);
			// store the next printing date
			StoreNextDayIntoVariable(nextPrintingDate);
			return true;
		}
		else if (intTemp == 2){
			// the day in week setting is choosen
			int dayOfWeekTemp;
			for (int i = 1; i <= 7; i++)
			{
				nextPrintingDate = nextPrintingDate.AddDays(1);
				dayOfWeekTemp = (int)nextPrintingDate.DayOfWeek;
				if (dayOfWeek[dayOfWeekTemp])
				{
					// if find the closest next day end the loop here
					StoreNextDayIntoVariable(nextPrintingDate);
					return true;
				}
			}
			// if no days is choosen, set a large value to next printing day
			nextPrintingDate = new DateTime(9999, 1, 1, 1, 1, 1);
			StoreNextDayIntoVariable(nextPrintingDate);
			return true;
		}

		// if the type of time setting cannot be found, return false
		MessageBox.Show("The format of the task file is wrong\nPlease download the program again");
		return false;
	}

	public void PostponePrintingTime(int postponeMinutes)
	{
		nextPrintingTime = DateTime.Now.AddMinutes(postponeMinutes);
	}

	public bool LoadConfiguration()
	{
		// open the configuration file
		try
		{
			using (System.IO.StreamReader file = new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + configurationPath, false))
			{
				// load the configuration
				// load the setting
				string line;
				string[] content;
				while ((line = file.ReadLine()) != null)
				{
					content = line.Split(' ');
					if (content.Length < 2)
					{
						return false;
					}
					// load language index
					if (content[0].Equals("TIMESETTING"))
					{
						loadedTimeSetting = content[1];
					}
					else if (content[0].Equals("PRINTEVERY"))
					{
						loadedPrintEvery = content[1];
					}
					else if (content[0].Equals("PRINTMON"))
					{
						loadedPrintMon = content[1];
					}
					else if (content[0].Equals("PRINTTUE"))
					{
						loadedPrintTue = content[1];
					}
					else if (content[0].Equals("PRINTWEN"))
					{
						loadedPrintWen = content[1];
					}
					else if (content[0].Equals("PRINTTHU"))
					{
						loadedPrintThu = content[1];
					}
					else if (content[0].Equals("PRINTFRI"))
					{
						loadedPrintFri = content[1];
					}
					else if (content[0].Equals("PRINTSAT"))
					{
						loadedPrintSat = content[1];
					}
					else if (content[0].Equals("PRINTSUN"))
					{
						loadedPrintSun = content[1];
					}
					else if (content[0].Equals("AUTOSTART"))
					{
						loadedAutoStart = content[1];
					}
					else if (content[0].Equals("PRINTHOUR"))
					{
						loadedPrintHour = content[1];
					}
					else if (content[0].Equals("PRINTMINUTE"))
					{
						loadedPrintMinute = content[1];
					}

					// set the value of the day of week
					if (loadedPrintMon.ToLower().Equals("true"))
					{
						loadedDayOfWeek[1] = true;
					}
					else
					{
						loadedDayOfWeek[1] = false;
					}

					if (loadedPrintTue.ToLower().Equals("true"))
					{
						loadedDayOfWeek[2] = true;
					}
					else
					{
						loadedDayOfWeek[2] = false;
					}

					if (loadedPrintWen.ToLower().Equals("true"))
					{
						loadedDayOfWeek[3] = true;
					}
					else
					{
						loadedDayOfWeek[3] = false;
					}

					if (loadedPrintThu.ToLower().Equals("true"))
					{
						loadedDayOfWeek[4] = true;
					}
					else
					{
						loadedDayOfWeek[4] = false;
					}

					if (loadedPrintFri.ToLower().Equals("true"))
					{
						loadedDayOfWeek[5] = true;
					}
					else
					{
						loadedDayOfWeek[5] = false;
					}

					if (loadedPrintSat.ToLower().Equals("true"))
					{
						loadedDayOfWeek[6] = true;
					}
					else
					{
						loadedDayOfWeek[6] = false;
					}

					if (loadedPrintSun.ToLower().Equals("true"))
					{
						loadedDayOfWeek[0] = true;
					}
					else
					{
						loadedDayOfWeek[0] = false;
					}
				}
			}
			MoveVariables();
}
		catch {
			// return false if there is any exception
			MessageBox.Show("The configuration cannot be loaded\nPlease download the program again or check if the status of the files are readable");
			return false;
		}

		// return true if nothing goes wrong
		return true;
	}

	public bool SaveConfiguration()
	{
		// open the configuration file
		try
		{
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + configurationPath, false))
			{
				// save the configuration
				file.WriteLine("TIMESETTING " + timeSetting);
				file.WriteLine("PRINTEVERY " + printEvery);
				file.WriteLine("PRINTMON " + printMon);
				file.WriteLine("PRINTTUE " + printTue);
				file.WriteLine("PRINTWEN " + printWen);
				file.WriteLine("PRINTTHU " + printThu);
				file.WriteLine("PRINTFRI " + printFri);
				file.WriteLine("PRINTSAT " + printSat);
				file.WriteLine("PRINTSUN " + printSun);
				file.WriteLine("PRINTHOUR " + printHour);
				file.WriteLine("PRINTMINUTE " + printMinute);
				file.WriteLine("AUTOSTART " + autoStart);
			}
		}
		catch
		{
			// return false if there is any exception
			MessageBox.Show("The configuration cannot be saved\nPlease download the program again or check if the status of the files are writable");
			return false;
		}

		// return true if nothing goes wrong
		return true;
	}

	public void ApplyAutoStartSetting(bool autoStartChecked)
	{
		if (autoStartChecked)
		{
			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				Assembly curAssembly = Assembly.GetExecutingAssembly();
				key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
			}
			catch { }

		}
		else
		{
			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				Assembly curAssembly = Assembly.GetExecutingAssembly();
				key.DeleteValue(curAssembly.GetName().Name);
			}
			catch { }

		}
	}

	public bool IsTimeToPrint()
	{
		LoadNextTime();
		//MessageBox.Show(loadedNextPrintingTime.ToString());
		int timeDifference = DateTime.Compare(loadedNextPrintingTime, DateTime.Now);
		//MessageBox.Show(timeDifference.ToString());
		if (timeDifference <= 0)
		{
			// return true if current time is larger then the print time
			return true;
		}
		else
		{
			// retue false if it is not time to print
			return false;
		}
	}

	public bool OpenReminderWindow()
	{
		return StartProcess(reminderWindowExecutable);
	}

	public bool StartProcess(string targetExecutable)
	{
		try
		{

			System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + targetExecutable);
		}
		catch
		{
			MessageBox.Show("The program cannot be started\nPlease download the program again or move the program folder to another directory");
			return false;
		}
		return true;
	}
}