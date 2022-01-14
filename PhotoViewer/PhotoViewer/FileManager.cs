using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoViewer
{
    public class FileManager
    {
        public List<Photo> LoadImgsInDir(string dirPath)
        {
            List<Photo> photos = new List<Photo>();

            foreach (string path in Directory.GetFiles(dirPath))
            {
                if (File.Exists(path) && Path.GetExtension(path) == ".JPG")
                {
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(path);
                    src.DecodePixelWidth = 200;
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    Photo photo = new Photo(Path.GetFileName(path), src, path);
                    photos.Add(photo);
                }
            }
            return photos;
        }
    }
}
