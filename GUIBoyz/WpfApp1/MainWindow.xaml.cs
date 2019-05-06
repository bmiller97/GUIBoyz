using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Diagnostics;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string EXECUTABLENAME = "BioCProject.exe";
        public int rows = -1;
        public int cols = -1;

        Microsoft.Win32.OpenFileDialog openFileDlg;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            /*string path = @"c:\";
            System.Diagnostics.Process.Start(path);*/
            // Create OpenFileDialog
           openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            Console.WriteLine(openFileDlg.FileName);
        }

        /// <summary>
        /// Loads in custom bin image by specifying file name, height, width, and value range.
        /// </summary>
        /// <param name="maxValue">Refers to the maximum range for pixel values. Anything higher than maxValue will be truncated.</param>
        /// <returns></returns>
        public byte[] loadBinImage(string file, int height, int width, int maxValue = 255)
        {
            float[] data = new float[height * width];

            using (var br = new BinaryReader(File.OpenRead(file)))
            {
                for (int i = 0; i < height * width; i++)
                    data[i] = br.ReadSingle();
            }
            //Convert from float array to byte array
            byte[] outData = new byte[height * width];
            for (int i = 0; i < outData.Length; i++)
            {
                //255 is the max value for byte sized data, values over 255 will be truncated and look wrong
                //this is why maxValue needs to be the largest value in the bin file, so byte values wont be truncated.
                outData[i] = (byte)(data[i] * (255.0f / maxValue));
            }
            return outData;

        }

        public BitmapSource ImgFromBin(string file, int height, int width, int maxValue = 255)
        {
            byte[] byteArrayIn = loadBinImage(file, height, width, maxValue);

            BitmapSource bitmapSource = BitmapSource.Create(304, 304, 1, 1, PixelFormats.Indexed8, BitmapPalettes.Gray256, byteArrayIn, 304);
            return bitmapSource;
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {

            //BitmapSource bitmapSource = BitmapSource.Create(304, 304, 1,1, PixelFormats.Indexed8, BitmapPalettes.Gray256, byteArrayIn, 304);

            imageI.Source = ImgFromBin(openFileDlg.FileName, rows, cols);
        }
        private void runImageParser() {
            Process.Start(EXECUTABLENAME);
        }

        private void InputColumns_KeyUp(object sender, KeyEventArgs e) {
            int parsed = -1;
            if(int.TryParse(inputColumns.Text,out parsed)) {
                //success
                cols = parsed;
                inputColumns.Text = "" + cols;
            } else { //failed
                inputColumns.Text = "";
            }


        }
        private void InputRows_KeyUp(object sender, KeyEventArgs e) {
            int parsed = -1;
            if (int.TryParse(inputRows.Text, out parsed)) {
                //success
                cols = parsed;
                inputRows.Text = ""+  rows;
            } else { //failed
                inputRows.Text = "";
            }


        }


        //works xd
    }
}
