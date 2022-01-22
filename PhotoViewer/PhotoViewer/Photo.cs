using System.Windows.Media;

namespace PhotoViewer
{
    public class Photo
    {
        public Photo(string name, ImageSource image, string path)
        {
            Name = name;
            Image = image;
            Path = path;
        }
        public string Name { get; }
        public ImageSource Image { get; }
        public string Path { get; }

    }
}
