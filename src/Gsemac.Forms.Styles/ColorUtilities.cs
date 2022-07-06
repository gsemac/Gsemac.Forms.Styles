using Gsemac.Core.Extensions;
using Gsemac.Drawing;
using System;
using System.Drawing;

namespace Gsemac.Forms.Styles {

    public static class ColorUtilities {

        // Public members

        public static double ComputeDistance(Color first, Color second) {

            return ComputeDistance(first, second, ColorDistanceStrategy.DeltaE);

        }
        public static double ComputeDistance(Color first, Color second, IColorDistanceStrategy strategy) {

            if (strategy is null)
                throw new ArgumentNullException(nameof(strategy));

            return strategy.ComputeDistance(first, second);

        }

        public static double ComputeSimilarity(Color first, Color second) {

            return ComputeSimilarity(first, second, ColorDistanceStrategy.DeltaE);

        }
        public static double ComputeSimilarity(Color first, Color second, IColorDistanceStrategy strategy) {

            if (strategy is null)
                throw new ArgumentNullException(nameof(strategy));

            return strategy.ComputeSimilarity(first, second);

        }

        /// <summary>
        /// Returns a luminance value between 0.0 and 1.0 for the given SRGB color.
        /// </summary>
        /// <param name="color">The SRGB color to compute the luminance value for.</param>
        /// <returns>A luminance value between 0.0 and 1.0.</returns>
        public static double ComputeRelativeLuminance(Color color) {

            // https://stackoverflow.com/a/56678483/5383169 (Myndex)

            // Map all RGB components to the range [0.0, 1.0].

            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            // Convert the gamma-encoded RGB to a linear value.

            r = SrgbToLinear(r);
            g = SrgbToLinear(g);
            b = SrgbToLinear(b);

            // Apply the standard coeffients to calculate luminance.

            double y = 0.2126 * r + 0.7152 * g + 0.0722 * b;

            return y;

        }
        /// <summary>
        /// Returns a lightness value (L*) between 0.0 (black) and 1.0 (white) for the given SRGB color.
        /// </summary>
        /// <param name="color">The SRGB color to compute the lightness value for.</param>
        /// <returns>A lightness value between 0.0 and 1.0.</returns>
        public static double ComputePerceivedLightness(Color color) {

            // https://stackoverflow.com/a/56678483/5383169 (Myndex)

            double luminance = ComputeRelativeLuminance(color);
            double perceivedLightness = LuminanceToLStar(luminance);

            return perceivedLightness / 100.0;

        }

        public static Color Tint(this Color color, float factor) {

            // https://stackoverflow.com/a/31325812/5383169 (JBE)

            int r = color.R;
            int g = color.G;
            int b = color.B;

            int newR = (int)(r + (255 - r) * factor);
            int newG = (int)(g + (255 - g) * factor);
            int newB = (int)(b + (255 - b) * factor);

            return Color.FromArgb(newR, newG, newB);

        }
        public static Color Shade(this Color color, float factor) {

            // https://stackoverflow.com/a/31325812/5383169 (JBE)

            factor = 1.0f - factor;

            return Color.FromArgb((int)(color.R * factor), (int)(color.G * factor), (int)(color.B * factor));

        }

        public static Color ToGreyscale(Color color) {

            int greyColor = (int)(0.11 * color.R + 0.59 * color.G + 0.30 * color.B);

            return Color.FromArgb(greyColor, greyColor, greyColor);

        }

