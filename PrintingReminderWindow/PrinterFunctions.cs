using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// import the library for printing image
using System.Drawing.Printing;
// import the library for check file exist
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace PrintingReminderWindow
{
    class PrinterFunctions
    {
        // define the shared variables
        String imagePath = "";

        // define the constructor of printer function
        public PrinterFunctions()
        {

        }

        // Reference code: https://stackoverflow.com/questions/5750659/print-images-c-net
        // define a function to print image
        public int DefaultPrint()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintImage;
            MessageBox.Show("Please make sure your printer is ready before you click \"OK\"");
            pd.Print();
            MessageBox.Show("The file is sent to the printer\nPlease follow the instruction of the printer to print");
            // return 0 if everthing works well
            return 0;
        }

        // define a function used to set the printed image
        private void PrintImage(object o, PrintPageEventArgs e)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);
            Point loc = new Point(100, 100);
            Rectangle m = e.MarginBounds;

            // Reference: https://stackoverflow.com/questions/9982579/printing-image-with-printdocument-how-to-adjust-the-image-to-fit-paper-size
            // resize the image to make it fit the size of the page
            if ((double)img.Width / (double)img.Height > (double)m.Width / (double)m.Height) // image is wider
            {
                m.Height = (int)((double)img.Height / (double)img.Width * (double)m.Width);
            }
            else
            {
                m.Width = (int)((double)img.Width / (double)img.Height * (double)m.Height);
            }

            e.Graphics.DrawImage(img, m);
        }

        // define a function to set the image file path
        public void SetImagePath(String filePath)
        {
            imagePath = filePath;
        }

        // Reference code: https://stackoverflow.com/questions/17318585/check-if-file-can-be-read
        // define a function to check if the file exist
        public Boolean CheckFileReadable()
        {
            string filePath = imagePath;
            try
            {
                if (filePath.Equals(""))
                {
                    return false;
                }
                File.Open(filePath, FileMode.Open, FileAccess.Read).Dispose();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public int AdvancePrint()
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            // set the print content
            printDocument.PrintPage += PrintImage;
            printDialog.Document = printDocument;
            
            // print the file if the user want to
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Please make sure your printer is ready before you click \"OK\"");
                printDocument.Print();
                MessageBox.Show("The file is sent to the printer\nPlease follow the instruction of the printer to print");
            }
            // return 0 if nothing wrong
            return 0;
        }
    }
}
