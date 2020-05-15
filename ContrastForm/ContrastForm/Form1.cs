using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ContrastForm
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public static Bitmap image;
        public static Bitmap imageOriginal;
        public static string full_name_of_image = "\0";
        public static UInt32[,] pixel;
        int contrastProsent = default;
        

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    full_name_of_image = open_dialog.FileName;
                    image = new Bitmap(open_dialog.FileName);
                    imageOriginal = new Bitmap(open_dialog.FileName);


                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = imageOriginal;

                    pictureBox1.Invalidate();
                    //получение матрицы с пикселями
                    pixel = new UInt32[image.Height, image.Width];
                    for (int y = 0; y < image.Height; y++)
                        for (int x = 0; x < image.Width; x++)
                            pixel[y, x] = (UInt32)(image.GetPixel(x, y).ToArgb());
                }
                catch
                {
                    full_name_of_image = "\0";
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //trackBar2_Scroll();
        }
        private void сохранитьКакToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox2.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

       /* public static void FromOnePixelToBitmap(int x, int y, UInt32 pixel)
        {
            image.SetPixel(y, x, Color.FromArgb((int)pixel));
        }*/

        //вывод на экран
        public void FromBitmapToScreen(Bitmap bitmap)
        {
           pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
           pictureBox2.Image = bitmap;
        }

        /*private void trackBar2_Scroll()
        {

            if (full_name_of_image != "\0")
            {
                button1.Enabled = false;
                contrastProsent = int.Parse(textBox1.Text);
                UInt32 p;
                for (int i = 0; i < Form1.image.Height; i++)
                    for (int j = 0; j < Form1.image.Width; j++)
                    {
                        p = Contrast.ContrastMethod(Form1.pixel[i, j], contrastProsent);
                        FromOnePixelToBitmap(i, j, p);
                        pixel[i, j] = (UInt32)(Form1.image.GetPixel(j, i).ToArgb());
                    }

               // FromBitmapToScreen();
                button1.Enabled = true;
            }
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            contrastProsent = int.Parse(textBox1.Text);
            //trackBar2_Scroll();
            //Bitmap Test1 = Negative.ProcessImage(new Filter(image));
            Bitmap Test1 = Contrast.ProcessImage(new Filter(image), contrastProsent);
            FromBitmapToScreen(Test1);
            button1.Enabled = true; 
        }


        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            MedianFilter1.MedianFiltering(image);
            FromBitmapToScreen(image);
            button2.Enabled = true;
        }
    }
}
