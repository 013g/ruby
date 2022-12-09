using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LogLaplace
{
    public partial class FormMain : Form
    {
        Random rnd = new Random();              // Генаретор случайных чисел
        double sig;
        double b;
        int sgn;
        int N;
        int N_done;
        int f = 0; /* Флаг для определиния какой метод используется: 0 - обратной функции
                                                                     1 - нейман
                                                                     2 - метрополис*/
        String s = "";
        List<Double> R = new List<Double>();    // Массив данных
        double W = 0;
        int k = 0;

        public FormMain()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            label4.Text = "";
        }

        // Функция Распределения
        double F(double x)
        {
            if ((Math.Log(Math.Exp(x)) - sig) > 0)
            {
                sgn = 1;
            }
            if ((Math.Log(Math.Exp(x)) - sig) < 0)
            {
                sgn = -1;
            }
            if ((Math.Log(Math.Exp(x)) - sig) == 0)
            {
                sgn = 0;
            }

            return 0.5 * Math.Abs(1 + sgn * (1 - Math.Exp(-1.0 * Math.Abs(Math.Log(Math.Exp(x)) - sig) / b)));
        }

        // Функция Плотности
        double p(double x)
        {
            return (1 / (2 * b * x)) * Math.Exp(-1.0 * Math.Abs(Math.Log(x) - sig) / b);
        }

        // Метод обратной функции
        double inverseFunction()
        {
            double rn = rnd.NextDouble();
            double x;

            if (rn >= 0.5)
            {
                x = sig + b * Math.Log(2 * rn);
            } else
            {
                x = sig - b * Math.Log(2 - 2 * rn);
            }
            return x;
        }

        // Метод Неймана
        double neumanMethod()
        {
            int a = 0;
            int z = 30;
            while (true)
            {
                double g1 = rnd.NextDouble();
                double g2 = rnd.NextDouble();
                double x = a + (z - a) * 1.0 * g1;
                double y = g2 * W;
                if (p(x) > y) return x;
            }
        }

        // Метод Метрополиса
        private double metropolis(double x0)
        {
            double x = 0, del = 30;
            double a = 0;
            x = x0 + (-1.0 + 2.0 * rnd.NextDouble()) * del;
            if ((x > 0) && (x < del))
            {
                a = p(x) / p(x0);
            }
            else
            {
                a = 0;
            }
            if (a >= 1.0)
            {
                x0 = x;
            }
            else if (rnd.NextDouble() < a) x0 = x;
            return x0;
        }

        // Кнопка Старта
        private void start_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(textBox1.Text, out double S)
                && Double.TryParse(textBox3.Text, out double G) &&
                Int64.TryParse(textBox2.Text, out long K) && S > 0 && G > 0)
            {
                sig = S;                                // Сигма
                b = G;                                  // b
                N = (int)K;                             // Количесство эксп.
                f = comboBox1.SelectedIndex;            // Какой метод
                s = comboBox1.SelectedItem.ToString();
                R.Clear();                              // Очистка массива данных

                W = p(sig);

                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                comboBox1.Enabled = false;
                button4.Enabled = false;

                if (backgroundWorker1.IsBusy != true)
                {
                    backgroundWorker1.RunWorkerAsync();
                }

            }
            else
            {
                MessageBox.Show("Невірний формат даних", "Помилка вводу");
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            R.Sort();
            FormResult form2 = new FormResult();          // Создание формы с результатми
            // Функция Распределения
            Series series = form2.chart1.Series.Add("F(x) - На практиці");
            Series series1 = form2.chart1.Series.Add("F(x) - В теорії");

            series.ChartType = SeriesChartType.Line;
            series1.ChartType = SeriesChartType.Line;

            form2.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{0:f3}";
            form2.chart1.ChartAreas[0].AxisX.Interval = Math.Ceiling(R[N_done - 1]) / 5.0;

            // Функция Плотности
            Series series2 = form2.chart2.Series.Add("p(x) - На практиці");

            double[] pdf = new double[(int)Math.Ceiling(R[N_done - 1])];
            for (int i = 0; i < pdf.Length; i++)
            {
                pdf[i] = 0;
            }
            for (int i = 0; i < N_done; i++)
            {
                for (int j = 0; j < pdf.Length; j++)
                {
                    if (R[i] > j && R[i] <= (j + 1)) pdf[j]++;
                }
            }

            for (int i = 0; i < pdf.Length; i++)
            {
                series2.Points.AddXY(i, pdf[i] / (N_done * 1.0));
            }
        

            Series series3 = form2.chart2.Series.Add("p(x) - В теорії");


            series3.ChartType = SeriesChartType.Line;

            form2.chart2.ChartAreas[0].AxisX.LabelStyle.Format = "{0:f3}";
            form2.chart2.ChartAreas[0].AxisX.Interval = Math.Ceiling(R[N_done - 1]) / 5.0;

            form2.chart1.Series[0].BorderWidth = 4;
            form2.chart1.Series[1].BorderWidth = 4;
            form2.chart2.Series[1].BorderWidth = 4;

            for (int i = 0; i < N_done; i++)
            {
                series.Points.AddXY(R[i], F(R[i]) * 1.02);
                series1.Points.AddXY(R[i], F(R[i]));
                series3.Points.AddXY(R[i], p(R[i]));
                
            }
            form2.label1.Text += " Метод " + s;
            form2.label2.Text += " Метод " + s;

            // Мат.ожидание. теор
            double Et = sig;
            form2.label7.Text = Et.ToString();

            // Мат. ожидание. практ
            double E = R.Sum() / (1.0 * N_done);
            form2.label12.Text = Math.Round(E, 3).ToString();

            // Мат. ожидание. ошибка
            form2.label16.Text = Math.Round(Math.Abs(E - Et), 3).ToString();

            // Дисперсия теор
            double Dt = 2 * b * b;
            form2.label8.Text = Dt.ToString();

            // Дисперсия практ
            double s2 = 0;
            for (int i = 0; i < N_done; i++)
            {
                s2 += R[i] * R[i];
            }
            s2 /= N_done * 1.0;
            double D = Math.Round(s2 - E * E, 3);
            form2.label11.Text = D.ToString();

            // Дисперсия ошибка
            form2.label15.Text = Math.Round(Math.Abs(D - Dt), 3).ToString();

            // Мода теория
            double Mod = sig;
            form2.label9.Text = Mod.ToString();

            // Мода практика
            int m = 0;
            double max = 0;
            for (int i = 0; i < pdf.Length; i++)
            {
                if (pdf[i] > max)
                {
                    max = pdf[i];
                    m = i;
                }
            }
            double Mp = m;
            form2.label10.Text = Mp.ToString();

            // Мода ошибка
            form2.label14.Text = Math.Round(Math.Abs(Mp - Mod), 3).ToString();

            form2.Text += " N = " + N_done.ToString() + " Mu = " + sig.ToString() + " b = " + b.ToString();
            form2.Show();

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            comboBox1.Enabled = true;
            button4.Enabled = true;
            k++;
        }

        // Создание progress bar
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label4.Text = e.ProgressPercentage.ToString() + "%";
        }

        // Кнопка Отмены
        private void cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            comboBox1.Enabled = true;
        }

        // Инициализация BackgroundWorker
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            double x0 = 0.555;
            for (int j = 0; j < 20; j++)
            {
                x0 = metropolis(x0);
            }

            for (int i = 1; i <= N; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (f == 0)
                    {
                        R.Add(inverseFunction());
                    }
                    else if (f == 1)
                    {
                        R.Add(neumanMethod());
                    }
                    else if (f == 2)
                    {
                        x0 = metropolis(x0);
                        R.Add(x0);
                    }

                    N_done = i;
                    if (i % 100 == 0)
                    {
                        System.Threading.Thread.Sleep(1);
                        worker.ReportProgress((int)(i * 100.0 / (1.0 * N)));
                    }
                }
            }
        }

        // Кнопка сохранения в файл
        private void save_Click(object sender, EventArgs e)
        {
            String fullPath = "Result" + k.ToString() + ".txt";
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < N_done; i++)
            {
                String str = R[i].ToString();
                strBuilder.AppendLine(str);
            }
            File.WriteAllText(fullPath, strBuilder.ToString());
            MessageBox.Show("Збережено за шляхом" + fullPath);
        }

        // Кнопка получения информации о распределении
        private void info_Click(object sender, EventArgs e)
        {
            FormInfo form3 = new FormInfo();
            form3.Show();
        }
    }
}
