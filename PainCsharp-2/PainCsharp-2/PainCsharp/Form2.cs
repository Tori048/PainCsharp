using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PainCsharp
{
    public partial class Form2 : Form
    {
        private Graphics g;
        private int PixelNumber;
        public Form2(int pixelnumber)
        {
            PixelNumber = pixelnumber;
            InitializeComponent();
        }

        private void PaintGrafic(object sender, PaintEventArgs e)
        {
            g = panel1.CreateGraphics();
            g.TranslateTransform(0, panel1.Height); // смещение начала координат (в пикселях)
            //g.RotateTransform(270);
            g.ScaleTransform(4f, 4f);
            Pen gridPen = new Pen(Color.Black, 0.0001f); //перо для отрисовки координатной сетки
            Pen penCO = new Pen(Color.Green, 1f);

            g.DrawLine(penCO, new Point(-100, 0), new Point(100, 0));  // oy
            g.DrawLine(penCO, new Point(0, -100), new Point(0, 100)); // ox

            // рисуем координатную сетку
            int x = -100; //начальное значение координаты х. Постороение идет из указанной точки
            int y = -100; // начальное значение координаты у. Пояснение см выше.

            while (x <= 95) //конечное значение координаты х. Бесконечное количество линий нам не надо, ибо никакой памяти не хватит
            {
                x = x + 5; // шаг линий, параллельных оси ОУ
                y = y + 5; //шаг линий, параллельных оси ОХ
                g.DrawLine(gridPen, new Point(x, 100), new Point(x, -100)); // рисуем линии, параллельные оси ОУ
                g.DrawLine(gridPen, new Point(100, y), new Point(-100, y)); //рисуем линии, параллельные оси ОХ
            }
        }
    }
}
