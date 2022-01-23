﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        private string _filePath = @"E:\Lagring\(E) Billeder\Shot on Canon\12-01-2020 gåtur med nico";
        public List<Photo> Photos { get; set; } = new List<Photo>();
        Task<Photo> HighPhoto { get; set; }
        CancellationTokenSource cts = new CancellationTokenSource();

        private void loadImgs_Click(object sender, RoutedEventArgs e)
        {
            LoadImages(_filePath);
        }

        public void LoadImages(string dirPath)
        {
            Photos = FileManager.LoadLowPhotosInDir(dirPath);
            MainViewer.Source = Photos.First().Image;
        }

        private async void prevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentImage > 0)
                await ShowPhoto(--_currentImage);
            UpdateButtons();
        }

        private async void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentImage < Photos.Count - 1)
                await ShowPhoto(++_currentImage);
            UpdateButtons();
        }

        private async Task ShowPhoto(int imageToShow)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (HighPhoto != null)
                cts.Cancel();
            cts = new CancellationTokenSource();
            this.Title = "PhotoViewer - " + Photos[imageToShow].Name;
            MainViewer.Source = Photos[imageToShow].Image;
            setExifData(Photos[imageToShow]);

            HighPhoto = FileManager.GetHighPhotoAsync(Photos[imageToShow].Path, cts.Token);
            Photo highPhoto = await HighPhoto;
            if (highPhoto != null && Photos[imageToShow].Name == highPhoto.Name)
            {
                MainViewer.Source = highPhoto.Image;
                setExifData(highPhoto);
            }

        }

        private void setExifData(Photo photo)
        {
            exifData1.Text = "Focal Length: " + photo.FocalLength + "\nExposure Bias: " + photo.ExposureBias + "\nName: " + photo.Name;
            exifData2.Text = "Exposure: " + photo.Exposure + "\nF-Stop: " + photo.FStop + "\nISO: " + photo.Iso;
        }

        private void UpdateButtons()
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Key) 
            {
                case Key.Left:
                    prevBtn_Click(null, null);
                    break;
                case Key.Right:
                    nextBtn_Click(null, null);
                    break;
                default:
                    break;
            }
        }
    }
}
