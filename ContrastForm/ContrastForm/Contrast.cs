using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ContrastForm
{
    class Contrast
    {
        public static UInt32 ContrastMethod(UInt32 point, int N)
        {
            int R;
            int G;
            int B;

            

            if (N >= 0)
            {
                if (N == 100) N = 99;
                R = (int)((((point & 0x00FF0000) >> 16) * 100 - 128 * N) / (100 - N));
                G = (int)((((point & 0x0000FF00) >> 8) * 100 - 128 * N) / (100 - N));
                B = (int)(((point & 0x000000FF) * 100 - 128 * N) / (100 - N));
            }
            else
            {
                R = (int)((((point & 0x00FF0000) >> 16) * (100 - (-N)) + 128 * (-N)) / 100);
                G = (int)((((point & 0x0000FF00) >> 8) * (100 - (-N)) + 128 * (-N)) / 100);
                B = (int)(((point & 0x000000FF) * (100 - (-N)) + 128 * (-N)) / 100);
            }

            //контролируем переполнение переменных
            if (R < 0) R = 0;
            if (R > 255) R = 255;
            if (G < 0) G = 0;
            if (G > 255) G = 255;
            if (B < 0) B = 0;
            if (B > 255) B = 255;

            point = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);

            return point;
        }


        public static unsafe Bitmap ProcessImage(Filter Main, int Value)
        {
            int RedVal, GreenVal, BlueVal; 
            double Pixel; 
            double Contrast = (100.0 + Value) / 100.0; //Вычисляем общее значение контраста 
            Contrast = Contrast * Contrast; 
            for (int I = 0; I < Main.AllPixelsBytes; I += Main.BytesPerPixel) 
            { 
                BlueVal = *(Main.Unsafe_IMG_Scan0 + (I + 0)); //B 
                GreenVal = *(Main.Unsafe_IMG_Scan0 + (I + 1)); //G 
                RedVal = *(Main.Unsafe_IMG_Scan0 + (I + 2)); //R 
                Pixel = RedVal / 255.0; 
                Pixel = Pixel - 0.5; 
                Pixel = Pixel * Contrast; 
                Pixel = Pixel + 0.5; 
                Pixel = Pixel * 255; 
                if (Pixel < 0) 
                    Pixel = 0; 
                if (Pixel > 255) 
                    Pixel = 255; 
                *(Main.Unsafe_IMG_Scan0 + (I + 2)) = Convert.ToByte(Pixel); 
                Pixel = GreenVal / 255.0; 
                Pixel = Pixel - 0.5; 
                Pixel = Pixel * Contrast; 
                Pixel = Pixel + 0.5; 
                Pixel = Pixel * 255; 
                if (Pixel < 0) 
                    Pixel = 0; 
                if (Pixel > 255) 
                    Pixel = 255; 
                *(Main.Unsafe_IMG_Scan0 + (I + 1)) = Convert.ToByte(Pixel); 
                Pixel = BlueVal / 255.0; 
                Pixel = Pixel - 0.5; 
                Pixel = Pixel * Contrast; 
                Pixel = Pixel + 0.5; 
                Pixel = Pixel * 255; 
                if (Pixel < 0) 
                    Pixel = 0; 
                if (Pixel > 255) 
                    Pixel = 255; 
                *(Main.Unsafe_IMG_Scan0 + (I + 0)) = Convert.ToByte(Pixel); 
            } 
            Main.UnLock(); 
            return Main.Picture; 
        }
    }


}
