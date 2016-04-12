using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MathNet.Numerics.LinearAlgebra;

namespace PatternRecognition2
{
    public partial class project : Form
    {
        private double[] means1 = new double[4];
        private double[] means2 = new double[4];
        private int m_count = 100;
        private int m_features = 4;
        private int c1 = 0;
        private int c2 = 1;
        private double[,] ori_data;
        private double[,] modified_data;
        private double[,] data_class1;
        private double[,] data_class2;
        private double[,] dig_data_class1;
        private double[,] dig_data_class2;

        public project()
        {
            InitializeComponent();
            StreamReader file;
            ori_data = new double[m_count, m_features];
            try
            {
                //file = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\data\\winequality-red.csv");
                file = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\data\\iris.txt");
            }
            catch(Exception e)
            {
                throw e;
            }
            //file.ReadLine();// Skip the first line
            for (int i = 0; i < m_count;i++ )
            {
                var line = file.ReadLine();
                var arr = line.Split(',');
                ////GET [0] [1] [3] [5] [6] [8] [9] [10] [11]
                //ori_data[i, 0] = Convert.ToDouble(arr[0]);
                //ori_data[i, 1] = Convert.ToDouble(arr[1]);
                //ori_data[i, 2] = Convert.ToDouble(arr[3]);
                //ori_data[i, 3] = Convert.ToDouble(arr[5]);
                //ori_data[i, 4] = Convert.ToDouble(arr[6]);
                //ori_data[i, 5] = Convert.ToDouble(arr[8]);
                //ori_data[i, 6] = Convert.ToDouble(arr[9]);
                //ori_data[i, 7] = Convert.ToDouble(arr[10]);
                //ori_data[i, 8] = Convert.ToDouble(arr[11]);
                ori_data[i, 0] = Convert.ToDouble(arr[0]);
                ori_data[i, 1] = Convert.ToDouble(arr[1]);
                ori_data[i, 2] = Convert.ToDouble(arr[2]);
                ori_data[i, 3] = Convert.ToDouble(arr[3]);
                //if (ori_data[i, 8] > 5)
                //{
                //    count_class1++;
                //}
                //else
                //{
                //    count_class2++;
                //}
            }
            data_class1 = new double[m_count/2, m_features];
            data_class2 = new double[m_count/2, m_features];
            for (int i = 0; i < 50; i++)
            {
                 for (int j = 0; j < m_features; j++)
                 {
                     data_class1[i, j] = ori_data[i, j];
                 }
            }
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < m_features; j++)
                {
                    data_class2[i, j] = ori_data[i+50, j];
                }
            }
            //var k = 0;
            //for (int i = 0; i < m_count; i++)
            //{
            //    if (ori_data[i, 8] > 5)
            //    {
            //        for (int j = 0; j < 8; j++)
            //        {
            //            data_class1[k, j] = ori_data[i, j];
            //        }
            //        k++;
            //    }
            //}
            //k = 0;
            //for (int i = 0; i < m_count; i++)
            //{
            //    if (ori_data[i, 8] <= 5)
            //    {
            //        for (int j = 0; j < 8; j++)
            //        {
            //            data_class2[k, j] = ori_data[i, j];
            //        }
            //        k++;
            //    }
            //}
        }
        private void project_Load(object sender, EventArgs e)
        {
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            c1 = Convert.ToInt16(txb_c1.Text);
            c2 = Convert.ToInt16(txb_c2.Text);
            for (int i = 0; i < m_count / 2; i++)
            {
                Draw(pn_qu_12before, Color.Blue, data_class1[i, c1], data_class1[i, c2]);
                Draw(pn_qu_12before, Color.Red, data_class2[i, c1], data_class2[i, c2]);
                Draw(pn_fi_before, Color.Blue, data_class1[i, c1], data_class1[i, c2]);
                Draw(pn_fi_before, Color.Red, data_class2[i, c1], data_class2[i, c2]);
                Draw(pn_hk_before, Color.Blue, data_class1[i, c1], data_class1[i, c2]);
                Draw(pn_hk_before, Color.Red, data_class2[i, c1], data_class2[i, c2]);
                Draw(pn_nn_before, Color.Blue, data_class1[i, c1], data_class1[i, c2]);
                Draw(pn_nn_before, Color.Red, data_class2[i, c1], data_class2[i, c2]);
            }
            ////// Training
            // Maximum likelihood
            var estimated_means1 = MLEstimateMeans(data_class1, 0, -1, 0);
            var estimated_means2 = MLEstimateMeans(data_class2, 0, -1, 0);
            txb_result.Text += "Maximum Likelihood Estimated Means: \r\n";
            txb_result.Text += "Class1:" + Tostring(estimated_means1) + "\r\nClass2:" + Tostring(estimated_means2) + "\r\n";
            
            var estimated_covariance1 = MLEstimateCovariance(estimated_means1, data_class1, 0, -1);
            var estimated_covariance2 = MLEstimateCovariance(estimated_means2, data_class2, 0, -1);
            txb_result.Text += "Maximum Likelihood Estimated Covariance: \r\n";
            txb_result.Text += "Class1:" + estimated_covariance1.ToString() + "\r\nClass2:" + estimated_covariance2.ToString() + "\r\n";
            
            // Bayesian
            var bayes_estimated_means1 = BayesEstimateMeans(data_class1, MatrixToArray(estimated_covariance1), 0);
            var bayes_estimated_means2 = BayesEstimateMeans(data_class2, MatrixToArray(estimated_covariance2), 0);
            txb_result.Text += "Bayesian Estimated Means: \r\n";
            txb_result.Text += "Class1:" + Tostring(bayes_estimated_means1) + "\r\nClass2:" + Tostring(bayes_estimated_means2) + "\r\n";
            
            //// Quadratic
            var accuracy_10 = QuadraticTest(5, data_class1, data_class2);
            var accuracy_1 = QuadraticTest(1, data_class1, data_class2);
            DrawDiscreminant(pn_qu_12before, estimated_covariance1, estimated_covariance2, estimated_means1, estimated_means2, c1, c2);
            txb_result.Text += "Quadratic Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_10 + "\r\nLeave one out:" + accuracy_1 + "\r\n";
            
            //// NN
            var accuracy_NN = NNTest(10, ori_data);
            var accuracy_NN_1 = NNTest(1, ori_data);
            txb_result.Text += "Nearest Neighbor Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_NN + "\r\nLeave one out:" + accuracy_NN_1 + "\r\n";
            
            //// Ho-Kashyap
            var accuracy_HK = HKTest(10, ori_data, 0.9, c1, c2, pn_hk_before);
            txb_result.Text += "Ho-Kashyap Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_HK + "\r\n";
            
            //// Fisher Discriminant
            var accuracy_Fisher = FisherTest(10, ori_data, data_class1, data_class2, c1, c2, pn_fi_before);
            txb_result.Text += "Fisher Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_Fisher + "\r\n";
            
            ///////// Diagolization
            var matrix_covariance1 = Matrix<double>.Build.DenseOfMatrix(estimated_covariance1);
            var matrix_covariance2 = Matrix<double>.Build.DenseOfMatrix(estimated_covariance2);
            var p1 = matrix_covariance1.Evd().EigenVectors;
            var y1 = p1.Inverse() * matrix_covariance1 * p1;
            var diagonalizing = Matrix<double>.Build.Dense(ori_data.GetLength(1), ori_data.GetLength(1), 0);
            for (int i = 0; i < ori_data.GetLength(1); i++)
            {
                diagonalizing[i, i] = 1 / Math.Sqrt(y1[i, i]);
            }
            var y2 = p1.Transpose() * matrix_covariance2 * p1;
            var z2 = diagonalizing.Transpose() * y2 * diagonalizing;
            var eigen_vectors2 = z2.Evd().EigenVectors;
            var v2 = eigen_vectors2.Transpose() * z2 * eigen_vectors2;

            dig_data_class1 = new double[m_count / 2, m_features];
            dig_data_class2 = new double[m_count / 2, m_features];
            modified_data = new double[m_count, m_features];
            var vector1 = Matrix<double>.Build.Dense(m_features, 1, 0);
            var vector2 = Matrix<double>.Build.Dense(m_features, 1, 0);
            for (int i = 0; i < m_count / 2; i++)
            {
                for (int j = 0; j < m_features; j++)
                {
                    vector1[j, 0] = data_class1[i, j];
                    vector2[j, 0] = data_class2[i, j];
                }
                vector1 = eigen_vectors2 * diagonalizing * p1.Transpose() * vector1;
                vector2 = eigen_vectors2 * diagonalizing * p1.Transpose() * vector1;
                for (int j = 0; j < m_features; j++)
                {
                    dig_data_class1[i, j] = vector1[j, 0];
                    dig_data_class2[i, j] = vector2[j, 0];
                    modified_data[i, j] = vector1[j, 0];
                    modified_data[i + m_count / 2, j] = vector2[j, 0];
                }
            }

            for (int i = 0; i < m_count / 2; i++)
            {
                Draw(pn_quad_after, Color.Blue, dig_data_class1[i, c1], dig_data_class1[i, c2]);
                Draw(pn_quad_after, Color.Red, dig_data_class2[i, c1], dig_data_class2[i, c2]);
                Draw(pn_fi_after, Color.Blue, dig_data_class1[i, c1], dig_data_class1[i, c2]);
                Draw(pn_fi_after, Color.Red, dig_data_class2[i, c1], dig_data_class2[i, c2]);
                Draw(pn_hk_after, Color.Blue, dig_data_class1[i, c1], dig_data_class1[i, c2]);
                Draw(pn_hk_after, Color.Red, dig_data_class2[i, c1], dig_data_class2[i, c2]);
                Draw(pn_nn_after, Color.Blue, dig_data_class1[i, c1], dig_data_class1[i, c2]);
                Draw(pn_nn_after, Color.Red, dig_data_class2[i, c1], dig_data_class2[i, c2]);
            }
            txb_result.Text += "After Diagolization\r\n";
            // Maximum likelihood
            estimated_means1 = MLEstimateMeans(dig_data_class1, 0, -1, 0);
            estimated_means2 = MLEstimateMeans(dig_data_class2, 0, -1, 0);
            txb_result.Text += "Maximum Likelihood Estimated Means: \r\n";
            txb_result.Text += "Class1:" + Tostring(estimated_means1) + "\r\nClass2:" + Tostring(estimated_means2) + "\r\n";
            
            estimated_covariance1 = MLEstimateCovariance(estimated_means1, dig_data_class1, 0, -1);
            estimated_covariance2 = MLEstimateCovariance(estimated_means2, dig_data_class2, 0, -1);
            txb_result.Text += "Maximum Likelihood Estimated Covariance: \r\n";
            txb_result.Text += "Class1:" + estimated_covariance1.ToString() + "\r\nClass2:" + estimated_covariance2.ToString() + "\r\n";
            
            // Bayesian
            bayes_estimated_means1 = BayesEstimateMeans(dig_data_class1, MatrixToArray(estimated_covariance1), 0);
            bayes_estimated_means2 = BayesEstimateMeans(dig_data_class2, MatrixToArray(estimated_covariance2), 0);
            txb_result.Text += "Bayesian Estimated Means: \r\n";
            txb_result.Text += "Class1:" + Tostring(bayes_estimated_means1) + "\r\nClass2:" + Tostring(bayes_estimated_means2) + "\r\n";
            
            //// Quadratic
            accuracy_10 = QuadraticTest(5, dig_data_class1, dig_data_class2);
            accuracy_1 = QuadraticTest(1, dig_data_class1, dig_data_class2);
            DrawDiscreminant(pn_quad_after, estimated_covariance1, estimated_covariance2, estimated_means1, estimated_means2, c1, c2);
            txb_result.Text += "Quadratic Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_10 + "\r\nLeave one out:" + accuracy_1 + "\r\n";
            
            //// NN
            accuracy_NN = NNTest(10, modified_data);
            accuracy_NN_1 = NNTest(1, modified_data);
            txb_result.Text += "Nearest Neighbor Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_NN + "\r\nLeave one out:" + accuracy_NN_1 + "\r\n";
            
            //// Ho-Kashyap
            accuracy_HK = HKTest(10, modified_data, 0.9, c1, c2, pn_hk_after);
            txb_result.Text += "Ho-Kashyap Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_HK + "\r\n";
            
            //// Fisher Discriminant
            accuracy_Fisher = FisherTest(10, modified_data,dig_data_class1, dig_data_class2, c1, c2, pn_fi_after);
            txb_result.Text += "Fisher Accuracy: \r\n";
            txb_result.Text += "Ten-fold:" + accuracy_Fisher + "\r\n";
            
        }
        public double FisherTest(int interval, double[,] points, double[,] class1, double[,] class2, int c1, int c2, Panel p)
        {
            var folds = points.GetLength(0) / interval;
            var start = 0;
            var end = 0;
            var correct = 0.0;
            var count = 0.0;
            for (int f = 0; f < folds; f++)
            {
                start = f * interval;
                end = start + interval;
                var m1 = MLEstimateMeans(class1, interval, f, 0);
                var m2 = MLEstimateMeans(class2, interval, f, 0);

                var matrix_m1 = Matrix<double>.Build.DenseOfColumnArrays(m1);
                var matrix_m2 = Matrix<double>.Build.DenseOfColumnArrays(m2);

                var x = Matrix<double>.Build.Dense(points.GetLength(1), 1, 0);
                var sw = Matrix<double>.Build.Dense(points.GetLength(1), points.GetLength(1), 0);
                var w = Matrix<double>.Build.Dense(points.GetLength(1), 1, 0);
                for (int i = 0; i < points.GetLength(0); i++)
                {
                    if (i < start || i >= end)
                    {
                        for (int j = 0; j < points.GetLength(1); j++)
                        {
                            x[j, 0] = points[i, j];
                        }
                        if (i < 50)
                        {
                            sw += (x - matrix_m1) * (x - matrix_m1).Transpose();
                        }
                        else
                        {
                            sw += (x - matrix_m2) * (x - matrix_m2).Transpose();
                        }
                    }
                }
                w = sw.Inverse() * (matrix_m1 - matrix_m2);
                var w0 = 0 - (w.Transpose() * (matrix_m1 + matrix_m2)).Determinant() / 2;

                // Test
                for (int i = start; i < end; i++)
                {
                    for (int j = 0; j < points.GetLength(1); j++)
                    {
                        x[j, 0] = points[i, j];
                    }
                    if (((x.Transpose() * w).Determinant() + w0) *( i - 49.5) < 0)
                    {
                        correct++;
                    }
                    count++;
                }
                
                // Plot
                if (f == 0)
                {
                    for (double i = -20; i < 20; i = i + 0.1)
                    {
                        if (w[c2,0] != 0)
                        {
                            Draw(p, Color.Black, i, -(w0 + i * w[c1, 0]) / w[c2, 0]);
                        }
                    }
                }
            }
            return correct / count;
        }
        public double HKTest(int interval, double[,] points, double delta, int c1, int c2, Panel p)
        {
            var Y = Matrix<double>.Build.Dense(points.GetLength(0) - interval, points.GetLength(1) + 1);
            var A = Matrix<double>.Build.Dense(points.GetLength(1) + 1, 1, 1);
            var B = Matrix<double>.Build.Dense(points.GetLength(0) - interval, 1, 1);
            var E = Matrix<double>.Build.Dense(points.GetLength(0) - interval, 1, -1);

            var start = 0;
            var end = 0;
            var correct = 0;
            var count = 0;
            var number = 0;
            var folds = points.GetLength(0) / interval;
            for (int f = 0; f < folds; f++)
            {
                start = f * interval;
                end = start + interval;
                count = 0;
                for (int i = 0; i < points.GetLength(0); i++)
                {
                    if (i < start || i >= end)
                    {
                        if (i < 50)
                        {
                            Y[count, 0] = 1;
                            for (int j = 0; j < points.GetLength(1); j++)
                            {
                                Y[count, j + 1] = points[i, j];
                            }
                        }
                        else
                        {
                            Y[count, 0] = -1;
                            for (int j = 0; j < points.GetLength(1); j++)
                            {
                                Y[count, j + 1] = -points[i, j];
                            }
                        }
                        count++;
                    }
                }
                while (CheckNegative(E) >= 0)
                {
                    E = Y * A - B;
                    B = B + delta * (E + AbsoluteMatrix(E));
                    A = (Y.Transpose() * Y).Inverse() * Y.Transpose() * B;
                }

                var T = Matrix<double>.Build.Dense(1, points.GetLength(1) + 1, 1);
                for (int i = start; i < end; i++)
                {
                    if (i < 50)
                    {
                        T[0, 0] = 1;
                        for (int j = 0; j < points.GetLength(1); j++)
                        {
                            T[0, j + 1] = points[i, j];
                        }
                    }
                    else
                    {
                        T[0, 0] = -1;
                        for (int j = 0; j < points.GetLength(1); j++)
                        {
                            T[0, j + 1] = -points[i, j];
                        }
                    }
                    if ((T * A).Determinant() > 0)
                        correct++;
                    number++;
                }
                if (f == 0)
                {
                    for (double i = -20; i < 20; i = i + 0.1)
                    {
                        if (A[c2+1, 0] != 0)
                        {
                            Draw(p, Color.Black, i, -(A[0, 0] + i * A[c1 + 1, 0]) / A[c2 + 1, 0]);
                        }
                    }
                }

            }
            return correct/number;
        }
        public Matrix<double> AbsoluteMatrix(Matrix<double> matrix)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                if (matrix[i, 0] < 0)
                    matrix[i, 0] = -matrix[i, 0];
            }
            return matrix;
        }
        public int CheckNegative(Matrix<double> matrix)
        {
            for (int i = 0; i < matrix.RowCount; i++)
            {
                if (matrix[i, 0] < 0)
                    return i;
            }
            return -1;
        }
        public double NNTest(int interval, double[,] points)
        {
            int folds = points.GetLength(0) / interval;
            int start, end;
            int correct = 0;
            int count = 0;
            for (int i = 0; i < folds; i++)
            {
                start = i * interval;
                end = start + interval;
                double[] vector;
                for (int j = start; j < end; j++)
                {
                    vector = new double[points.GetLength(1)];
                    for (int k = 0; k < vector.Length; k++)
                    {
                        vector[k] = points[j, k];
                    }
                    if ((NNSingleTest(start, end, points, vector) - 49.5) * (j - 49.5) > 0)
                    {
                        correct++;
                    }
                    else
                    { }
                    count++;
                }
                
            }
            return (double)correct / count;
        }
        public int NNSingleTest(int start, int end, double[,] points, double[] sample)
        {
            var result = 1;
            var dist = double.MaxValue;
            var temp = 0.0;
            for (int i = 0; i < points.GetLength(0); i++)
            {
                if (i < start || i >= end)
                {
                    for (int j = 0; j < sample.Length; j++)
                    {
                        temp += (points[i, j] - sample[j]) * (points[i, j] - sample[j]);
                    }
                    if (temp < dist)
                    {
                        dist = temp;
                        result = i;
                    }
                    temp = 0.0;
                }
            }
            return result;
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
        public double QuadraticTest(int interval, double[,] points1, double[,] points2)
        {
            int folds = points1.GetLength(0) / interval;
            int start, end;
            int correct = 0;
            int count = 0;
            for (int i = 0; i < folds; i++)
            {
                //estimate
                var estimated_means1 = MLEstimateMeans(points1, interval, i, 0);
                var estimated_means2 = MLEstimateMeans(points2, interval, i, 0);

                var estimated_covariance1 = MLEstimateCovariance(estimated_means1, points1, interval, i);
                var estimated_covariance2 = MLEstimateCovariance(estimated_means2, points2, interval, i);
                //estimated_means1 = BayesEstimateMeans(points1, MatrixToArray(estimated_covariance1), 0);
                //estimated_means2 = BayesEstimateMeans(points2, MatrixToArray(estimated_covariance2), 0);
                //test
                start = i * interval;
                end = start + interval;
                double[] vector;
                for (int j = start; j < end; j++)
                {
                    //for class 1
                    vector = new double[points1.GetLength(1)];
                    for (int k = 0; k < vector.Length; k++)
                    {
                        vector[k] = points1[j, k];
                    }
                    if (1 == QuadraticSingleTest(estimated_covariance1, estimated_covariance2, estimated_means1, estimated_means2, vector))
                    {
                        correct++;
                    }
                    for (int k = 0; k < vector.Length; k++)
                    {
                        vector[k] = points2[j, k];
                    }
                    if (2 == QuadraticSingleTest(estimated_covariance1, estimated_covariance2, estimated_means1, estimated_means2, vector))
                    {
                        correct++;
                    }
                    count += 2;
                }
            }
            return (double)correct/count;
        }
        private int QuadraticSingleTest(Matrix<double> matrix1, Matrix<double> matrix2, double[] means1, double[] means2, double[] vector)
        {
            var matrix_means1 = Matrix<double>.Build.DenseOfColumnArrays(means1);
            var matrix_means2 = Matrix<double>.Build.DenseOfColumnArrays(means2);
            var matrix_vector = Matrix<double>.Build.DenseOfColumnArrays(vector);
            // Calculate A B C
            var A = matrix2.Inverse() - matrix1.Inverse();
            var B = 2 * (matrix_means1.Transpose() * matrix1.Inverse()) - matrix_means2.Transpose() * matrix2.Inverse();
            var C = (matrix_means2.Transpose() * matrix2.Inverse() * matrix_means2).Determinant() - (matrix_means1.Transpose() * matrix1.Inverse() * matrix_means1).Determinant() - 2 * (Math.Log10(matrix1.Determinant() / matrix2.Determinant()));
            var result = (matrix_vector.Transpose() * A * matrix_vector).Determinant() + C;
            for (int i = 0; i < means1.Length; i++)
            {
                result += B[0, i] * matrix_vector[i, 0];
            }
            if (result > 0)
            {
                return 1;
            }
            return 2;
        }
        public double[,] MatrixToArray(Matrix<double> matrix)
        {
            var temp = new double[matrix.RowCount, matrix.ColumnCount];
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    temp[i, j] = matrix[i, j];
                }
            }
            return temp;
        }
        public double[] BayesEstimateMeans(double[,] data, double[,] covariance, int count)
        {
            if (count == 0)
            {
                count = data.GetLength(0);
            }
            var matrix_covariance = Matrix<double>.Build.DenseOfArray(covariance);
            var result = new double[data.GetLength(1)];
            var means_init = Matrix<double>.Build.DenseOfColumnArrays(new double[] { 1, 1, 1, 1 });
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
                result[j] = temp[j, 0];
            }
            return result;
        }
        public Matrix<double> MLEstimateCovariance(double[] means, double[,] data, int interval, int fold_number)
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
                    for (int j = 0; j < means.Length; j++)
                    {
                        transposed_data[j, 0] = data[i, j];
                        vec_data[0, j] = data[i, j];
                    }
                    covariance += (transposed_data - transposed_means) * (vec_data - vec_means);
                }
            }
            int count = data.GetLength(0);
            if (fold_number > -1)
            {
                count -= interval;
            }
            return covariance / count;
        }
        public double[] MLEstimateMeans(double[,] data, int interval, int fold_number, int count)
        {
            if (count == 0)
            {
                count = data.GetLength(0);
            }
            var result = new double[m_features];
            var temp = new double[m_features];
            for (int i = 0; i < count; i++)
            {
                if (i < interval * fold_number || i >= interval * (fold_number + 1))
                {
                    for (int j = 0; j < m_features; j++)
                    {
                        temp[j] += data[i, j];
                    }
                }
            }
            if (fold_number > -1)
            {
                count -= interval;
            }
            for (int j = 0; j < m_features; j++)
            {
                result[j] = temp[j] / count;
            }
            return result;
        }
        public string Tostring(double[] array)
        {
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i] + "\t";
            }
            result = "{ " + result + "}";
            return result;
        }
        public void Draw(Panel p, Color c, double x, double y)
        {
            var g = p.CreateGraphics();
            var pen = new Pen(c);
            //var point = new Point((int)(x * 30 - 50), (int)(y * 30 + 50));
            var point = new Point((int)(x * 2 + 100), (int)(y * 2 + 130));
            var s = new System.Drawing.Size(1, 1);
            var circle = new Rectangle(point, s);
            g.DrawRectangle(pen, circle);
        }
    }
}
