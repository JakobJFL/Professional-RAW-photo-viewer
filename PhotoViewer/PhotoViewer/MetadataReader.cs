using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace PhotoViewer
{
    public static class MetadataReader
    {
        //http://www.gnazzo.net/Content/blogs/Getting%20Metadata%20from%20Digital%20Photographs%20using%20CSharp.pdf
        public static string getExposure(Bitmap bitmap)
        {
            try
            {
                var propertyValue = bitmap.GetPropertyItem(33434).Value;
                double a = BitConverter.ToUInt16(propertyValue, 0);
                double b = BitConverter.ToUInt16(propertyValue, 4);
                return a + "/" + b;
            }
            catch (ArgumentException)
            {
                return "n/a";
            }
        }
        public static string getIso(Bitmap bitmap)
        {
            try
            {
                var propertyValue = bitmap.GetPropertyItem(34855).Value;
                return BitConverter.ToUInt16(propertyValue, 0).ToString();
            }
            catch (ArgumentException)
            {
                return "n/a";
            }
        }
        public static string getFStop(Bitmap bitmap)
        {
            try
            {
                var propertyValue = bitmap.GetPropertyItem(33437).Value;
                double a = BitConverter.ToUInt16(propertyValue, 0);
                double b = BitConverter.ToUInt16(propertyValue, 4);

                return "f/" + a / b;
            }
            catch (ArgumentException)
            {
                return "n/a";
            }
        }
        public static string getFocalLength(Bitmap bitmap)
        {
            try
            {
                var propertyValue = bitmap.GetPropertyItem(37386).Value;
                double a = BitConverter.ToUInt16(propertyValue, 0);
                double b = BitConverter.ToUInt16(propertyValue, 4);
                return a / b + " mm";
            }
            catch (ArgumentException)
            {
                return "n/a";
            }
        }
        public static string getExposureBias(Bitmap bitmap)
        {
            try
            {
                var propertyValue = bitmap.GetPropertyItem(37380).Value;
                double a = BitConverter.ToInt16(propertyValue, 0);
                double b = BitConverter.ToInt16(propertyValue, 4);
                if (b == 1)
                    return a.ToString();
                else if (a >= 0)
                    return (a / b) + "," + b;
                else
                    return "-" + (a / b) + "," + b;
            }
            catch (ArgumentException)
            {
                return "n/a";
            }
        }
        public static Rotation getOrientation(Bitmap bitmap)
        {
            if (!bitmap.PropertyIdList.Contains(274))
                return Rotation.Rotate0;

            var propertyValue = bitmap.GetPropertyItem(274).Value;
            int val = BitConverter.ToUInt16(propertyValue, 0);
            var rot = Rotation.Rotate0;

            if (val == 3 || val == 4)
                rot = Rotation.Rotate180;
            else if (val == 5 || val == 6)
                rot = Rotation.Rotate90;
            else if (val == 7 || val == 8)
                rot = Rotation.Rotate270;
            return rot;
        }
    }
}
