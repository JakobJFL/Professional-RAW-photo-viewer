using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PhotoViewer
{
    public static class FileManager
    {
        public static List<Photo> LoadLowPhotosInDir(string dirPath)
        {
            List<Photo> photos = new List<Photo>();

            foreach (string imgPath in Directory.GetFiles(dirPath))
            {
                if (File.Exists(imgPath) && Path.GetExtension(imgPath) == ".JPG")
                {
                    photos.Add(GetLowPhoto(imgPath));
                }
            }
            return photos;
        }
        public static Photo GetLowPhoto(string path)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(path);
            src.DecodePixelWidth = 300;
            src.EndInit();
            //src.Freeze();
            return new Photo(Path.GetFileName(path), src, path);
        }
        public static async Task<Photo> GetHighPhotoAsync(string path, CancellationToken cancellationToken)
        {
            return await Task.Run(() => {
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(path);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                if (cancellationToken.IsCancellationRequested)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return null;
                }
                src.Freeze();
                Console.WriteLine(path);
                return new Photo(Path.GetFileName(path), src, path);
            });
        }
        
    }
}
