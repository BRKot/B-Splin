using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Сплайн
{
    public partial class Form1 : Form
    {

        float a3, a2, a1, a0;
        float b3, b2, b1, b0;

        float step;


        Graphics g;

        Random random = new Random();

        int countDote = 0;

        float[,] firstLine;
        float[,] splin;

        public Form1()
        {
            InitializeComponent();
            g = panel.CreateGraphics();
        }

        void takeCoutDote()
        {
            try
            {
                countDote = Convert.ToInt32(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Введены не верные данные!");
            }
        }

        void fillLine(ref float[,] line, int k)
        {

            for (int i = 0; i < line.GetLength(0); i++)
            {
                line[i, 0] = random.Next(10, 900);
                line[i, 1] = random.Next(20, 600);
            }
        }

        void drawDote(float[,] line, Color color)
        {

            Brush brush = new SolidBrush(color);

            for (int i = 0; i < line.GetLength(0); i++)
            {
                g.FillEllipse(brush, line[i, 0] - 10, line[i, 1] - 10, 20, 20);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            takeCoutDote();

            firstLine = new float[countDote, 2];

            fillLine(ref firstLine, 0);

            drawDote(firstLine, Color.Black);

            drawLines(firstLine, Color.Black);
        }

        void drawLines(float[,] line, Color color)
        {
            Pen pen = new Pen(color, 2);

            for (int i = 0; i < line.GetLength(0) - 1; i++)
            {
                g.DrawLine(pen, line[i, 0], line[i, 1], line[i + 1, 0], line[i + 1, 1]);
            }

            g.DrawLine(pen, line[0, 0], line[0, 1], line[line.GetLength(0) - 1, 0], line[line.GetLength(0) - 1, 1]);
        }

        void drawSplin(float[,] line, Color color)
        {
            Pen pen = new Pen(color, 2);

            for (int i = 0; i < line.GetLength(0) - 1; i++)
            {
                g.DrawLine(pen, line[i, 0], line[i, 1], line[i + 1, 0], line[i + 1, 1]);
            }
        }

        void clearLines(float[,] line)
        {

            drawLines(line, panel.BackColor);
            drawDote(line, panel.BackColor);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(panel.BackColor);
        }

        void clearSplin()
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < firstLine.GetLength(0); i++)
            {
                points.Add(new Point((int)firstLine[i, 0], (int)firstLine[i, 1]));
            }

            points.Add(new Point((int)firstLine[0, 0], (int)firstLine[0, 1]));

            Pen pen = new Pen(panel.BackColor, 10);

            for (int i = 0; i < points.Count; i++)
            {
                g.DrawCurve(pen, points.ToArray());
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{

        //    List<Point> points = new List<Point>();
        //    for(int i = 0; i < firstLine.GetLength(0); i++)
        //    {
        //        points.Add(new Point((int)firstLine[i, 0], (int)firstLine[i, 1]));
        //    }

        //    //points.Add(new Point((int)firstLine[0, 0], (int)firstLine[0, 1]));

        //    Pen pen = new Pen(Color.Green, 5);

        //    for (int i = 0; i < points.Count; i++)
        //    {
        //        g.DrawClosedCurve(pen, points.ToArray());
        //    }
        //}
        private void button2_Click(object sender, EventArgs e)
        {

            takeStep();

            float t = 0;
            float constT = step;

            

            splin = new float[countDote*100, 2];
            PointF[] points = new PointF[100];


            for (int j = 1; j <= countDote; j++)
            {
                List<PointF> p = new List<PointF>();

                int x1 = j, x2, x3, x4;

                t = 0;

               if (j == countDote)
               {
                    x1 = j-1;
                    x2 = 0;
                    x3 = 1;
                    x4 = 2;
               }
               else if (j == countDote - 1)
               {
                    x1 = j - 1;
                    x2 = j;
                    x3 = 0;
                    x4 = 1;
               }
               else if (j == countDote - 2)
               {
                    x1 = j-1;
                    x2 = j;
                    x3 = j+1;
                    x4 = 0;
               }
                else
                {
                    x1 = j - 1;
                    x2 = j;
                    x3 = j + 1;
                    x4 = j + 2;
                }

                getA3(firstLine[x1, 0], firstLine[x2, 0], firstLine[x3, 0], firstLine[x4, 0]);
                getA2(firstLine[x1, 0], firstLine[x2, 0], firstLine[x3, 0], firstLine[x4, 0]);
                getA1(firstLine[x1, 0], firstLine[x2, 0], firstLine[x3, 0], firstLine[x4, 0]);
                getA0(firstLine[x1, 0], firstLine[x2, 0], firstLine[x3, 0], firstLine[x4, 0]);

                getB3(firstLine[x1, 1], firstLine[x2, 1], firstLine[x3, 1], firstLine[x4, 1]);
                getB2(firstLine[x1, 1], firstLine[x2, 1], firstLine[x3, 1], firstLine[x4, 1]);
                getB1(firstLine[x1, 1], firstLine[x2, 1], firstLine[x3, 1], firstLine[x4, 1]);
                getB0(firstLine[x1, 1], firstLine[x2, 1], firstLine[x3, 1], firstLine[x4, 1]);

                for (int i = j; i < 100; i++)
                {
                    PointF newP = new PointF();
                    newP.X = ((a3 * t + a2) * t + a1) * t + a0;
                    newP.Y = ((b3 * t + b2) * t + b1) * t + b0;
                    p.Add(newP);
                    t += step;

                }



                for (int i = 0; i < 100-2; i++)
                {
                    points[i].X = p[i].X;
                    points[i].Y = p[i].Y;

                }
                g.DrawCurve(new Pen(Color.Black), points, 0.01f);
                


            }
            //for (int i = 0; i < points.Length; i++)
            //{
            //    points[i].X = p[i].X;
            //    points[i].Y = p[i].Y;

            //}
            //g.DrawCurve(new Pen(Color.Black), points);
        }

        void takeStep()
        {
            step = 0.01f;
        }

        void getA3(float x0, float x1, float x2, float x3) { a3 = (-x0 + 3 * (x1 - x2) + x3) / 6; }
        void getA2(float x0, float x1, float x2, float x3) { a2 = (x0 - 2* x1 + x2)/2; }
        void getA1(float x0, float x1, float x2, float x3) { a1 = (x2 - x0) / 2; }
        void getA0(float x0, float x1, float x2, float x3) { a0 = (x0 + 4*x1 +x1*2) / 6; }
        void getB3(float y0, float y1, float y2, float y3) { b3 = (-y0 + 3 * (y1 - y2) + y3) / 6; }
        void getB2(float y0, float y1, float y2, float y3) { b2 = (y0 - 2 * y1 + y2) / 2; }
        void getB1(float y0, float y1, float y2, float y3) { b1 = (y2 - y0) / 2; }
        void getB0(float y0, float y1, float y2, float y3) { b0 = (y0 + 4 * y1 + y1 * 2) / 6; }

    }
}
