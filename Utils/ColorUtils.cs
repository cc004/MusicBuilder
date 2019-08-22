using Microsoft.Xna.Framework;

namespace MusicBuilder.Utils
{
    public static class ColorUtils
    {
        public static Color ColorBlend(Color start, Color end, double progress)
        {
            //progress = progress * progress * progress * progress;
            return new Color((int) (progress * end.R + (1.0 - progress) * start.R),
                             (int) (progress * end.G + (1.0 - progress) * start.G),
                             (int) (progress * end.B + (1.0 - progress) * start.B));
        }

        public static Color ColorHue(double hue)
        {
            hue = hue - (int)hue;
            int p = (int)(hue * 6);
            int q = (int) (256 * (hue * 3- p / 2.0));
            switch (p)
            {
                case 0: return new Color(255 - q, 128 + q, 0);
                case 1: return new Color(128 - q, 255, q);
                case 2: return new Color(0, 255 - q, 128 + q);
                case 3: return new Color(q, 128 - q, 255);
                case 4: return new Color(128 + q, 0, 255 - q);
                case 5: return new Color(255, q, 128 - q);
            }
            return new Color(0, 0, 0);
        }
    }
}