using System.Windows.Media;

namespace PhotoViewer
{
    public class Photo
    {
        public enum OrType
        {
            Landscape,
            portrait
        }
        public Photo(string name, ImageSource image, string path)
        {
            Name = name;
            Image = image;
            Path = path;
        }
        public string Name { get; }
        public ImageSource Image { get; }
        public string Path { get; }
        public string FStop { get; set; }
        public string Exposure { get; set; }
        public string Iso { get; set; }
        public string FocalLength { get; set; }
        public string ExposureBias { get; set; }
        public string Date { get; set; }
        public OrType Orientation { get; set; }

    }
}
