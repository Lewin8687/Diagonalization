using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bluebit.MatrixLibrary;

namespace PatternRecognition2
{
    public partial class Assign2 : Form
    {
        private double[] means1;
        private double[] means2;
        private double[,] matrix1;
        private double[,] matrix2;
        public Assign2()
        {
            InitializeComponent();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            var a = Convert.ToDouble(txb_a.Text);
            var b = Convert.ToDouble(txb_b.Text);
            var c = Convert.ToDouble(txb_c.Text);
            var alpha = Convert.ToDouble(txb_alpha.Text);

            matrix1 = new double[3, 3] { 
                { a*a, alpha*a*b, alpha*a*c }, 
                { alpha*a*b, b*b, alpha*b*c }, 
                { alpha*a*c, alpha*b*c, c*c } };
            matrix2 = new double[3, 3] { 
                { c*c, alpha*b*c, alpha*a*c }, 
                { alpha*b*c, b*b, alpha*a*b }, 
                { alpha*a*c, alpha*a*b, a*a } };

            means1 = new double[3] { 
                Convert.ToInt32(txb_m11.Text), 
                Convert.ToInt32(txb_m12.Text), 
                Convert.ToInt32(txb_m13.Text) };
            means2 = new double[3] { 
                Convert.ToInt32(txb_m21.Text), 
                Convert.ToInt32(txb_m22.Text), 
                Convert.ToInt32(txb_m23.Text) };
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            Convertor();
        }
        public void Convertor()
        {
            var eigen1 = new Eigen(matrix1);
            
            var p1 = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    p1[i, j] = eigen1.Eigenvectors[i, j].Real;
                }
            }
            var y1 = Matrix.Multiply(Matrix.Multiply(p1.Inverse(), matrix1), p1);
            var diagonalizing = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                diagonalizing[i, i] = 1 / Math.Sqrt(y1[i, i]);
            }
            var y2 = Matrix.Multiply(p1.Transpose(),Matrix.Multiply(matrix2,p1));
            var z2 = Matrix.Multiply(Matrix.Multiply(diagonalizing.Transpose(), y2),diagonalizing);
            
            var eigen2 = new Eigen(z2);
            var eigen_vectors2 = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    eigen_vectors2[i, j] = eigen2.Eigenvectors[i, j].Real;
                }
            }
            var v2 = Matrix.Multiply(Matrix.Multiply(eigen_vectors2.Transpose(),z2), eigen_vectors2);
            var new_means1 = Matrix.Multiply(eigen_vectors2,Matrix.Multiply(diagonalizing, Matrix.Multiply(p1.Transpose(), means1)));
            var new_means2 = Matrix.Multiply(eigen_vectors2,Matrix.Multiply(Matrix.Multiply(diagonalizing, p1.Transpose()), means2));
            PrintMatrix(y1, "y1");
            PrintMatrix(y2, "y2");
            PrintMatrix(z2, "z2");
            PrintMatrix(v2, "v2");

            eigen2 = new Eigen(matrix2);
            var p2 = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    p2[i, j] = eigen2.Eigenvectors[i, j].Real;
                }
            }
            var y = Matrix.Multiply(Matrix.Multiply(p2.Inverse(), matrix2), p2);
            var diagonalizing2 = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                diagonalizing2[i, i] = Math.Sqrt(y[i, i]);
            }
            var diagonalizing3 = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                diagonalizing3[i, i] = Math.Sqrt(y1[i, i]);
            }
            for (int i = 0; i < 200; i++)
            {
                var penBlue = new Pen(Color.Blue);
                var penRed = new Pen(Color.Red);
                var vector1 = GenerateGaussianVector(means1);
                vector1 = Matrix.Multiply(p1, Matrix.Multiply(diagonalizing3, vector1));
                var vector2 = GenerateGaussianVector(means2);
                vector2 = Matrix.Multiply(p2, Matrix.Multiply(diagonalizing2, vector2));
                // x1-x2 before
                var g = pn_12before.CreateGraphics();
                var point = new Point((int)(vector1[0] * 6 + 150), (int)(vector1[1] * 6 + 130));
                var s = new System.Drawing.Size(2,2);
                var circle = new Rectangle(point,s);
                g.DrawRectangle(penBlue, circle);

                point = new Point((int)(vector2[0] * 6 + 150), (int)(vector2[1] * 6 + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penRed, circle);
                // x1-x3 before
                g = pn_13before.CreateGraphics();
                point = new Point((int)(vector1[0] * 6 + 150), (int)(vector1[2] * 6 + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penBlue, circle);

                point = new Point((int)(vector2[0] * 6 + 150), (int)(vector2[2] * 6 + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penRed, circle);
                // after
                vector1 = GenerateGaussianVector(new_means1);
                vector2 = GenerateGaussianVector(new_means2);
                var eigen = new Eigen(v2);
                var m = new Matrix();
                for (int k = 0; k < 3; k++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        m[k, j] = eigen.Eigenvectors[k, j].Real;
                    }
                }
                var result1 = Matrix.Multiply(Matrix.Multiply(m.Inverse(), v2), m);
                var diagonal = new Matrix();
                for (int k = 0; k < 3; k++)
                    diagonal[k, k] = 1 / Math.Sqrt(result1[k, k]);
                vector2 = Matrix.Multiply(m, Matrix.Multiply(diagonal, vector2));
                // x1-x2 after
                g = pn_12after.CreateGraphics();
                int scalar = 10;
                point = new Point((int)(vector1[0] * scalar + 110), (int)(vector1[1] * scalar + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penBlue, circle);

                point = new Point((int)(vector2[0] * scalar + 110), (int)(vector2[1] * scalar + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penRed, circle);
                // x1-x3 after
                g = pn_13after.CreateGraphics();
                point = new Point((int)(vector1[0] * scalar + 110), (int)(vector1[2] * scalar + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penBlue, circle);

                point = new Point((int)(vector2[0] * scalar + 110), (int)(vector2[2] * scalar + 130));
                circle = new Rectangle(point, s);
                g.DrawRectangle(penRed, circle);
            }
        }
        public void PrintMatrix(Matrix matrix, string name)
        {
            txb_result.Text += name + ":\r\n";
            txb_result.Text += matrix.ToString();
            txb_result.Text += "\r\n";
        }
        private static Random rand = new Random();
        public double[] GenerateGaussianVector(double[] mean)
        {
            var vector = new double[mean.Length];
            for (int i = 0; i < mean.Length; i++)
            {
                double temp = 0;
                for (int j = 0; j < 12; j++)
                {
                    int randomNumber = rand.Next(0, 100);
                    temp = temp + (double)randomNumber / 100;
                }
                temp = temp - 6;
                vector[i] = mean[i] + temp;
            }
            return vector;
        }
    }
}