        public static LabColor RgbToLab(int r, int g, int b) {

            return XyzToLab(RgbToXyz(r, g, b));

        }
        public static LabColor RgbToLab(Color color) {

            return RgbToLab(color.R, color.G, color.B);

        }
        public static XyzColor RgbToXyz(int r, int g, int b) {

            // Convert from RGB to XYZ.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double tempR = r / 255.0;
            double tempG = g / 255.0;
            double tempB = b / 255.0;

            tempR = tempR > 0.04045f ? Math.Pow((tempR + 0.055) / 1.055, 2.4) : tempR / 12.92;
            tempG = tempG > 0.04045f ? Math.Pow((tempG + 0.055) / 1.055, 2.4) : tempG / 12.92;
            tempB = tempB > 0.04045f ? Math.Pow((tempB + 0.055) / 1.055, 2.4) : tempB / 12.92;

            tempR *= 100.0;
            tempG *= 100.0;
            tempB *= 100.0;

            double x = tempR * 0.4124 + tempG * 0.3576 + tempB * 0.1805;
            double y = tempR * 0.2126 + tempG * 0.7152 + tempB * 0.0722;
            double z = tempR * 0.0193 + tempG * 0.1192 + tempB * 0.9505;

            return new XyzColor(x, y, z);

        }
        public static XyzColor RgbToXyz(Color color) {

            return RgbToXyz(color.R, color.G, color.B);

        }
        public static Color LabToRgb(double l, double a, double b) {

            return XyzToRgb(LabToXyz(l, a, b));

        }
        public static Color LabToRgb(LabColor color) {

            return LabToRgb(color.L, color.A, color.B);

        }
        public static Color XyzToRgb(double x, double y, double z) {

            // Convert from XYZ to RGB.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double tempX = x / 100.0;
            double tempY = y / 100.0;
            double tempZ = z / 100.0;

            double r = tempX * 3.2406 + tempY * -1.5372 + tempZ * -0.4986;
            double g = tempX * -0.9689 + tempY * 1.8758 + tempZ * 0.0415;
            double b = tempX * 0.0557 + tempY * -0.2040 + tempZ * 1.0570;

            r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
            g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
            b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b;

            r *= 255.0;
            g *= 255.0;
            b *= 255.0;

            return Color.FromArgb((byte)r, (byte)g, (byte)b);

        }
        public static Color XyzToRgb(XyzColor color) {

            return XyzToRgb(color.X, color.Y, color.Z);

        }

        // Private members

        private const double LabReferenceX = 95.047;
        private const double LabReferenceY = 100.0;
        private const double LabReferenceZ = 108.883;

        private static double SrgbToLinear(double srgbChannel) {

            if (srgbChannel <= 0.04045)
                return srgbChannel / 12.92;

            return Math.Pow((srgbChannel + 0.055) / 1.055, 2.4);

        }
        private static double LuminanceToLStar(double luminance) {

            // Send this function a luminance value between 0.0 and 1.0,
            // and it returns L* which is "perceptual lightness"

            if (luminance <= 216.0 / 24389.0)
                return luminance * (24389.0 / 27.0);

            return Math.Pow(luminance, 1.0 / 3.0) * 116.0 - 16.0;

        }

        private static XyzColor LabToXyz(double l, double a, double b) {

            // Convert from LAB to XYZ.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double y = (l + 16) / 116.0;
            double x = a / 500.0 + y;
            double z = y - b / 200.0f;

            x = Math.Pow(x, 3) > 0.008856 ? Math.Pow(x, 3) : (x - 16 / 116.0) / 7.787;
            y = Math.Pow(y, 3) > 0.008856 ? Math.Pow(y, 3) : (y - 16 / 116.0) / 7.787;
            z = Math.Pow(z, 3) > 0.008856 ? Math.Pow(z, 3) : (z - 16 / 116.0) / 7.787;

            x *= 0.95047;
            y *= 1.00000;
            z *= 1.08883;

            return new XyzColor(x, y, z);

        }
        private static LabColor XyzToLab(double x, double y, double z) {

            // Convert from XYZ to LAB.
            // Algorithm from https://www.easyrgb.com/en/math.php
            // Using D65 observer values

            x /= LabReferenceX;
            y /= LabReferenceY;
            z /= LabReferenceZ;

            x = x > 0.008856 ? Math.Pow(x, 1 / 3.0) : 7.787 * x + 16 / 116.0;
            y = y > 0.008856 ? Math.Pow(y, 1 / 3.0) : 7.787 * y + 16 / 116.0;
            z = z > 0.008856 ? Math.Pow(z, 1 / 3.0) : 7.787 * z + 16 / 116.0;

            double l = 116 * y - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);

            return new LabColor(l, a, b);

        }
        private static LabColor XyzToLab(XyzColor color) {

            return XyzToLab(color.X, color.Y, color.Z);

        }

    }

}