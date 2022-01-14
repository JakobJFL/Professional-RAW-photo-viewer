using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PhotoViewer
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
        private int _currentImage = 0;

        public List<Photo> Photos { get; set; }
        public Process myProcess { get; set; }

        public void LoadImages(string filePath)
        {
            FileManager fileManager = new FileManager();
            List<Photo> photos = fileManager.LoadImgsInDir(filePath);
            Photos = photos;
            foreach (var item in photos)
            {
                Console.WriteLine(item.Name);
            }
            MainViewer.Source = photos.First().Image;
        }

        private void loadImgs_Click(object sender, RoutedEventArgs e)
        {
            LoadImages(@"E:\Lagring\(E) Billeder\Shot on Canon\Hjemme ved forlæder\efter jul");
        }

        private void prevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentImage > 0)
                MainViewer.Source = Photos[--_currentImage].Image;
            Check();
        }

        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentImage < Photos.Count-1)
                MainViewer.Source = Photos[++_currentImage].Image;
            Check();
            //await LoadHigeImg();
        }

        private void Check()
        {
            if (_currentImage < Photos.Count - 1)
                nextBtn.IsEnabled = true;
            else
                nextBtn.IsEnabled = false;
            if (_currentImage > 0)
                prevBtn.IsEnabled = true;
            else
                prevBtn.IsEnabled = false;
        }


        private async Task LoadHigeImg()
        {
            using (myProcess = new Process())
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(Photos[_currentImage].Path);
                image.EndInit();
                MainViewer.Source = image;
                myProcess = null;
            }
           
        }
    }
}
