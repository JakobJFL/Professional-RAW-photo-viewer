using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
            var r = new Regex(":");
            foreach (string imgPath in Directory.GetFiles(dirPath))
            {
                if (File.Exists(imgPath) && Path.GetExtension(imgPath) == ".JPG" || Path.GetExtension(imgPath) == ".jpg")
                {
                    Photo photo = GetLowPhoto(imgPath);
                    photos.Add(photo);
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
                Bitmap bitmap = new Bitmap(path);
                src.BeginInit();
                src.UriSource = new Uri(path);
                src.CacheOption = BitmapCacheOption.OnLoad;
                Rotation rotation = MetadataReader.getOrientation(bitmap);
                src.Rotation = rotation;
                src.EndInit();
                if (cancellationToken.IsCancellationRequested)
                    return null;
                src.Freeze();
                Photo photo = new Photo(Path.GetFileName(path), src, path);
                photo.ExposureBias = MetadataReader.getExposureBias(bitmap);
                photo.Exposure = MetadataReader.getExposure(bitmap);
                photo.FocalLength = MetadataReader.getFocalLength(bitmap);
                photo.FStop = MetadataReader.getFStop(bitmap);
                photo.Iso = MetadataReader.getIso(bitmap);
                return photo;
            });
        }
    }
}
