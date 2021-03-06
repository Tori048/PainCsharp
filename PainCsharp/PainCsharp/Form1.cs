﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PainCsharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Image Histogramma(Image PictureOne)
        {
            Bitmap barChart = null;
            Bitmap PictureOne1 = new Bitmap(PictureOne);
            barChart = new Bitmap(PictureOne.Width, PictureOne.Height);
            int[] R = new int[256];
            int[] G = new int[256];
            int[] B = new int[256];
            int i, j;
            Color color;
            for (i = 0; i < PictureOne.Width; ++i)
                for (j = 0; j < PictureOne.Height; ++j)
                {
                    color = PictureOne1.GetPixel(i, j);
                    ++R[color.R];
                    ++G[color.G];
                    ++B[color.B];
                }
            int max = 0;
            for (i = 0; i < 256; ++i)
            {
                if (R[i] > max)
                    max = R[i];
                if (G[i] > max)
                    max = G[i];
                if (B[i] > max)
                    max = B[i];
            }
            double point = (double)max / PictureOne.Height;
            for (i = 0; i < PictureOne.Width - 3; ++i)
            {
                for (j = PictureOne.Height - 1;
                    j > PictureOne.Height - R[i / 4] / point;
                    --j)
                {
                    barChart.SetPixel(i, j, Color.Red);
                }
                ++i;
                for (j = PictureOne.Height - 1; j > PictureOne.Height - G[i / 4] / point; --j)
                {
                    barChart.SetPixel(i, j, Color.Green);
                }
                ++i;
                for (j = PictureOne.Height - 1; j > PictureOne.Height - B[i / 4] / point; --j)
                {
                    barChart.SetPixel(i, j, Color.Blue);
                }
            }
            return barChart;
        }

        public void FromImageToTxt(Image image)
        {
            progressConvertToTxt.Visible = true;
            progressBarConvertToTxt.Visible = true;
            //Объявляем переменную для формирования строки первой матрицы в текстовом представлении (значения по оси Х картинки)
            string FileLine1 = string.Empty;
            //Объявляем список строк, в который будем построчно добавлять матрицу в текстовом виде
            List<string> file1 = new List<string>();

            //Объявляем переменную для формирования строки матрицы, в которую будем писать значения цветов в формате ARGB
            string FileLine2 = string.Empty;
            //Список строк для формирования второго файла
            List<string> file2 = new List<string>();

            Bitmap b1 = new Bitmap(image);

            //Объявляем переменные для значений высоты и ширины матрицы (картинки)...
            //...и тут же задаем значения этих переменных взяв их из высоты и ширины картинки в пикселях
            int height = b1.Height; //Это высота картинки, и наша матрица по вертикали будет состоять из точно такого же числа элементов.
            int width = b1.Width; //Это ширина картинки, т.е. число элементов матрицы по горизонтали
            progressBarConvertToTxt.Maximum = height+2;
            //Тут мы объявляем саму матрицу в виде двумерного массива,
            Color[,] colorMatrix = new Color[width, height];

            //Цикл будет выполняться от 0 и до тех пор, пока y меньше height (высоты матрицы и картинки)
            //На каждой итерации увеличиваем значение y на единицу.
            for (int y = 0; y < height; y++)
            {
                //В начале каждой итерации мы обнуляем переменные для формирования строк для файлов
                FileLine1 = string.Empty;
                FileLine2 = string.Empty;
                //А теперь сканируем горизонтальные строки матрицы:
                for (int x = 0; x < width; x++)
                {
                    //В матрицу добавляем цвет точки с координатами x,y из картинки b1.            
                    colorMatrix[x, y] = b1.GetPixel(x, y);
                    //А теперь преобразуем цвет точки (x,y) в:
                    //1. Текстовое представление:
                    FileLine1 += colorMatrix[x, y].ToString() + " ";
                    //2. Значение цвета в целочисленном формате:
                    FileLine2 += colorMatrix[x, y].ToArgb().ToString() + " ";
                    //Тут в обоих случаях в конечном итоге строки преобразуются к типу string для удобства их сохранения в текстовом файле
                }
                //А теперь в списки добавляем каждую из полученных строк:
                //Строка картинки в текстовом виде:
                file1.Add(FileLine1);
                //Строка картинки в виде значения цветов пикселей
                file2.Add(FileLine2);
                progressBarConvertToTxt.Value++;
            }
            //Записываем полученные результаты в текстовые файлы:
            File.WriteAllLines("text.txt", file1);
            progressBarConvertToTxt.Value++;
            File.WriteAllLines("ARGB.txt", file2);
            progressBarConvertToTxt.Value++;
            MessageBox.Show("Изображение сохранено");
            progressBarConvertToTxt.Visible = false;
            progressConvertToTxt.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                sr.Close();
                label1.Text = label1.Text + pictureBox1.Image.Width.ToString() + "x" + pictureBox1.Image.Height.ToString();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
            catch(OutOfMemoryException mem)
            {
                MessageBox.Show("Что-то с памятью моей стало \r\n" + mem.ToString());
            }
            catch(ArgumentException arg)
            {
                MessageBox.Show("Что-то не то пошло на вход \r\n" + arg.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new
                  StreamReader(openFileDialog2.FileName);
                sr.Close();
                label2.Text = label2.Text + pictureBox2.Image.Width.ToString() + "x" + pictureBox2.Image.Height.ToString();
            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                pictureBox2.Image = Image.FromFile(openFileDialog2.FileName);
            }
            catch (OutOfMemoryException mem)
            {
                MessageBox.Show("Что-то с памятью моей стало \r\n" + mem.ToString());
            }
            catch (ArgumentException arg)
            {
                MessageBox.Show("Что-то не то пошло на вход \r\n" + arg.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (pictureBox1.Image != null)
            {
                Bitmap PictureOne = new Bitmap(pictureBox1.Image);
                Bitmap barChart = null;
                barChart = new Bitmap(Histogramma(PictureOne));
                pictureBox3.Image = barChart;
            }
            if (pictureBox2.Image != null)
            {
                Bitmap PictureTwo = new Bitmap(pictureBox2.Image);
                Bitmap barChart2 = null;
                barChart2 = new Bitmap(Histogramma(PictureTwo));
                pictureBox4.Image = barChart2;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(comboBox2.Text) == 1)
                {
                    List<string> file = new List<string>();
                    if (openFileDialog3.ShowDialog() == DialogResult.OK)
                    {
                        file = File.ReadAllLines(openFileDialog3.FileName).ToList();
                        if (file.Count() < 1) return; //Файл пустой!
                        var width = file[0].Split(' ').Count() - 1; //В конце строки у нас есть дополнительный пробел!
                        var heigh = file.Count;

                        Bitmap b2 = new Bitmap(width, heigh);
                        string[] s;
                        for (var y = 0; y < heigh; y++)
                        {
                            s = file[y].TrimEnd(' ').Split(' ');
                            for (var x = 0; x < width; x++)
                            {
                                var i = int.Parse(s[x]);
                                Color pixel = new Color();
                                pixel = Color.FromArgb(i);
                                b2.SetPixel(x, y, pixel);
                            }
                        }
                        pictureBox1.Image = b2;
                        label1.Text = label1.Text + pictureBox1.Image.Width.ToString() + "x" + pictureBox1.Image.Height.ToString();
                    }
                }
                else if (int.Parse(comboBox2.Text) == 2)
                {
                    List<string> file = new List<string>();
                    if (openFileDialog3.ShowDialog() == DialogResult.OK)
                    {
                        file = File.ReadAllLines(openFileDialog3.FileName).ToList();
                        if (file.Count() < 1) return; //Файл пустой!
                        var width = file[0].Split(' ').Count() - 1; //В конце строки у нас есть дополнительный пробел!
                        var heigh = file.Count;

                        Bitmap b2 = new Bitmap(width, heigh);
                        string[] s;
                        for (var y = 0; y < heigh; y++)
                        {
                            s = file[y].TrimEnd(' ').Split(' ');
                            for (var x = 0; x < width; x++)
                            {
                                var i = int.Parse(s[x]);
                                Color pixel = new Color();
                                pixel = Color.FromArgb(i);
                                b2.SetPixel(x, y, pixel);
                            }
                        }
                        pictureBox2.Image = b2;
                        label1.Text = label1.Text + pictureBox1.Image.Width.ToString() + "x" + pictureBox1.Image.Height.ToString();
                    }
                }
            }
            catch(FormatException)
            {
                MessageBox.Show("Выбери в какой текстбокс выкидывать изображения");
            }
            //saveFileDialog1.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    pictureBox2.Image.Save(saveFileDialog1.FileName);
            //}
            //MessageBox.Show("Файл сохранен");
        }


        private void ImToTxt_Click(object sender, EventArgs e)
        {
            try
            {
                int number = int.Parse(comboBox1.Text);
                switch (number)
                {
                    case 1:
                        FromImageToTxt(pictureBox1.Image);
                        break;
                    case 2:
                        FromImageToTxt(pictureBox2.Image);
                        break;
                    default:
                        MessageBox.Show("Что-то не так с номером изображения, которое вы хотите преобразовать в txt формат");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Хэй, что-то тут не так с номером или преобразованием\n" + ex.ToString());
            }
        }
    }
}
