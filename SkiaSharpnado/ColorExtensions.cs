
namespace SkiaSharpnado
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color, bool withAlpha = false)
        {
            const string alphaFormat = "{0:X2}";
            const string hexFormat = "{0}{1:X2}{2:X2}{3:X2}";

            var red = (int)(color.Red * 255);
            var green = (int)(color.Green * 255);
            var blue = (int)(color.Blue * 255);

            string alphaString = string.Empty;
            if (withAlpha)
            {
                var alpha = (int)(color.Alpha * 255);
                alphaString = string.Format(alphaFormat, alpha);
            }

            return string.Format(hexFormat, alphaString, red, green, blue);
        }
    }
}
