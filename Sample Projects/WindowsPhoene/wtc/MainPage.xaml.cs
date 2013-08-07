using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace wtc
{
    public partial class MainPage : PhoneApplicationPage
    {
        CameraCaptureTask cameraCaptureTask;
        NameThatColor.NTC ntc;
        // Constructor
        public MainPage()
        {
            ntc = new NameThatColor.NTC();
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += cameraCaptureTask_Completed;
            InitializeComponent();
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage img = new BitmapImage();
                img.SetSource(e.ChosenPhoto);
                chosenImage.Source = img;
                chosenImage.Height = mainCanvas.ActualHeight;
                chosenImage.Width = mainCanvas.ActualWidth;
            }
        }

        private void btn_photoChooser_Click(object sender, RoutedEventArgs e)
        {
            cameraCaptureTask.Show();
        }
        private int getPixel(int x, int y, int width, int 
            [] array)
        {
            int index = y * width + x;
            return array[index];
        }
        private void drawingField_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(mainCanvas as UIElement);
            Canvas.SetTop(cursorEllipse, pos.Y - (cursorEllipse.Height / 2));
            Canvas.SetLeft(cursorEllipse, pos.X - (cursorEllipse.Width / 2));
            WriteableBitmap temp = new WriteableBitmap(chosenImage, null);
            int index = (int)pos.Y * temp.PixelWidth + ((int)pos.X);

            int pixel = temp.Pixels[index];

            List<int> pixels = new List<int>();

            for (int i = -5; i < 5; i++)
            {
                for (int y = -5; y < 5; y++)
                {
                    pixels.Add(getPixel((int)pos.X+i, (int)pos.Y+y, temp.PixelWidth, temp.Pixels));
                }
            }

            double av_B = pixels.Average(x => x & 0xFF);
            double av_G = pixels.Average(x => { x >>= 8; return x & 0xFF; });
            double av_R = pixels.Average(x => { x >>=16; return x & 0xFF; });
            
            

            //int B = (int)(pixel & 0xFF); pixel >>= 8;
            //int G = (int)(pixel & 0xFF); pixel >>= 8;
            //int R = (int)(pixel & 0xFF); pixel >>= 8;
            //int A = (int)(pixel);
            //Color col = new Color { A = (byte)A, B = (byte)B, G = (byte)G, R = (byte)R };
            try
            {
                MessageBox.Show("The main color is: " + ntc.getMainColorName((byte)av_R,(byte) av_G, (byte)av_B)
                    + '\n' + "The exact shade is : " + ntc.getColorName((byte)av_R, (byte)av_G, (byte)av_B));
            }
            catch
            {

            }
        }

        private void drawingField_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point pos = e.GetPosition(mainCanvas as UIElement);
            Canvas.SetTop(cursorEllipse, pos.Y - (cursorEllipse.Height / 2));
            Canvas.SetLeft(cursorEllipse, pos.X - (cursorEllipse.Width / 2));
        }

    }
}