using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bluebit.MatrixLibrary;

namespace PatternRecognition2
{
    public partial class Assign3 : Form
    {
        private double[] means1;
        private double[] means2;
        private double[,] matrix1;
        private double[,] matrix2;
        private double[,] orig_vectors1 = new double[200, 3];
        private double[,] orig_vectors2 = new double[200, 3];
        private double[] new_means1;
        private double[] new_means2;
        public Assign3()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, EventArgs e)
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

            pn_12before.Refresh();
            pn_13before.Refresh();
            pn_12after.Refresh();
            pn_13after.Refresh();
            txb_result.Text = "";
            Convertor();
        }
        
        public void Convertor()
        {
            #region Calculate v2 new means
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
            var y2 = Matrix.Multiply(p1.Transpose(), Matrix.Multiply(matrix2, p1));
            var z2 = Matrix.Multiply(Matrix.Multiply(diagonalizing.Transpose(), y2), diagonalizing);
            var eigen2 = new Eigen(z2);
            var eigen_vectors2 = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    eigen_vectors2[i, j] = eigen2.Eigenvectors[i, j].Real;
                }
            }
            var v2 = Matrix.Multiply(Matrix.Multiply(eigen_vectors2.Transpose(), z2), eigen_vectors2);
            new_means1 = Matrix.Multiply(eigen_vectors2, Matrix.Multiply(diagonalizing, Matrix.Multiply(p1.Transpose(), means1)));
            new_means2 = Matrix.Multiply(eigen_vectors2, Matrix.Multiply(Matrix.Multiply(diagonalizing, p1.Transpose()), means2));
            #endregion

            #region generate 200 points
            for (int i = 0; i < 200; i++)
            {
                var orig_vector1 = GenerateGaussianVector(means1);
                var orig_vector2 = GenerateGaussianVector(means2);
                var vector1 = AddCovariance(matrix1, orig_vector1);
                var vector2 = AddCovariance(matrix2, orig_vector2);
                //for (int j = 0; i < 3;j++ )
                //{
                //    orig_vectors1[i, j] = orig_vector1[j];
                //    orig_vectors2[i, j] = orig_vector2[j];
                //}
                try
                {
                    Draw(pn_12before, Color.Blue, vector1[0], vector1[1]);
                    Draw(pn_13before, Color.Blue, vector1[0], vector1[2]);
                    Draw(pn_12before, Color.Red, vector2[0], vector2[1]);
                    Draw(pn_13before, Color.Red, vector2[0], vector2[2]);

                    // after
                    //vector1 = ModifyVector(vector1, means1);
                    //vector2 = ModifyVector(vector2, means2);
                    vector1 = GenerateGaussianVector(new_means1);
                    vector2 = GenerateGaussianVector(new_means2);

                    vector2 = AddCovariance(z2, vector2);

                    Draw(pn_12after, Color.Blue, vector1[0], vector1[1]);
                    Draw(pn_13after, Color.Blue, vector1[0], vector1[2]);
                    Draw(pn_12after, Color.Red, vector2[0], vector2[1]);
                    Draw(pn_13after, Color.Red, vector2[0], vector2[2]);
                }
                catch(Exception)
                { }
            }
            #endregion

            #region Calculate discreminant function
            var E = new double[,]{{1,0,0},{0,1,0},{0,0,1}};
            var identity = new Matrix(E);
            
            DrawDiscreminant(pn_12before, matrix1, matrix2, means1, means2, 0, 1);
            DrawDiscreminant(pn_13before, matrix1, matrix2, means1, means2, 0, 2);
            DrawDiscreminant(pn_12after, identity, z2, new_means1, new_means2, 0, 1);
            DrawDiscreminant(pn_13after, identity, z2, new_means1, new_means2, 0, 2);
            #endregion

            #region Test
            var correct = 0;
            var wrong = 0;
            // Before
            for(int i=0;i<200;i++)
            {
                var vector1 = GenerateGaussianVector(means1);
                var vector2 = GenerateGaussianVector(means2);
                vector1 = AddCovariance(matrix1, vector1);
                vector2 = AddCovariance(matrix2, vector2);

                if (1 == Test(matrix1, matrix2, means1, means2, vector1))
                {
                    correct++;
                }
                else
                {
                    wrong++;
                }
                if (2 == Test(matrix1, matrix2, means1, means2, vector2))
                {
                    correct++;
                }
                else
                {
                    wrong++;
                }
            }
            txb_result.Text += "Before:\r\ncorrect: " + correct.ToString() + "\r\nwrong: " + wrong.ToString();
            txb_result.Text += "\r\nAccuracy: " + (double)correct / (correct + wrong);
            // After
            correct = 0;
            wrong = 0;
            for(int i=0;i<200;i++)
            {
                var vector1 = GenerateGaussianVector(new_means1);
                var vector2 = GenerateGaussianVector(new_means2);
                vector2 = AddCovariance(v2, vector2);

                if (1 == Test(identity, v2, new_means1, new_means2, vector1))
                {
                    correct++;
                }
                else
                {
                    wrong++;
                }
                if (2 == Test(identity, v2, new_means1, new_means2, vector2))
                {
                    correct++;
                }
                else
                {
                    wrong++;
                }
            }
            txb_result.Text += "\r\nAfter:\r\ncorrect: " + correct.ToString() + "\r\nwrong: " + wrong.ToString();
            txb_result.Text += "\r\nAccuracy: " + (double)correct / (correct + wrong);
            #endregion
        }
        public int Test(Matrix matrix1, Matrix matrix2, double[] means1, double[] means2, double[] vector)
        {
            var m1 = new Matrix(matrix1);
            var m2 = new Matrix(matrix2);
            var matrix_means1 = new Vector(means1);
            var matrix_means2 = new Vector(means2);
            var matrix_vector = new Vector(vector);
            // Calculate A B C
            var A = m2.Inverse() - m1.Inverse();
            var B = 2 * (Matrix.Multiply(matrix_means1.ToMatrix().Transpose(), m1.Inverse()) - Matrix.Multiply(matrix_means2.ToMatrix().Transpose(), m2.Inverse()));
            var C = (Matrix.Multiply(Matrix.Multiply(matrix_means2.ToMatrix().Transpose(), m2.Inverse()), matrix_means2.ToMatrix())).Determinant() - (Matrix.Multiply(Matrix.Multiply(matrix_means1.ToMatrix().Transpose(), m1.Inverse()), matrix_means1.ToMatrix()).Determinant() - 2 * (Math.Log10(m1.Determinant() / m2.Determinant())));
            var result = Matrix.Multiply(Matrix.Multiply(matrix_vector.ToMatrix().Transpose(), A), matrix_vector.ToMatrix()).Determinant() + B[0, 0] * matrix_vector[0] + B[0, 2] * matrix_vector[2] + B[0, 2] * matrix_vector[2] + C;
            if (result > 0)
            {
                return 1;
            }
            return 2;
        }
        public void DrawDiscreminant(Panel p, Matrix matrix1, Matrix matrix2, double[] means1, double[] means2, int x, int y)
        {
            var m1 = new Matrix(matrix1);
            var m2 = new Matrix(matrix2);
            var matrix_means1 = new Vector(means1);
            var matrix_means2 = new Vector(means2);
            // Calculate A B C
            var A = m2.Inverse() - m1.Inverse();
            var B = 2 * (Matrix.Multiply(matrix_means1.ToMatrix().Transpose(), m1.Inverse()) - Matrix.Multiply(matrix_means2.ToMatrix().Transpose(), m2.Inverse()));
            var C = (Matrix.Multiply(Matrix.Multiply(matrix_means2.ToMatrix().Transpose(), m2.Inverse()), matrix_means2.ToMatrix())).Determinant() - (Matrix.Multiply(Matrix.Multiply(matrix_means1.ToMatrix().Transpose(), m1.Inverse()), matrix_means1.ToMatrix()).Determinant() - 2 * (Math.Log10(m1.Determinant() / m2.Determinant())));
            var a11 = A[x, x];
            var a12 = A[x, y];
            var a21 = A[y, x];
            var a22 = A[y, y];
            var b1 = B[0, x];
            var b2 = B[0, y];
            for (double i = -20; i < 20; i = i + 0.1)
            {
                var quad = a22;
                var line = a12 * i + a21 * i + b2;
                var cons = C + b1 * i + a11 * i * i;

                if ((line * line - 4 * quad * cons) >= 0)
                {
                    try
                    {
                        if (quad != 0)
                        {
                            var j = (-line + Math.Sqrt(line * line - 4 * quad * cons)) / (2 * quad);
                            Draw(p, Color.Black, i, j);

                            j = (-line - Math.Sqrt(line * line - 4 * quad * cons)) / (2 * quad);
                            Draw(p, Color.Black, i, j);
                        }
                        else if (line != 0)
                        {
                            var j = -cons / line;
                            Draw(p, Color.Black, i, j);
                        }
                    }
                    catch(Exception)
                    { }
                }
            }
        }
        public double[] ModifyVector(double[] vector, double[] orig_means)
        {
            for (int i = 0; i < 3; i++)
            {
                vector[i] = vector[i] - orig_means[i] + new_means1[i];
            }
            return vector;
        }
        public double[] SecondAddCovariance(double[,] matrix, double[] vector)
        {
            var eigen = new Eigen(matrix);
            var p = new Matrix(); 
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    p[i, j] = eigen.Eigenvectors[i, j].Real;
                }
            }
            var y = Matrix.Multiply(Matrix.Multiply(p.Inverse(), matrix), p);
            var diagonalizing = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                diagonalizing[i, i] = 1 / Math.Sqrt(y[i, i]);
            }
            return Matrix.Multiply(Matrix.Multiply(p, diagonalizing), vector);
        }
        public double[] AddCovariance(double[,] matrix, double[] vector)
        {
            var eigen = new Eigen(matrix);
            var p = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    p[i, j] = eigen.Eigenvectors[i, j].Real;
                }
            }
            var y = Matrix.Multiply(Matrix.Multiply(p.Inverse(), matrix), p);
            var diagonalizing = new Matrix();
            for (int i = 0; i < 3; i++)
            {
                diagonalizing[i, i] = Math.Sqrt(y[i, i]);
            }
            return Matrix.Multiply(Matrix.Multiply(p, diagonalizing), vector);
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
        public void Draw(Panel p, Color c, double x, double y)
        {
            var g = p.CreateGraphics();
            var pen = new Pen(c);
            var point = new Point((int)(x * 6 + 150), (int)(y * 6 + 150));
            var s = new System.Drawing.Size(2, 2);
            var circle = new Rectangle(point, s);
            g.DrawRectangle(pen, circle);
        }
    }
}
