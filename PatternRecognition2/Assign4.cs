using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace PatternRecognition2
{
    public partial class Assign4 : Form
    {
        private double[] means1;//origin means for class 1
        private double[] means2;//origin means for class 2
        private double[,] covariance1;//origin covariance for class 1
        private double[,] covariance2;//origin covariance for class 2
        private double[,] ori_points1;//origin 200 points for class 1
        private double[,] ori_points2;//origin 200 points for class 2
        public Assign4()
        {
            InitializeComponent();
        }

        private void Assign4_Load(object sender, EventArgs e)
        {

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            //initialization
            var a = Convert.ToDouble(txb_a.Text);
            var b = Convert.ToDouble(txb_b.Text);
            var c = Convert.ToDouble(txb_c.Text);
            var alpha = Convert.ToDouble(txb_alpha.Text);

            covariance1 = new double[3, 3] { 
                { a*a, alpha*a*b, alpha*a*c }, 
                { alpha*a*b, b*b, alpha*b*c }, 
                { alpha*a*c, alpha*b*c, c*c } };
            covariance2 = new double[3, 3] { 
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

            ori_points1 = new double[200, 3];
            ori_points2 = new double[200, 3];
            pn_12before.Refresh();
            pn_13before.Refresh();
            pn_12after.Refresh();
            pn_13after.Refresh();
            pn_b_x1.Refresh();
            pn_b_x2.Refresh();
            pn_b_x3.Refresh();
            pn_ml_x1.Refresh();
            pn_ml_x2.Refresh();
            pn_ml_x3.Refresh();
            txb_result.Text = "";

            #region Before diagonalization
            //generate 200 points
            ori_points1 = GenerateVectors(200, means1, covariance1);
            ori_points2 = GenerateVectors(200, means2, covariance2);
            //plot in x1-x2 and x1-x3
            for (int i = 0; i < 200; i++)
            {
                Draw(pn_12before, Color.Blue, ori_points1[i, 0], ori_points1[i, 1]);
                Draw(pn_13before, Color.Blue, ori_points1[i, 0], ori_points1[i, 2]);
                Draw(pn_12before, Color.Red, ori_points2[i, 0], ori_points2[i, 1]);
                Draw(pn_13before, Color.Red, ori_points2[i, 0], ori_points2[i, 2]);
            }
            //estimate likelihood
            var estimated_means1 = EstimateMeans(ori_points1, 0, -1, 0);
            var estimated_means2 = EstimateMeans(ori_points2, 0, -1, 0);
            
            var estimated_covariance1 = EstimateCovariance(estimated_means1, ori_points1, 0, -1);
            var estimated_covariance2 = EstimateCovariance(estimated_means2, ori_points2, 0, -1);
            txb_result.Text += "BEFORE DIAGONALIZATION\r\n";
            txb_result.Text += "True means for class 1: \r\n" + Tostring(means1) + "\r\n";
            txb_result.Text += "True means for class 2: \r\n" + Tostring(means2) + "\r\n";
            txb_result.Text += "True covariance for class 1: \r\n" + Tostring(covariance1) + "\r\n";
            txb_result.Text += "True covariance for class 2: \r\n" + Tostring(covariance2) + "\r\n";
            txb_result.Text += "ML means estimated for class 1: \r\n" + Tostring(estimated_means1) + "\r\n";
            txb_result.Text += "ML covariance estimated for class 1: \r\n" + estimated_covariance1.ToString() + "\r\n";
            txb_result.Text += "ML means estimated for class 2: \r\n" + Tostring(estimated_means2) + "\r\n";
            txb_result.Text += "ML covariance estimated for class 2: \r\n" + estimated_covariance2.ToString() + "\r\n";
            // plot convergence of parameters
            var double_converge = new double[means1.Length];
            for (int i = 1; i < 100; i++)
            {
                // likelihood
                double_converge = EstimateMeans(ori_points1, 0, -1, i);
                Draw(pn_ml_x1, Color.Black, (i * 2 - 150) / 8, (double_converge[0] * 3 + 20 - 150) / 8);
                Draw(pn_ml_x2, Color.Black, (i * 2 - 150) / 8, (double_converge[1] * 3 + 20 - 150) / 8);
                Draw(pn_ml_x3, Color.Black, (i * 2 - 150) / 8, (double_converge[2] * 3 + 20 - 150) / 8);
                // bayesian
                double_converge = BayesEstimateMeans(ori_points1, covariance1, i);
                Draw(pn_b_x1, Color.Black, (i * 2 - 150) / 8, (double_converge[0] * 3 + 20 - 150) / 8);
                Draw(pn_b_x2, Color.Black, (i * 2 - 150) / 8, (double_converge[1] * 3 + 20 - 150) / 8);
                Draw(pn_b_x3, Color.Black, (i * 2 - 150) / 8, (double_converge[2] * 3 + 20 - 150) / 8);
            }

            // Bayesian estimation
            var bayes_estimated_means1 = BayesEstimateMeans(ori_points1, covariance1, 0);
            var bayes_estimated_means2 = BayesEstimateMeans(ori_points2, covariance2, 0);
            txb_result.Text += "Bayesian means estimated for class 1: \r\n" + Tostring(bayes_estimated_means1) + "\r\n";
            txb_result.Text += "Bayesian means estimated for class 2: \r\n" + Tostring(bayes_estimated_means2) + "\r\n";
            // Use estimated distribution, Bayes discriminant function
            DrawDiscreminant(pn_12before, estimated_covariance1, estimated_covariance2, means1, means2, 0, 1);
            DrawDiscreminant(pn_13before, estimated_covariance1, estimated_covariance2, means1, means2, 0, 2);
            
            // Testing
            // ten fold
            var accuracy_10 = Test(20, ori_points1, ori_points2);
            txb_result.Text += "10-fold cross validation, Accuracy: \r\n" + accuracy_10 + "\r\n";
            // 400 fold
            var accuracy_400 = Test(1, ori_points1, ori_points2);
            txb_result.Text += "Leave-one-out method, Accuracy: \r\n" + accuracy_400 + "\r\n";
#endregion
            #region After diagonalization
            // diagonalize
            var matrix_covariance1 = Matrix<double>.Build.DenseOfArray(covariance1);
            var matrix_covariance2 = Matrix<double>.Build.DenseOfArray(covariance2);
            var p1 = matrix_covariance1.Evd().EigenVectors;
            var y1 = p1.Inverse() * matrix_covariance1 * p1;
            var diagonalizing = Matrix<double>.Build.Dense(means1.Length, means1.Length, 0);
            for (int i = 0; i < 3; i++)
            {
                diagonalizing[i, i] = 1 / Math.Sqrt(y1[i, i]);
            }
            var y2 = p1.Transpose() * matrix_covariance2 * p1;
            var z2 = diagonalizing.Transpose() * y2 * diagonalizing;
            var eigen_vectors2 = z2.Evd().EigenVectors;
            var v2 = eigen_vectors2.Transpose() * z2 * eigen_vectors2;

            var new_means1 = eigen_vectors2 * diagonalizing * p1.Transpose() * Matrix<double>.Build.DenseOfColumnArrays(means1);
            var new_means2 = eigen_vectors2 * diagonalizing * p1.Transpose() * Matrix<double>.Build.DenseOfColumnArrays(means2);
            //generate 200 points
            var arr_means1 = new double[] { new_means1[0, 0], new_means1[1, 0], new_means1[2, 0] };
            var arr_means2 = new double[] { new_means2[0, 0], new_means2[1, 0], new_means2[2, 0] };
            var identity = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            var matrix_v2 = new double[,]{
                {v2[0,0],v2[0,1],v2[0,2]},
                {v2[1,0],v2[1,1],v2[1,2]},
                {v2[2,0],v2[2,1],v2[2,2]}
            };
            var new_points1 = GenerateVectors(200, arr_means1, identity);
            var new_points2 = GenerateVectors(200, arr_means2, matrix_v2);
            //plot in x1-x2 and x1-x3
            for (int i = 0; i < 200; i++)
            {
                Draw(pn_12after, Color.Blue, new_points1[i, 0], new_points1[i, 1]);
                Draw(pn_13after, Color.Blue, new_points1[i, 0], new_points1[i, 2]);
                Draw(pn_12after, Color.Red, new_points2[i, 0], new_points2[i, 1]);
                Draw(pn_13after, Color.Red, new_points2[i, 0], new_points2[i, 2]);
            }
            //estimate likelihood
            var estimated__means1 = EstimateMeans(new_points1, 0, -1, 0);
            var estimated__means2 = EstimateMeans(new_points2, 0, -1, 0);
            var estimated__covariance1 = EstimateCovariance(estimated__means1, new_points1, 0, -1);
            var estimated__covariance2 = EstimateCovariance(estimated__means2, new_points2, 0, -1);
            txb_result.Text += "AFTER DIAGONALIZATION\r\n";
            txb_result.Text += "True means for class 1: \r\n" + Tostring(arr_means1) + "\r\n";
            txb_result.Text += "True means for class 2: \r\n" + Tostring(arr_means2) + "\r\n";
            txb_result.Text += "True covariance for class 1: \r\n" + Tostring(identity) + "\r\n";
            txb_result.Text += "True covariance for class 2: \r\n" + Tostring(matrix_v2) + "\r\n";
            txb_result.Text += "ML means estimated for class 1: \r\n" + Tostring(estimated__means1) + "\r\n";
            txb_result.Text += "ML covariance estimated for class 1: \r\n" + estimated__covariance1.ToString() + "\r\n";
            txb_result.Text += "ML means estimated for class 2: \r\n" + Tostring(estimated__means2) + "\r\n";
            txb_result.Text += "ML covariance estimated for class 2: \r\n" + estimated__covariance2.ToString() + "\r\n";
            
            // Bayesian estimation
            var bayes__estimated_means1 = BayesEstimateMeans(new_points1, identity, 0);
            var bayes__estimated_means2 = BayesEstimateMeans(new_points2, matrix_v2, 0);
            txb_result.Text += "Bayesian means estimated for class 1: \r\n" + Tostring(bayes__estimated_means1) + "\r\n";
            txb_result.Text += "Bayesian means estimated for class 2: \r\n" + Tostring(bayes__estimated_means2) + "\r\n";
            
            // Use estimated distribution, Bayes discriminant function
            DrawDiscreminant(pn_12after, estimated__covariance1, estimated__covariance2, arr_means1, arr_means2, 0, 1);
            DrawDiscreminant(pn_13after, estimated__covariance1, estimated__covariance2, arr_means1, arr_means2, 0, 2);
            // Testing
            // ten fold
            var accuracy__10 = Test(20, new_points1, new_points2);
            txb_result.Text += "10-fold cross validation, Accuracy: \r\n" + accuracy__10 + "\r\n";
            // 400 fold
            var accuracy__400 = Test(1, new_points1, new_points2);
            txb_result.Text += "Leave-one-out method, Accuracy: \r\n" + accuracy__400 + "\r\n";
            #endregion
        }

        public string Tostring(double[] array)
        {
            string result="";
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i] + "\t";
            }
            result = "{ " + result + "}";
            return result;
        }
        public string Tostring(double[,] array)
        {
            string result="";
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    result += array[i, j] + "\t";
                }
                result += "\r\n";
            }
            return result;
        }
        public double Test(int interval, double[,] points1, double[,] points2)
        {
            int folds = points1.GetLength(0) / interval;
            int start, end;
            int correct = 0;
            int count = 0;
            for (int i = 0; i < folds; i++)
            {
                //estimate
                var estimated_means1 = EstimateMeans(points1, interval, i, 0);
                var estimated_means2 = EstimateMeans(points2, interval, i, 0);

                var estimated_covariance1 = EstimateCovariance(estimated_means1, points1, interval, i);
                var estimated_covariance2 = EstimateCovariance(estimated_means2, points2, interval, i);
                //test
                start = i * interval;
                end = start + interval;
                double[] vector;
                for (int j = start; j < end; j++)
                {
                    //for class 1
                    vector = new double[] { points1[j, 0], points1[j, 1], points1[j, 2] };
                    if (1 == SingleTest(estimated_covariance1, estimated_covariance2, estimated_means1, estimated_means2, vector))
                    {
                        correct++;
                    }
                    vector = new double[] { points2[j, 0], points2[j, 1], points2[j, 2] };
                    if (2 == SingleTest(estimated_covariance1, estimated_covariance2, estimated_means1, estimated_means2, vector))
                    {
                        correct++;
                    }
                    count += 2;
                }
            }
            return (double)correct/count;
        }
        private int SingleTest(Matrix<double> matrix1, Matrix<double> matrix2, double[] means1, double[] means2, double[] vector)
        {
            var matrix_means1 = Matrix<double>.Build.DenseOfColumnArrays(means1);
            var matrix_means2 = Matrix<double>.Build.DenseOfColumnArrays(means2);
            var matrix_vector = Matrix<double>.Build.DenseOfColumnArrays(vector);
            // Calculate A B C
            var A = matrix2.Inverse() - matrix1.Inverse();
            var B = 2 * (matrix_means1.Transpose() * matrix1.Inverse()) - matrix_means2.Transpose() * matrix2.Inverse();
            var C = (matrix_means2.Transpose() * matrix2.Inverse() * matrix_means2).Determinant() - (matrix_means1.Transpose() * matrix1.Inverse() * matrix_means1).Determinant() - 2 * (Math.Log10(matrix1.Determinant() / matrix2.Determinant()));
            var result = (matrix_vector.Transpose() * A * matrix_vector).Determinant() + B[0, 0] * matrix_vector[0 ,0] + B[0, 1] * matrix_vector[1, 0] + B[0, 2] * matrix_vector[2, 0] + C;
            if (result > 0)
            {
                return 1;
            }
            return 2;
        }
        public void DrawDiscreminant(Panel p, Matrix<double> matrix1, Matrix<double> matrix2, double[] means1, double[] means2, int x, int y)
        {
            var matrix_means1 = Matrix<double>.Build.DenseOfColumnArrays(means1);
            var matrix_means2 = Matrix<double>.Build.DenseOfColumnArrays(means2);
            // Calculate A B C
            var A = matrix2.Inverse() - matrix1.Inverse();
            var B = 2 * (matrix_means1.Transpose() * matrix1.Inverse() - matrix_means2.Transpose() * matrix2.Inverse());
            var C = (matrix_means2.Transpose() * matrix2.Inverse() * matrix_means2).Determinant() - (matrix_means1.Transpose() * matrix1.Inverse() * matrix_means1).Determinant() - 2 * (Math.Log10(matrix1.Determinant() / matrix2.Determinant()));
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
                    catch (Exception)
                    { }
                }
            }
        }


        public Matrix<double> EstimateCovariance(double[] means, double[,] data, int interval, int fold_number)
        {
            var covariance = Matrix<double>.Build.Dense(means.Length, means.Length, 0);
            var vec_data = Matrix<double>.Build.Dense(1, means.Length, 1);
            var vec_means = Matrix<double>.Build.Dense(1, means.Length, 1);
            var transposed_data = Matrix<double>.Build.Dense(means.Length, 1);
            var transposed_means = Matrix<double>.Build.Dense(means.Length, 1);
            for (int j = 0; j < means.Length; j++)
            {
                transposed_means[j, 0] = means[j];
                vec_means[0, j] = means[j];
            }
            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (i < interval * fold_number || i >= interval * (fold_number + 1))
                {
                    vec_data[0, 0] = data[i, 0];
                    vec_data[0, 1] = data[i, 1];
                    vec_data[0, 2] = data[i, 2];

                    for (int j = 0; j < means.Length; j++)
                    {
                        transposed_data[j, 0] = data[i, j];
                    }
                    covariance += (transposed_data - transposed_means) * (vec_data - vec_means);
                }
            }
            int count = data.GetLength(0);
            if (fold_number > -1)
            {
                count -= interval;
            }
            return covariance/count;
        }

        public double[] EstimateMeans(double[,] data, int interval, int fold_number, int count)
        {
            if (count == 0)
            {
                count = data.GetLength(0);
            }
            var result = new double[3];
            var temp = new double[3];
            for (int i = 0; i < count; i++)
            {
                if (i < interval * fold_number || i >= interval * (fold_number + 1))
                {
                    temp[0] += data[i, 0];
                    temp[1] += data[i, 1];
                    temp[2] += data[i, 2];
                }
            }
            if (fold_number > -1)
            {
                count -= interval;
            }
            result[0] = temp[0] / count;
            result[1] = temp[1] / count;
            result[2] = temp[2] / count;
            return result;
        }

        public double[] BayesEstimateMeans(double[,] data, double[,] covariance, int count)
        {
            if (count == 0)
            {
                count = data.GetLength(0);
            }
            var matrix_covariance = Matrix<double>.Build.DenseOfArray(covariance);
            var result = new double[data.GetLength(1)];
            var means_init = Matrix<double>.Build.DenseOfColumnArrays(new double[] { 1, 1, 1 });
            var covariance_init = Matrix<double>.Build.DenseDiagonal(data.GetLength(1), data.GetLength(1), 1);
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    result[j] += data[i, j];
                }
            }
            for (int j = 0; j < data.GetLength(1); j++)
            {
                result[j] = result[j] / count;
            }
            var temp = Matrix<double>.Build.DenseOfColumnArrays(result);

            temp = covariance_init * (covariance_init + matrix_covariance / count).Inverse() * temp + matrix_covariance * (covariance_init + matrix_covariance / count).Inverse() * means_init / count;
            for (int j = 0; j < data.GetLength(1); j++)
            {
                result[j] = temp[j,0];
            }
            return result;
        }
        public void Draw(Panel p, Color c, double x, double y)
        {
            var g = p.CreateGraphics();
            var pen = new Pen(c);
            var point = new Point((int)(x * 8 + 150), (int)(y * 8 + 150));
            var s = new System.Drawing.Size(2, 2);
            var circle = new Rectangle(point, s);
            g.DrawRectangle(pen, circle);
        }

        public double[,] GenerateVectors(int number, double[] means, double[,] covariance)
        {
            var result = new double[number, means.Length];
            var temp = new double[means.Length];
            for (int k = 0; k < number; k++)
            {
                temp = GenerateGaussianVector(means);
                var vector = Vector<double>.Build.DenseOfArray(temp);  
                //PD(1/2)X
                var matrix = Matrix<double>.Build.DenseOfArray(covariance);
                var eigen = matrix.Evd();
                var p = Matrix<double>.Build.Dense(means.Length, means.Length);
                for (int i = 0; i < means.Length; i++)
                {
                    for (int j = 0; j < means.Length; j++)
                    {
                        p[i, j] = eigen.EigenVectors[i, j];
                    }
                }
                var y = p.Inverse() * matrix * p;
                var diagonalizing = Matrix<double>.Build.DenseDiagonal(means.Length, means.Length, 1);
                for (int i = 0; i < means.Length; i++)
                {
                    diagonalizing[i, i] = Math.Sqrt(y[i, i]);
                }
                var z = p * diagonalizing * vector;
                for (int i = 0; i < means.Length; i++)
                {
                    result[k, i] = z[i] + means[i];
                }
            }
            return result;
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
                vector[i] = temp;
            }
            return vector;
        }
    }
}
