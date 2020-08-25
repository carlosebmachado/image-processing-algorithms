﻿using System.Drawing;

namespace IPA.Core
{
    public class ImageEffects
    {
        public static Bitmap SimpleGrayScale(Bitmap image)
        {
            // percorre pixel a pixel bi-dimensionalmente
            Bitmap applyed = (Bitmap)image.Clone();
            for (int x = 0; x < applyed.Width; x++)
            {
                for (int y = 0; y < applyed.Height; y++)
                {
                    // pega o pixel e aplica o efeito
                    var p = applyed.GetPixel(x, y);
                    var l = (p.R + p.G + p.B) / 3;
                    applyed.SetPixel(x, y, Color.FromArgb(p.A, l, l, l));
                }
            }
            return applyed;
        }

        public static Bitmap WeightedGrayScale(Bitmap image)
        {
            Bitmap applyed = (Bitmap)image.Clone();
            for (int x = 0; x < applyed.Width; x++)
            {
                for (int y = 0; y < applyed.Height; y++)
                {
                    var p = applyed.GetPixel(x, y);
                    var l = p.R * 0.299 + p.G * 0.587 + p.B * 0.114;
                    applyed.SetPixel(x, y, Color.FromArgb(p.A, (int)l, (int)l, (int)l));
                }
            }
            return applyed;
        }

        public static Bitmap Negative(Bitmap image)
        {
            Bitmap applyed = (Bitmap)image.Clone();
            for (int x = 0; x < applyed.Width; x++)
            {
                for (int y = 0; y < applyed.Height; y++)
                {
                    var p = applyed.GetPixel(x, y);
                    applyed.SetPixel(x, y, Color.FromArgb(p.A, 255 - p.R, 255 - p.G, 255 - p.B));
                }
            }
            return applyed;
        }

        public static Bitmap Threshold(Bitmap image, int L)
        {
            Bitmap applyed = (Bitmap)image.Clone();
            for (int x = 0; x < applyed.Width; x++)
            {
                for (int y = 0; y < applyed.Height; y++)
                {
                    // atraves do limiar definido, define a cor do pixel
                    var p = applyed.GetPixel(x, y);
                    if (p.R > L)
                    {
                        applyed.SetPixel(x, y, Color.FromArgb(p.A, 255, 255, 255));
                    }
                    else
                    {
                        applyed.SetPixel(x, y, Color.FromArgb(p.A, 0, 0, 0));
                    }
                }
            }
            return applyed;
        }

        public static Bitmap Adiction(Bitmap image1, Bitmap image2, float p)
        {
            Bitmap cImage1 = (Bitmap)image1.Clone();
            Bitmap cImage2 = (Bitmap)image2.Clone();
            int w, h;
            Bitmap applyed;
            // verifica a menor imagem para ser a base
            if (cImage1.Width < cImage2.Width)
            {
                w = cImage1.Width;
                h = cImage1.Height;
                applyed = (Bitmap)image1.Clone();
            }
            else
            {
                w = cImage2.Width;
                h = cImage2.Height;
                applyed = (Bitmap)image2.Clone();
            }

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    // é feita a soma das imagens atraves do peso definido para cada img
                    var l1 = cImage1.GetPixel(x, y);
                    var l2 = cImage2.GetPixel(x, y);
                    applyed.SetPixel(x, y, Color.FromArgb(l1.A, 
                        (int)((l1.R * (1F - p)) + (l2.R * p)),
                        (int)((l1.G * (1F - p)) + (l2.G * p)),
                        (int)((l1.B * (1F - p)) + (l2.B * p))));
                }
            }

            return applyed;
        }

        public static Bitmap Subtraction(Bitmap image1, Bitmap image2, bool invert)
        {
            Bitmap cImage1 = (Bitmap)image1.Clone();
            Bitmap cImage2 = (Bitmap)image2.Clone();
            int w, h;
            Bitmap applyed;

            if (cImage1.Width < cImage2.Width)
            {
                w = cImage1.Width;
                h = cImage1.Height;
                applyed = (Bitmap)image1.Clone();
            }
            else
            {
                w = cImage2.Width;
                h = cImage2.Height;
                applyed = (Bitmap)image2.Clone();
            }

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var l1 = cImage1.GetPixel(x, y);
                    var l2 = cImage2.GetPixel(x, y);
                    // possivel inversao de subtração
                    if (invert)
                    {
                        applyed.SetPixel(x, y, Color.FromArgb(l1.A,
                        Clamp(l2.R - l1.R, 0, 255),
                        Clamp(l2.G - l1.G, 0, 255),
                        Clamp(l2.B - l1.B, 0, 255)));
                    }
                    else
                    {
                        applyed.SetPixel(x, y, Color.FromArgb(l1.A,
                        Clamp(l1.R - l2.R, 0, 255),
                        Clamp(l1.G - l2.G, 0, 255),
                        Clamp(l1.B - l2.B, 0, 255)));
                    }
                    
                }
            }

            return applyed;
        }

        public static int Clamp(int val, int min, int max)
        {
            // função básica de clamp
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }
    }
}