using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MainForm
{
    /// <summary>
    /// Нечеткое число
    /// </summary>
    class FuzzyNumberLR
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        /// <summary>
        /// Пара ключ-значение
        /// </summary>
        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FuzzyNumberLR()
        {
            set = new List<Point>();
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(Point p)
        {
            if (p.y > 1 || p.y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == p.x)
                {
                    // если повтор
                    if (set[i].y == p.y)
                        return;
                }
                else
                {
                    if (set[i].x > p.x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(p.x, p.y));
            else
                set.Insert(index, new Point(p.x, p.y));
        }

        /// <summary>
        /// Получить значение по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Point Get(int i)
        {
            return set[i];
        }

        /// <summary>
        /// Получить число точек во множестве
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return set.Count;
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Spline;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }

        /// <summary>
        /// Унимодальность
        /// </summary>
        /// <returns></returns>
        private bool Unimodality()
        {
            int countCore = 0;

            for (int i = 0; i < set.Count; i++)
                if (set[i].y == 1)
                    countCore++;

            if (countCore == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Выпуклость
        /// </summary>
        /// <returns></returns>
        private bool Bulge()
        {
            double max = 0;
            bool flagFall = false;

            if (set.Count != 0)
                max = set[0].y;

            for (int i = 1; i < set.Count; i++)
            {
                if (set[i].y >= max)
                {
                    if (flagFall)
                        return false;

                    max = set[i].y;
                }
                else
                {
                    max = set[i].y;
                    flagFall = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка, является ли множество нечетким числом
        /// </summary>
        /// <returns></returns>
        public bool CheckFuzzyNumber()
        {
            return this.Bulge() && this.Unimodality();
        }

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyNumberLR Amount(FuzzyNumberLR a, FuzzyNumberLR b)
        {
            FuzzyNumberLR r = new FuzzyNumberLR();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x + b.set[i].x);

            return r;
        }

        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyNumberLR Difference(FuzzyNumberLR a, FuzzyNumberLR b)
        {
            FuzzyNumberLR r = new FuzzyNumberLR();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x - b.set[i].x);

            return r;
        }
    }

    /// <summary>
    /// Нечеткий интервал
    /// </summary>
    class FuzzyIntervalLR
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        /// <summary>
        /// Пара ключ-значение
        /// </summary>
        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FuzzyIntervalLR()
        {
            set = new List<Point>();
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(Point p)
        {
            if (p.y > 1 || p.y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == p.x)
                {
                    // если повтор
                    if (set[i].y == p.y)
                        return;
                }
                else
                {
                    if (set[i].x > p.x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(p.x, p.y));
            else
                set.Insert(index, new Point(p.x, p.y));
        }

        /// <summary>
        /// Получить значение по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Point Get(int i)
        {
            return set[i];
        }

        /// <summary>
        /// Получить число точек во множестве
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return set.Count;
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Spline;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }

        /// <summary>
        /// Выпуклость
        /// </summary>
        /// <returns></returns>
        private bool Bulge()
        {
            double max = 0;
            bool flagFall = false;

            if (set.Count != 0)
                max = set[0].y;

            for (int i = 1; i < set.Count; i++)
            {
                if (set[i].y >= max)
                {
                    if (flagFall)
                        return false;

                    max = set[i].y;
                }
                else
                {
                    max = set[i].y;
                    flagFall = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка, является ли множество нечетким интервалом
        /// </summary>
        /// <returns></returns>
        public bool CheckFuzzyInterval()
        {
            return this.Bulge();
        }

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyIntervalLR Amount(FuzzyIntervalLR a, FuzzyIntervalLR b)
        {
            FuzzyIntervalLR r = new FuzzyIntervalLR();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x + b.set[i].x);

            return r;
        }

        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyIntervalLR Difference(FuzzyIntervalLR a, FuzzyIntervalLR b)
        {
            FuzzyIntervalLR r = new FuzzyIntervalLR();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x - b.set[i].x);

            return r;
        }
    }

    /// <summary>
    /// Нечеткое число
    /// </summary>
    class FuzzyNumber
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        /// <summary>
        /// Пара ключ-значение
        /// </summary>
        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FuzzyNumber()
        {
            set = new List<Point>();
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(Point p)
        {
            if (p.y > 1 || p.y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == p.x)
                {
                    // если повтор
                    if (set[i].y == p.y)
                        return;
                }
                else
                {
                    if (set[i].x > p.x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(p.x, p.y));
            else
                set.Insert(index, new Point(p.x, p.y));
        }

        /// <summary>
        /// Получить значение по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Point Get(int i)
        {
            return set[i];
        }

        /// <summary>
        /// Получить число точек во множестве
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return set.Count;
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Line;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }

        /// <summary>
        /// Унимодальность
        /// </summary>
        /// <returns></returns>
        private bool Unimodality()
        {
            int countCore = 0;

            for (int i = 0; i < set.Count; i++)
                if (set[i].y == 1)
                    countCore++;

            if (countCore == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Выпуклость
        /// </summary>
        /// <returns></returns>
        private bool Bulge()
        {
            double max = 0;
            bool flagFall = false;

            if (set.Count != 0)
                max = set[0].y;

            for (int i = 1; i < set.Count; i++)
            {
                if (set[i].y >= max)
                {
                    if (flagFall)
                        return false;

                    max = set[i].y;
                }
                else
                {
                    max = set[i].y;
                    flagFall = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка, является ли множество нечетким числом
        /// </summary>
        /// <returns></returns>
        public bool CheckFuzzyNumber()
        {
            return this.Bulge() && this.Unimodality();
        }

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyNumber Amount(FuzzyNumber a, FuzzyNumber b)
        {
            FuzzyNumber r = new FuzzyNumber();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x + b.set[i].x);

            return r;
        }

        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyNumber Difference(FuzzyNumber a, FuzzyNumber b)
        {
            FuzzyNumber r = new FuzzyNumber();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x - b.set[i].x);

            return r;
        }
    }

    /// <summary>
    /// Нечеткий интервал
    /// </summary>
    class FuzzyInterval
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        /// <summary>
        /// Пара ключ-значение
        /// </summary>
        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FuzzyInterval()
        {
            set = new List<Point>();
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(Point p)
        {
            if (p.y > 1 || p.y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == p.x)
                {
                    // если повтор
                    if (set[i].y == p.y)
                        return;
                }
                else
                {
                    if (set[i].x > p.x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(p.x, p.y));
            else
                set.Insert(index, new Point(p.x, p.y));
        }

        /// <summary>
        /// Получить значение по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Point Get(int i)
        {
            return set[i];
        }

        /// <summary>
        /// Получить число точек во множестве
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return set.Count;
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Line;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }

        /// <summary>
        /// Выпуклость
        /// </summary>
        /// <returns></returns>
        private bool Bulge()
        {
            double max = 0;
            bool flagFall = false;

            if (set.Count != 0)
                max = set[0].y;

            for (int i = 1; i < set.Count; i++)
            {
                if (set[i].y >= max)
                {
                    if (flagFall)
                        return false;

                    max = set[i].y;
                }
                else
                {
                    max = set[i].y;
                    flagFall = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка, является ли множество нечетким интервалом
        /// </summary>
        /// <returns></returns>
        public bool CheckFuzzyInterval()
        {
            return this.Bulge();
        }

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyInterval Amount(FuzzyInterval a, FuzzyInterval b)
        {
            FuzzyInterval r = new FuzzyInterval();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x + b.set[i].x);

            return r;
        }

        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="a"></param>
        public static FuzzyInterval Difference(FuzzyInterval a, FuzzyInterval b)
        {
            FuzzyInterval r = new FuzzyInterval();

            for (int i = 0; i < Math.Min(a.set.Count, b.set.Count); i++)
                r.Add(b.set[i].y, a.set[i].x - b.set[i].x);

            return r;
        }
    }

    /// <summary>
    /// Треугольное нечеткое число
    /// </summary>
    class TriangularFuzzyNumber
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        private void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public TriangularFuzzyNumber(double a, double b, double c)
        {
            set = new List<Point>();

            this.Add(0.0, a);
            this.Add(1.0, b);
            this.Add(0.0, c);

            if (b <= a || c <= b)
                throw new Exception("Ошибка! Не является нечетким интервалом.");
        }

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TriangularFuzzyNumber Amount(TriangularFuzzyNumber a, TriangularFuzzyNumber b)
        {
            return new TriangularFuzzyNumber(
                a.set[0].x + b.set[0].x,
                a.set[1].x + b.set[1].x,
                a.set[2].x + b.set[2].x
                );
        }

        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TriangularFuzzyNumber Difference(TriangularFuzzyNumber a, TriangularFuzzyNumber b)
        {
            return new TriangularFuzzyNumber(
                a.set[0].x - b.set[0].x,
                a.set[1].x - b.set[1].x,
                a.set[2].x - b.set[2].x
                );
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Line;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }
    }

    /// <summary>
    /// Трапецивидный нечеткий интервал
    /// </summary>
    class TrapezoidalFuzzyInterval
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        private void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="a"> </param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public TrapezoidalFuzzyInterval(double a, double b, double c, double d)
        {
            set = new List<Point>();

            this.Add(0.0, a);
            this.Add(1.0, b);
            this.Add(1.0, c);
            this.Add(0.0, d);

            if (b<=a || c<=b || d<=c)
                throw new Exception("Ошибка! Не является нечетким интервалом.");
        }    

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TrapezoidalFuzzyInterval Amount(TrapezoidalFuzzyInterval a, TrapezoidalFuzzyInterval b)
        {
            return new TrapezoidalFuzzyInterval(
                a.set[0].x + b.set[0].x,
                a.set[1].x + b.set[1].x,
                a.set[2].x + b.set[2].x,
                a.set[3].x + b.set[3].x
                );
        }

        /// <summary>
        /// Разность
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static TrapezoidalFuzzyInterval Difference(TrapezoidalFuzzyInterval a, TrapezoidalFuzzyInterval b)
        {
            return new TrapezoidalFuzzyInterval(
                a.set[0].x - b.set[0].x,
                a.set[1].x - b.set[1].x,
                a.set[2].x - b.set[2].x,
                a.set[3].x - b.set[3].x
                );
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Line;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }
    }

    /// <summary>
    /// Нечеткое множество
    /// </summary>
    class FuzzySet
    {
        /// <summary>
        /// Множество
        /// </summary>
        private List<Point> set;

        /// <summary>
        /// Пара ключ-значение
        /// </summary>
        public class Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FuzzySet()
        {
            set = new List<Point>();
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(double y, double x)
        {
            if (y > 1 || y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == x)
                {
                    // если повтор
                    if (set[i].y == y)
                        return;
                }
                else
                {
                    if (set[i].x > x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(x, y));
            else
                set.Insert(index, new Point(x, y));
        }

        /// <summary>
        /// Добавляет элемент в множество
        /// </summary>
        /// <param name="name">Значение по y</param>
        /// <param name="value">Значение по x</param>
        public void Add(Point p)
        {
            if (p.y > 1 || p.y < 0)
                throw new Exception("Значение y должно быть в пределах от 0 до 1!");

            int index = -1;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i].x == p.x)
                {
                    // если повтор
                    if (set[i].y == p.y)
                        return;
                }
                else
                {
                    if (set[i].x > p.x)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index == -1)
                set.Add(new Point(p.x, p.y));
            else
                set.Insert(index, new Point(p.x, p.y));
        }

        /// <summary>
        /// Получить значение по индексу
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Point Get(int i)
        {
            return set[i];
        }

        /// <summary>
        /// Получить число точек во множестве
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return set.Count;
        }

        /// <summary>
        /// Отображает множество на графе (функция принадлежности)
        /// </summary>
        /// <param name="graph"></param>
        public void Draw(Chart graph)
        {
            if (set.Count == 0)
                return;

            graph.Series.Clear();

            Series s = new Series();
            s.ChartType = SeriesChartType.Line;
            s.BorderWidth = 5;

            s.MarkerBorderColor = System.Drawing.Color.Black;
            s.MarkerBorderWidth = 2;
            s.MarkerStyle = MarkerStyle.Circle;
            s.MarkerSize = 8;
            s.MarkerColor = System.Drawing.Color.White;

            Axis ax = new Axis();
            Axis ay = new Axis();

            ax.LineWidth = 2;
            ay.LineWidth = 2;
            ax.Crossing = 0;
            ay.Crossing = 0;


            s.Points.AddXY(set[0].x - 1, set[0].y == 0 ? 0.02 : set[0].y);
            s.Points[0].MarkerStyle = MarkerStyle.None;

            for (int i = 0; i < set.Count; i++)
            {
                s.Points.AddXY(set[i].x, set[i].y == 0 ? 0.02 : set[i].y);
                ax.CustomLabels.Add(set[i].x - 0.85, set[i].x + 1.0, Math.Round(set[i].x, 2).ToString());
                ay.CustomLabels.Add(set[i].y - 0.9, set[i].y + 1.0, Math.Round(set[i].y, 2).ToString());
            }

            s.Points.AddXY(set[set.Count - 1].x + 1, set[set.Count - 1].y == 0 ? 0.02 : set[set.Count - 1].y);
            s.Points[s.Points.Count - 1].MarkerStyle = MarkerStyle.None;

            graph.ChartAreas[0].AxisX = ax;
            graph.ChartAreas[0].AxisY = ay;
            graph.ChartAreas[0].AxisY.Maximum = 1.25;
            graph.ChartAreas[0].AxisY.Minimum = -0.25;
            graph.ChartAreas[0].AxisX.Minimum = set[0].x - 1;
            graph.ChartAreas[0].AxisX.Maximum = set[set.Count - 1].x + 1;
            graph.ChartAreas[0].AxisX.Interval = (Math.Abs(set[0].x - 1) + Math.Abs(set[set.Count - 1].x + 1)) / 15;
            graph.ChartAreas[0].AxisY.Interval = 0.25;

            graph.Series.Add(s);
        }

        /////Дополнительные функции/////

        /// <summary>
        /// Находит точку пересечения двух отрезков
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static Point SegmentsIntersection(Point a1, Point a2, Point b1, Point b2)
        {
            double dx12 = a2.x - a1.x;
            double dy12 = a2.y - a1.y;
            double dx34 = b2.x - b1.x;
            double dy34 = b2.y - b1.y;

            double denominator = (dy12 * dx34 - dx12 * dy34);

            if (denominator == 0)
                return null;
            else
            {
                double t1 = ((a1.x - b1.x) * dy34 + (b1.y - a1.y) * dx34) / denominator;
                double t2 = ((b1.x - a1.x) * dy12 + (a1.y - b1.y) * dx12) / -denominator;

                if ((t1 >= 0.0) && (t1 <= 1.0) && (t2 >= 0.0) && (t2 <= 1.0))
                    return new Point(a1.x + dx12 * t1, a1.y + dy12 * t1);
                else
                    return null;
            }
        }

        /// <summary>
        /// Дистанция между двумя точками (без использования корней)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double Distance(Point a, Point b)
        {
            return Math.Abs((a.x - b.x) + (a.y - b.y));
        }

        /// <summary>
        /// Проверяет вхождение точки в треугольник
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool CheckPointInside(Point a, Point b, Point c, Point p)
        {
            double ab = (a.x - p.x) * (b.y - a.y) - (b.x - a.x) * (a.y - p.y);
            double bc = (b.x - p.x) * (c.y - b.y) - (c.x - b.x) * (b.y - p.y);
            double ca = (c.x - p.x) * (a.y - c.y) - (a.x - c.x) * (c.y - p.y);

            // вершины лежат на одной прямой (коллинеарность)
            if (ab == 0 && bc == 0 && ca == 0)
            {
                double ab1 = Distance(a, b);
                double bc1 = Distance(b, c);
                double ca1 = Distance(c, a);

                if (ab1 >= bc1 && ab1 >= ca1)
                    if (Distance(a, p) + Distance(b, p) == ab1)
                        return true;
                    else if (bc1 >= ab1 && bc1 >= ca1)
                        if (Distance(b, p) + Distance(c, p) == bc1)
                            return true;
                        else if (ca1 >= ab1 && ca1 >= bc1)
                            if (Distance(c, p) + Distance(a, p) == ca1)
                                return true;

                return false;
            }
            else
                return (ab >= 0 && bc >= 0 && ca >= 0) || (ab <= 0 && bc <= 0 && ca <= 0);
        }

        /// <summary>
        /// Проверяет вхождение вершин одного множества в другое
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="r"></param>
        /// <param name="flagSide">true - точка внутри, false - точка снаружи</param>
        private static void CkeckSetInside(FuzzySet a, FuzzySet b, FuzzySet r, bool flagSide)
        {
            // список треугольников (будем делить множество на треугольники)
            List<Point> triangles = new List<Point>();
            // количество вхождений
            int count = 0;

            // если y первого элемента не равен 0,
            // то ставим ограничитель
            if (a.set[0].y != 0)
            {
                double minX = Math.Min(a.set[0].x, b.set[0].x);
                triangles.Add(new Point(minX, a.set[0].y));
                triangles.Add(new Point(minX, 0));
            }

            // делим множество на треугольники,
            // если y == 0, то добавляем дополниетльную точку 
            // на лежащую на оси x
            for (int i = 0; i < a.set.Count; i++)
            {
                triangles.Add(a.set[i]);

                if (a.set[i].y != 0)
                    triangles.Add(new Point(a.set[i].x, 0));
            }

            // если y последнего элемента не равен 0,
            // то ставим ограничитель
            if (a.set[a.set.Count - 1].y != 0)
            {
                double maxX = Math.Max(a.set[a.set.Count - 1].x, b.set[b.set.Count - 1].x);
                triangles.Add(new Point(maxX, a.set[a.set.Count - 1].y));
                triangles.Add(new Point(maxX, 0));
            }

            // перебираем вершины множества и проверяем 
            // их на условие вхождения в треугольники 
            // созданные ранее
            for (int i = 0; i < b.set.Count; i++)
            {
                for (int j = 0; j < triangles.Count - 2; j++)
                {
                    if (CheckPointInside(triangles[j], triangles[j + 1], triangles[j + 2], b.set[i]))
                        count++;
                }
                if (flagSide)
                {
                    if (count > 0)
                        r.Add(b.set[i]);
                }
                else
                {
                    if (count == 0)
                        r.Add(b.set[i]);
                }
                count = 0;
            }
        }

        /// <summary>
        /// Копирует значения одного множества в другое
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        private static void CopySet(FuzzySet a, FuzzySet r)
        {
            for (int i = 0; i < a.set.Count; i++)
                r.Add(a.set[i]);
        }

        /// <summary>
        /// Проверяет множества на пустоту, если не пустое хоть одно, то вернет его
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static bool CheckEmptySet(FuzzySet a, FuzzySet b, FuzzySet r)
        {
            if (a.set.Count == 0)
            {
                if (b.set.Count != 0)
                    CopySet(b, r);
                return true;
            }

            else if (b.set.Count == 0)
            {
                if (a.set.Count != 0)
                    CopySet(a, r);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет пересечение отрезков двух множеств
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="r"></param>
        private static void CheckSetIntersection(FuzzySet a, FuzzySet b, FuzzySet r)
        {
            Point p; // точка пересечения

            List<Point> pointsA = new List<Point>();
            List<Point> pointsB = new List<Point>();
            double minX = Math.Min(a.set[0].x, b.set[0].x);
            double maxX = Math.Max(a.set[a.set.Count - 1].x, b.set[b.set.Count - 1].x);

            // если у первого или последного элемента 'y'==0, то
            // вставляем мнимальный и максимальный x
            // + переписываем все в новые списки чтобы не трогать списки a и b

            if (a.set[0].y != 0)
                pointsA.Add(new Point(minX, a.set[0].y));

            if (b.set[0].y != 0)
                pointsB.Add(new Point(minX, b.set[0].y));

            for (int i = 0; i < a.set.Count; i++)
                pointsA.Add(a.set[i]);

            for (int i = 0; i < b.set.Count; i++)
                pointsB.Add(b.set[i]);

            if (a.set[a.set.Count - 1].y != 0)
                pointsA.Add(new Point(maxX, a.set[a.set.Count - 1].y));

            if (b.set[b.set.Count - 1].y != 0)
                pointsB.Add(new Point(maxX, b.set[b.set.Count - 1].y));

            //перебираем все отрезки множества a на наличие пересечений со множеством b
            for (int i = 0; i < pointsA.Count - 1; i++)
                for (int j = 0; j < pointsB.Count - 1; j++)
                {
                    p = SegmentsIntersection(pointsA[i], pointsA[i + 1], pointsB[j], pointsB[j + 1]);

                    // если было пересечение, то добавляем в результат
                    if (p != null)
                        r.Add(p);
                }
        }

        /// <summary>
        /// Проверяет пересечение элемента x со множеством
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static void CheckPointIntersection(FuzzySet a, double x, FuzzySet r)
        {
            if (a.set.Count == 0)
                throw new Exception("Множество не должно быть пустым!");

            FuzzySet b = new FuzzySet();
            b.Add(0.0, x);
            b.Add(1.0, x);

            Point p = null;
            List<Point> pointsA = new List<Point>();

            double minX = Math.Min(a.set[0].x, x);
            double maxX = Math.Max(a.set[a.set.Count - 1].x, x);

            // если у первого или последного элемента 'y'==0, то
            // вставляем мнимальный и максимальный x
            // если x  первого или последного элемента 'x'==0, то
            // вставляем мнимальный и максимальный x с параметром 'y'==0
            // + переписываем все в новые списки чтобы не трогать список a
            if (a.set[0].y != 0)
                pointsA.Add(new Point(minX, a.set[0].y));
            else
            {
                if (x < a.set[0].x)
                    pointsA.Add(new Point(minX, 0));
            }

            for (int i = 0; i < a.set.Count; i++)
                pointsA.Add(a.set[i]);

            if (a.set[a.set.Count - 1].y != 0)
                pointsA.Add(new Point(maxX, a.set[a.set.Count - 1].y));
            else
            {
                if (x > a.set[a.set.Count - 1].x)
                    pointsA.Add(new Point(maxX, 0));
            }

            //перебираем все отрезки множества a на наличие пересечений со множеством b
            for (int i = 0; i < pointsA.Count - 1; i++)
            {
                p = SegmentsIntersection(pointsA[i], pointsA[i + 1], b.set[0], b.set[1]);

                if (p != null)
                {
                    r.Add(p);
                    break;
                }
            }
        }

        /////Операции над НМ/////

        //Включение
        //Равенство
        //Дополнение
        //Пересечение
        //Объединение
        //Разность
        //Дезъюнктивная сумма

        /// <summary>
        /// Включение ⊆
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Including(FuzzySet a, FuzzySet b)
        {
            FuzzySet r = new FuzzySet();

            if (CheckEmptySet(a, b, r))
                return true; //? или false

            CkeckSetInside(b, a, r, true);

            if (r.set.Count == a.set.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Равенство =
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equality(FuzzySet a, FuzzySet b)
        {
            if (a.set.Count != b.set.Count)
                return false;

            for (int i = 0; i < a.set.Count; i++)
                if (a.set[i].x != b.set[i].x || a.set[i].y != b.set[i].y)
                    return false;

            return true;
        }

        /// <summary>
        /// Дополнение ! или -
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static FuzzySet Extension(FuzzySet a)
        {
            FuzzySet r = new FuzzySet();

            if (a.set.Count == 0)
                return r;

            for (int i = 0; i < a.set.Count; i++)
                r.Add(new Point(a.set[i].x, 1.0 - a.set[i].y));

            return r;
        }

        /// <summary>
        /// Пересечение ∩
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FuzzySet Intersection(FuzzySet a, FuzzySet b)
        {
            FuzzySet r = new FuzzySet();

            if (CheckEmptySet(a, b, r))
                return r;

            CheckSetIntersection(a, b, r);
            CkeckSetInside(a, b, r, true);
            CkeckSetInside(b, a, r, true);

            return r;
        }

        /// <summary>
        /// Объединение ∪ 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FuzzySet Unification(FuzzySet a, FuzzySet b)
        {
            FuzzySet r = new FuzzySet();

            if (CheckEmptySet(a, b, r))
                return r;

            // костыль
            // добавляем в результат совпадающие точки множеств
            // т.к. CheckSetIntersection не вернет совпадающие точки
            // + CkeckSetInside с параметром false не включает точки
            // лежащие на ребре множества
            for (int i = 0; i < a.set.Count; i++)
                for (int j = 0; j < b.set.Count; j++)
                    if (a.set[i].x == b.set[j].x && a.set[i].y == b.set[j].y)
                        r.Add(a.set[i]);

            CheckSetIntersection(a, b, r);
            CkeckSetInside(a, b, r, false);
            CkeckSetInside(b, a, r, false);

            return r;
        }

        /// <summary>
        /// Разность -
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FuzzySet Difference(FuzzySet a, FuzzySet b)
        {
            FuzzySet r = new FuzzySet();

            if (CheckEmptySet(a, b, r))
                return r;

            r = Intersection(a, Extension(b));

            return r;
        }

        /// <summary>
        /// Дизъюнктивная сумма ⊕
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FuzzySet DisjunctiveSum(FuzzySet a, FuzzySet b)
        {
            FuzzySet r = new FuzzySet();

            if (CheckEmptySet(a, b, r))
                return r;

            r = Unification(Difference(a, b), Difference(b, a));

            return r;
        }

        /// <summary>
        /// Построение НМ по условиям нечеткости (более нечеткое)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static FuzzySet GetMoreFuzzySet(FuzzySet a)
        {
            double[] b = new double[a.set.Count];

            FuzzySet r = new FuzzySet();
            double q = 0;

            for(int i=0;i<a.set.Count;i++)
            {
                q = Math.Abs(a.set[i].y - 0.5);

                if (q == 0)
                    r.Add(0.5, a.set[i].x);
                else
                    r.Add(Math.Abs(q + 0.5)-Math.Abs(q-0.5),a.set[i].x);
            }

            return r;
        }

        /// <summary>
        /// Сравнение нечеткости множеств 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static FuzzySet ComparisonFuzzySets(FuzzySet a, FuzzySet b)
        {
            FuzzySet a0 = new FuzzySet();
            for (int i = 0; i < a.set.Count; i++)
                if (a.set[i].y > 0.5)
                    a0.Add(1,a.set[i].x);

            FuzzySet b0 = new FuzzySet();
            for (int i = 0; i < b.set.Count; i++)
                if (b.set[i].y > 0.5)
                    b0.Add(1, b.set[i].x);

            double da = FuzzySet.Distance(a, a0);
            double db = FuzzySet.Distance(b, b0);

            // если носсители равны, то корректно иначе нет
            if(a.Carrier().set.Count== b.Carrier().set.Count)
            {
                if (da > db)
                    return a;
                else
                    return b;
            }
            else
                return null;
        }

        /// <summary>
        /// Расстояние по Хеммингу
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double Distance(FuzzySet a, FuzzySet b)
        {
            double distance = 0;
            int index = -1;
            bool[] flags = new bool[b.set.Count];

            for (int i = 0; i < a.set.Count; i++)
            {
                index = -1;
                for (int j = 0; j < b.set.Count; j++)
                {
                    if (a.set[i].x == b.set[j].x)
                    {
                        index = j;
                        flags[j] = true;
                        break;
                    }
                }

                if (index == -1)
                    distance += a.set[i].y;
                else
                    distance += Math.Abs(a.set[i].y - b.set[index].y);
            }

            for (int i = 0; i < b.set.Count; i++)
            {
                if (flags[i] == false)
                {
                    distance += b.set[i].y;
                }
            }

            return distance;
        }

        /// <summary>
        /// Ядро
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public FuzzySet Core()
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < set.Count; i++)
                if (set[i].y == 1)
                    r.Add(set[i]);

            return r;
        }

        /// <summary>
        /// Носитель
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public FuzzySet Carrier()
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < set.Count; i++)
                if (set[i].y > 0)
                    r.Add(set[i]);

            return r;
        }

        /// <summary>
        /// a - разрез
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public FuzzySet aLevel(double a)
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < set.Count; i++)
                if (set[i].y >= a)
                    r.Add(set[i]);

            return r;
        }

        /// <summary>
        /// Алгебраическая сумма
        /// </summary>
        /// <param name="a"></param>
        public FuzzySet Amount(FuzzySet a)
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < Math.Min(a.set.Count, set.Count); i++)
                r.Add(set[i].y + a.set[i].y - set[i].y * a.set[i].y, set[i].x);

            return r;
        }

        /// <summary>
        /// Алгебраическое произведение
        /// </summary>
        /// <param name="a"></param>
        public FuzzySet Multiply(FuzzySet a)
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < Math.Min(a.set.Count, set.Count); i++)
                r.Add(set[i].y * a.set[i].y, set[i].x);

            return r;
        }

        /// <summary>
        /// Концентрация
        /// </summary>
        /// <param name="a"></param>
        public FuzzySet Concentration()
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < set.Count; i++)
                r.Add(set[i].y * set[i].y, set[i].x);

            return r;
        }

        /// <summary>
        /// Растяжение
        /// </summary>
        /// <param name="a"></param>
        public FuzzySet Stretching()
        {
            FuzzySet r = new FuzzySet();

            for (int i = 0; i < set.Count; i++)
                r.Add(Math.Sqrt(set[i].y), set[i].x);

            return r;
        }



        /////Определения операций/////


        public enum Type
        {
            Unification,
            Intersection
        }

        public static double Extension(double a)
        {
            if (a > 1 || a < 0)
                throw new Exception("Значение a должно быть в пределах от 0 до 1!");

            return 1.0 - a;
        }

        public static double Maximum(double a, double b, Type type)
        {
            if (a > 1 || a < 0)
                throw new Exception("Значение a должно быть в пределах от 0 до 1!");

            if (b > 1 || b < 0)
                throw new Exception("Значение b должно быть в пределах от 0 до 1!");

            double r = 0;

            switch (type)
            {
                case Type.Intersection:
                    r = Math.Max(a, b);
                    break;
                case Type.Unification:
                    r = Math.Min(a, b);
                    break;
            }

            return r;
        }

        public static double Algebraic(double a, double b, Type type)
        {
            if (a > 1 || a < 0)
                throw new Exception("Значение a должно быть в пределах от 0 до 1!");

            if (b > 1 || b < 0)
                throw new Exception("Значение b должно быть в пределах от 0 до 1!");

            double r = 0;

            switch (type)
            {
                case Type.Intersection:
                    r = a * b;
                    break;
                case Type.Unification:
                    r = a + b - a * b;
                    break;
            }

            return r;
        }

        public static double Limited(double a, double b, Type type)
        {
            if (a > 1 || a < 0)
                throw new Exception("Значение a должно быть в пределах от 0 до 1!");

            if (b > 1 || b < 0)
                throw new Exception("Значение b должно быть в пределах от 0 до 1!");

            double r = 0;

            switch (type)
            {
                case Type.Intersection:
                    r = Math.Max(0.0, a + b - 1.0);
                    break;
                case Type.Unification:
                    r = Math.Min(1.0, a + b);
                    break;
            }

            return r;
        }

        /// <summary>
        /// Степень принадлежности элемента множеству
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double CheckElement(FuzzySet a, double x)
        {
            FuzzySet r = new FuzzySet();

            CheckPointIntersection(a, x, r);

            if (r.set.Count == 0)
                throw new Exception("Ошибка, не удалось найти пересечение!");

            return r.set[0].y;
        }
    }

    /// <summary>
    /// Лингвистическая переменная
    /// </summary>
    class LinguisticVariable
    {
        public string name;
        public double min;
        public double max;
        public Dictionary<string, FuzzySet> sets;

        public enum qualifiers
        {
            moreOrLess, //более менее
            very,       //очень
            not         //не
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="names"></param>
        public LinguisticVariable(string name, double min, double max, string[] names)
        {
            this.name = name;
            this.min = min;
            this.max = max;
            this.sets = new Dictionary<string, FuzzySet>();

            for (int i = 0; i < names.Length; i++)
                sets.Add(names[i], new FuzzySet());
        }

        /// <summary>
        /// Добавляет новую точку в выбранное множество
        /// </summary>
        /// <param name="name"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public void Add(string name, double y, double x)
        {
            if (x < min || x > max)
                throw new Exception("Выход за границы лингвистической переменной");

            sets[name].Add(y, x);
        }

        /// <summary>
        /// Применяет квалификатор
        /// </summary>
        /// <param name="index">Номер элемента в НМ</param>
        /// <param name="q">Квалификатор</param>
        public static FuzzySet UseQualifier(FuzzySet fs, qualifiers q)
        {
            FuzzySet r = new FuzzySet();

            switch (q)
            {
                case qualifiers.not:
                    for (int i = 0; i < fs.Count(); i++)
                        r.Add(1.0 - fs.Get(i).y, fs.Get(i).x);
                    break;
                case qualifiers.very:
                    for (int i = 0; i < fs.Count(); i++)
                        r.Add(fs.Get(i).y * fs.Get(i).y, fs.Get(i).x);
                    break;
                case qualifiers.moreOrLess:
                    for (int i = 0; i < fs.Count(); i++)
                        r.Add(Math.Sqrt(fs.Get(i).y), fs.Get(i).x);
                    break;
            }

            return r;
        }

        /// <summary>
        /// Вывести функцию принадлежности
        /// </summary>
        /// <param name="chart">Диаграмма</param>
        public void ToGraph(string name, Chart graph)
        {
            sets[name].Draw(graph);
        }
    }

    /// <summary>
    /// Нечеткие выводы
    /// </summary>
    class FuzzyOutput
    {
        private static Dictionary<string, LinguisticVariable> lv;
        private static Dictionary<string, double> parameters;
        private static List<Rule> rules;


        public class Rule
        {
            public Term result;
            public Expression expression;

            public Rule(Term result, Expression expression)
            {
                this.result = result;
                this.expression = expression;
            }
        }

        public class Expression
        {
            public Term t1;
            public Term t2;
            public double y1 = -1.0;
            public double y2 = -1.0;
            public Expression e1;
            public Expression e2;
            public operation action;

            public enum operation
            {
                and,
                or
            }

            public Expression(Expression e1, operation action, Term t2)
            {
                this.e1 = e1;
                this.action = action;
                this.t2 = t2;
            }

            public Expression(Term t1, operation action, Expression e2)
            {
                this.t1 = t1;
                this.action = action;
                this.e2 = e2;
            }

            public Expression(Term t1, operation action, Term t2)
            {
                this.t1 = t1;
                this.action = action;
                this.t2 = t2;
            }

            public Expression(Term t)
            {
                this.t1 = t;
            }
        }

        public class Term
        {
            // имя лингвистической переменной к которой принадлежит
            // (необходим в выводах)
            public string nameLv;
            public FuzzySet set;
            public LinguisticVariable.qualifiers[] qualifiers;

            public Term(string nameLv, string nameSet, params LinguisticVariable.qualifiers[] qualifiers)
            {
                if (lv.ContainsKey(nameLv) == false)
                    throw new Exception("Ошибка! Нет такой переменной.");

                if (lv[nameLv].sets.ContainsKey(nameSet) == false)
                    throw new Exception("Ошибка! Нет такой переменной.");

                this.nameLv = nameLv;
                set = lv[nameLv].sets[nameSet];

                // если квалификатор отсутствует, то добавим пустой квалификатор
                if (qualifiers.Length == 0)
                    this.qualifiers = null;
                else
                    this.qualifiers = qualifiers;
            }
        }

        public FuzzyOutput()
        {
            lv = new Dictionary<string, LinguisticVariable>();
            parameters = new Dictionary<string, double>();
            rules = new List<Rule>();
        }

        public void AddVariable(LinguisticVariable lv)
        {
            FuzzyOutput.lv.Add(lv.name, lv);
        }

        public void AddParameter(string nameLv, double x)
        {
            if (lv.ContainsKey(nameLv) == false)
                throw new Exception("Ошибка! Нет такой переменной.");

            if (parameters.ContainsKey(nameLv))
                throw new Exception("Ошибка! Такой параметр уже имееся.");

            parameters.Add(nameLv, x);
        }

        public void AddRule(Term result, Expression expression)
        {
            rules.Add(new Rule(result, expression));
        }

        /// <summary>
        /// Применяет все квалификаторы терма
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private Term ApplyQualifier(Term a)
        {
            if (a.qualifiers != null && a.qualifiers.Length != 0)
            {
                for (int i = 0; i < a.qualifiers.Length; i++)
                    a.set = LinguisticVariable.UseQualifier(a.set, a.qualifiers[i]);

                a.qualifiers = null;
                return a;
            }
            else
                return a;
        }

        /// <summary>
        /// Добавляет результат выполнения операции как операнд предыдущей операции
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="y"></param>
        /// <param name="setsHeight"></param>
        /// <param name="flagFirst"></param>
        private void AddStep(Expression previous, double y, List<FuzzySet> setsHeight, bool flagFirst)
        {
            FuzzySet r = new FuzzySet();
            r.Add(y, 0);

            if (flagFirst)
                setsHeight.Add(r);
            else
            {
                if (previous.t1 == null)
                    previous.y1 = y;

                if (previous.t2 == null)
                    previous.y2 = y;
            }
        }

        /// <summary>
        /// Обходит правила и выполняет их - результат список высот в виде множеств с одной точкой
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="setsHeight"></param>
        private void StepDeep(Expression expression, Expression previous, List<FuzzySet> setsHeight, bool flagFisrt)
        {
            if (expression.e2 != null)
                StepDeep(expression.e2, expression, setsHeight, false);

            if (expression.e1 != null)
                StepDeep(expression.e1, expression, setsHeight, false);

            double a1 = -1;
            double a2 = -1;
            FuzzySet r = new FuzzySet();

            // если терм не пустой, то вычисляем для него степень функции
            // иначе в expression хранится уже вычисленная степень
            if (expression.t1 != null)
                a1 = FuzzySet.CheckElement(ApplyQualifier(expression.t1).set, parameters[expression.t1.nameLv]);
            else
                a1 = expression.y1;

            if (expression.t2 != null)
                a2 = FuzzySet.CheckElement(ApplyQualifier(expression.t2).set, parameters[expression.t2.nameLv]);
            else
                a2 = expression.y2;

            // если a2 == -1 значит операнд лишь один иначе два
            if (a2 == -1)
                AddStep(previous, a1, setsHeight, flagFisrt);
            else
                switch (expression.action)
                {
                    case Expression.operation.and:
                        AddStep(previous, Math.Min(a1, a2), setsHeight, flagFisrt);
                        break;
                    case Expression.operation.or:
                        AddStep(previous, Math.Max(a1, a2), setsHeight, flagFisrt);
                        break;
                }
        }

        public FuzzySet Output()
        {
            if (rules.Count == 0)
                throw new Exception("Отсутствуют правила");

            // резльтат
            FuzzySet r = new FuzzySet();
            // для хранения степени принадлежности
            Dictionary<string, Dictionary<string, double>> rulesVariables = new Dictionary<string, Dictionary<string, double>>();
            // результат применения правил (содержит одну точку)
            List<FuzzySet> setsHeight = new List<FuzzySet>();
            // результат применения правил (содержит одну точку)
            List<FuzzySet> setsIntersection = new List<FuzzySet>();

            // 1.
            // находим степень принадлежности для правил
            for (int i = 0; i < rules.Count; i++)
                StepDeep(rules[i].expression, rules[i].expression, setsHeight, true);

            // 2.
            //  применяем правила ко множествам
            for (int i = 0; i < rules.Count; i++)
                setsIntersection.Add(FuzzySet.Intersection(ApplyQualifier(rules[i].result).set, setsHeight[i]));

            // 3.
            // объединяем результат всех правил в одно множество
            r = setsIntersection[0];

            for (int i = 1; i < setsIntersection.Count; i++)
                r = FuzzySet.Unification(r, setsIntersection[i]);

            return r;
        }
    }

    /// <summary>
    /// Нечеткие отношения
    /// </summary>
    class FuzzyRelation
    {
        double[,] matrix;

        public FuzzyRelation(int r, int c)
        {
            matrix = new double[r, c];
        }

        /// <summary>
        /// Индексатор
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            get
            {
                return matrix[i, j];
            }
            set
            {
                if (value >= 0 || value <= 1)
                    matrix[i, j] = value;
                else
                    throw new Exception("Значение должно быть от 0 до 1!");
            }
        }

        /// <summary>
        /// Вывод матрицы
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append(matrix[i, j] + " ");
                }
                sb.Append("\r\n");
            }


            return sb.ToString();
        }

        /////Операции над НО/////

        /// <summary>
        /// Носитель
        /// </summary>
        /// <returns></returns>
        public bool Carrier()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] == 0)
                        return false;

            return true;
        }

        /// <summary>
        /// a - уровень
        /// </summary>
        /// <returns></returns>
        public bool ALevel(double a)
        {
            if (a < 0 || a > 1)
                throw new Exception("Параметр a должен входить в диапазон [0,1]");

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] < a)
                        return false;

            return true;
        }

        /// <summary>
        /// Высота
        /// </summary>
        /// <returns></returns>
        public double Height()
        {
            double height = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] > height)
                        height = matrix[i, j];

            return height;
        }

        /// <summary>
        /// Мода
        /// </summary>
        /// <returns></returns>
        public double Mode()
        {
            double height = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] > height)
                        height = matrix[i, j];

            return height;
        }

        /// <summary>
        /// Ядро
        /// </summary>
        /// <returns></returns>
        public bool Core()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] == 1)
                        return true;

            return false;
        }

        /// <summary>
        /// Доминанта
        /// </summary>
        /// <returns></returns>
        public FuzzyRelation Dominant(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), matrix.GetLength(1));

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] > fr[i, j])
                        result[i, j] = matrix[i, j];
                    else
                        result[i, j] = fr[i, j];

            return result;
        }

        /// <summary>
        /// Пересечение
        /// </summary>
        /// <param name="fr"></param>
        public FuzzyRelation Intersection(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), matrix.GetLength(1));

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[i, j] = Math.Min(matrix[i, j], fr[i, j]);

            return result;
        }

        /// <summary>
        /// Объединение
        /// </summary>
        /// <param name="fr"></param>
        public FuzzyRelation Unification(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), matrix.GetLength(1));

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[i, j] = Math.Max(matrix[i, j], fr[i, j]);

            return result;
        }

        /// <summary>
        /// Дополнение
        /// </summary>
        /// <param name="fr"></param>
        public bool Extension(FuzzyRelation fr)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (matrix[i, j] != 1 - fr[i, j])
                        return false;

            return true;
        }

        /// <summary>
        /// Дизъюнктивная сумма
        /// </summary>
        /// <param name="fr"></param>
        public FuzzyRelation DisjunctiveSum(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), matrix.GetLength(1));

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[i, j] =
                    Math.Max(Math.Min(matrix[i, j], 1 - fr[i, j]),
                    Math.Min(1 - matrix[i, j], fr[i, j]));

            return result;
        }

        /// <summary>
        /// Произведение
        /// </summary>
        /// <param name="fr"></param>
        public FuzzyRelation MultiplyAll(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), matrix.GetLength(1));

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[i, j] = matrix[i, j] * fr[i, j];

            return result;
        }

        /// <summary>
        /// Сумма
        /// </summary>
        /// <param name="fr"></param>
        public FuzzyRelation Amount(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), matrix.GetLength(1));

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    result[i, j] = matrix[i, j] + fr[i, j] - matrix[i, j] * fr[i, j];

            return result;
        }

        /// <summary>
        /// Композиция (свертка)
        /// </summary>
        /// <param name="fr"></param>
        /// <returns></returns>
        public FuzzyRelation Convolution(FuzzyRelation fr)
        {
            FuzzyRelation result = new FuzzyRelation(matrix.GetLength(0), fr.matrix.GetLength(1));

            for (var i = 0; i < matrix.GetLength(0); i++)
                for (var j = 0; j < fr.matrix.GetLength(1); j++)
                    for (var k = 0; k < matrix.GetLength(1); k++)
                        result[i, j] = Math.Max(Math.Min(matrix[i, k], fr[k, j]), result[i, j]);

            return result;
        }
    }
}