using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MainForm
{
    public partial class Form1 : Form
    {
        private void Lab1()
        {
            FuzzySet a = new FuzzySet();

            a.Add(0, 2);
            a.Add(1, 3);
            a.Add(0, 4);

            FuzzySet b = new FuzzySet();

            b.Add(0, 3);
            b.Add(1, 4);
            b.Add(0, 5);

            FuzzySet c = new FuzzySet();

            // Объединение
            c = FuzzySet.Unification(a,b);
            // Пересечение
            c = FuzzySet.Intersection(a, b);
            //дополнение
            c = FuzzySet.Extension(a);
            c.Draw(Graph);
        }

        private void Lab2()
        {
            // функции принадлежности

            // треугольная
            FuzzySet a = new FuzzySet();

            a.Add(0, 2);
            a.Add(1, 4);
            a.Add(0, 6);
            //a.Draw(Graph);

            // трапецивидная
            FuzzySet b = new FuzzySet();

            b.Add(0, 2);
            b.Add(1, 4);
            b.Add(1, 6);
            b.Add(0, 8);
            //b.Draw(Graph);

            // линейная z - образная
            FuzzySet c = new FuzzySet();

            c.Add(1, 2);
            c.Add(0, 5);
            //c.Draw(Graph);

            // линейная s - образная
            FuzzySet d = new FuzzySet();

            d.Add(0, 2);
            d.Add(1, 5);
            //d.Draw(Graph);

            // линейная п - образная
            FuzzySet e = new FuzzySet();

            e.Add(0, 2);
            e.Add(1, 2);
            e.Add(1, 4);
            e.Add(0, 4);
            e.Draw(Graph);

            FuzzySet f = new FuzzySet();

            f.Add(0, 2);
            f.Add(1, 4);
            f.Add(1, 6);
            f.Add(0, 8);

            FuzzySet g = new FuzzySet();

            g = f.aLevel(0.5);
            g = f.Carrier();
            g = f.Concentration();
            g = f.Core();
            g = f.Stretching();
            g = f.Amount(e);
            g = f.Multiply(e);
        }

        private void Lab3()
        {
            // треугольное нечеткое число
            TriangularFuzzyNumber a1 = new TriangularFuzzyNumber(1, 2, 3);
            TriangularFuzzyNumber b1 = new TriangularFuzzyNumber(3, 4, 5);
            TriangularFuzzyNumber.Amount(a1, b1).Draw(Graph);

            //трапецивидный нечеткий интервал
            TrapezoidalFuzzyInterval a2 = new TrapezoidalFuzzyInterval(1, 2, 3, 4);
            TrapezoidalFuzzyInterval b2 = new TrapezoidalFuzzyInterval(3, 4, 5, 6);
            TrapezoidalFuzzyInterval.Amount(a2, b2).Draw(Graph);

            // нечеткое число
            FuzzyNumber a3 = new FuzzyNumber();
            a3.Add(0, 1);
            a3.Add(1, 2);
            a3.Add(0, 3);
            FuzzyNumber b3 = new FuzzyNumber();
            b3.Add(0, 4);
            b3.Add(1, 5);
            b3.Add(0, 6);

            FuzzyNumber.Amount(a3, b3).Draw(Graph);
            FuzzyNumber.Difference(a3, b3).Draw(Graph);

            // нечеткий интервал
            FuzzyInterval a4 = new FuzzyInterval();
            a4.Add(0, 1);
            a4.Add(0.5, 2);
            a4.Add(1, 3);
            a4.Add(0.6, 4);
            a4.Add(0, 5);

            FuzzyInterval b4 = new FuzzyInterval();
            b4.Add(0, 5);
            b4.Add(0.5, 6);
            b4.Add(1, 7);
            b4.Add(0.6, 8);
            b4.Add(0, 9);

            FuzzyInterval.Amount(a4, b4).Draw(Graph);
            FuzzyInterval.Difference(a4, b4).Draw(Graph);

            // нечеткое число LR
            FuzzyNumberLR a5 = new FuzzyNumberLR();
            a5.Add(0, 1);
            a5.Add(1, 2);
            a5.Add(0, 3);
            FuzzyNumberLR b5 = new FuzzyNumberLR();
            b5.Add(0, 4);
            b5.Add(1, 5);
            b5.Add(0, 6);

            FuzzyNumberLR.Amount(a5, b5).Draw(Graph);
            FuzzyNumberLR.Difference(a5, b5).Draw(Graph);

            //нечеткий интервал LR
            FuzzyIntervalLR a6 = new FuzzyIntervalLR();
            a5.Add(0, 1);
            a5.Add(1, 2);
            a5.Add(0, 3);
            FuzzyIntervalLR b6 = new FuzzyIntervalLR();
            b5.Add(0, 4);
            b5.Add(1, 5);
            b5.Add(0, 6);

            FuzzyIntervalLR.Amount(a6, b6).Draw(Graph);
            FuzzyIntervalLR.Difference(a6, b6).Draw(Graph);

        }

        private void Lab4()
        {
            FuzzySet a = new FuzzySet();
            a.Add(0.1,1);
            a.Add(1, 3);
            a.Add(0.9, 4);
            a.Add(1, 5);

            FuzzySet b = new FuzzySet();
            b.Add(0.2, 2);
            b.Add(0.8, 3);
            b.Add(1, 4);
            b.Add(0.5, 5);

            // расстояние по Хеммингу
            double d = FuzzySet.Distance(a, b);

            // сравнение нечеткости множеств
            FuzzySet c = FuzzySet.ComparisonFuzzySets(a,b);

            FuzzySet q = new FuzzySet();
            q.Add(0.2,2);
            q.Add(0.7, 3);
            q.Add(1.0, 4);
            q.Add(0.5, 5);

            // построение более нечеткого множества
            FuzzySet c1 = FuzzySet.GetMoreFuzzySet(q);
        }

        private void Lab5()
        {
            FuzzyRelation a = new FuzzyRelation(3,3);
            a[0, 0] = 0.5;
            a[1, 0] = 0.3;
            a[0, 2] = 0.7;
            a[1, 1] = 0;
            a[1, 2] = 1;
            a[0, 1] = 0.1;
            a[1, 2] = 0.2;
            a[2, 2] = 1;


            FuzzyRelation b = new FuzzyRelation(3, 3);
            a[0, 0] = 0.7;
            a[1, 0] = 0.8;
            a[0, 2] = 0.7;
            a[1, 1] = 0.9;
            a[1, 2] = 1;
            a[0, 1] = 1;
            a[1, 2] = 0.4;
            a[2, 2] = 0;

            a.ALevel(0.5);
            a.Carrier();
            a.Core();

            a.Amount(b);

            a.Convolution(b);

            string r = a.ToString();
        }

        private void Lab6()
        {
            LinguisticVariable t = new LinguisticVariable("Температура", 0, 150, new string[] { "Высокая", "Средняя", "Низкая" });

            t.Add("Высокая", 0, 50);
            t.Add("Высокая", 1, 100);

            t.Add("Средняя", 0, 0);
            t.Add("Средняя", 1, 50);
            t.Add("Средняя", 1, 100);
            t.Add("Средняя", 0, 150);

            t.Add("Низкая", 1, 50);
            t.Add("Низкая", 0, 100);

            LinguisticVariable d = new LinguisticVariable("Давление", 0, 100, new string[] { "Высокое", "Среднее", "Низкое" });

            d.Add("Высокое", 0, 0);
            d.Add("Высокое", 1, 100);

            d.Add("Среднее", 0, 0);
            d.Add("Среднее", 1, 50);
            d.Add("Среднее", 0, 100);

            d.Add("Низкое", 1, 0);
            d.Add("Низкое", 0, 100);


            LinguisticVariable r = new LinguisticVariable("Расход", 0, 8, new string[] { "Большой", "Средний", "Малый" });

            r.Add("Большой", 0, 4);
            r.Add("Большой", 1, 6);
            r.Add("Большой", 0, 8);

            r.Add("Средний", 0, 2);
            r.Add("Средний", 1, 4);
            r.Add("Средний", 0, 6);

            r.Add("Малый", 0, 0);
            r.Add("Малый", 1, 2);
            r.Add("Малый", 0, 4);


            FuzzyOutput fo = new FuzzyOutput();

            fo.AddVariable(t);
            fo.AddVariable(d);
            fo.AddVariable(r);

            fo.AddParameter("Температура", 85);
            fo.AddParameter("Расход", 3.5);

            fo.AddRule(
                new FuzzyOutput.Term("Давление", "Низкое"),
                new FuzzyOutput.Expression(
                    new FuzzyOutput.Term("Температура", "Низкая"),
                    FuzzyOutput.Expression.operation.and,
                    new FuzzyOutput.Term("Расход", "Малый")));

            fo.AddRule(
              new FuzzyOutput.Term("Давление", "Среднее"),
              new FuzzyOutput.Expression(
                  new FuzzyOutput.Term("Температура", "Средняя")));

            fo.AddRule(
              new FuzzyOutput.Term("Давление", "Высокое"),
              new FuzzyOutput.Expression(
                  new FuzzyOutput.Term("Температура", "Высокая"),
                  FuzzyOutput.Expression.operation.or,
                  new FuzzyOutput.Term("Расход", "Большой")));


            fo.Output().Draw(Graph);
        }

        private void Lab7()
        {
            LinguisticVariable t = new LinguisticVariable("Температура", 0, 150, new string[] { "Высокая", "Средняя", "Низкая" });

            t.Add("Высокая", 0, 50);
            t.Add("Высокая", 1, 100);

            t.Add("Средняя", 0, 0);
            t.Add("Средняя", 1, 50);
            t.Add("Средняя", 1, 100);
            t.Add("Средняя", 0, 150);

            t.Add("Низкая", 1, 50);
            t.Add("Низкая", 0, 100);

            LinguisticVariable d = new LinguisticVariable("Давление", 0, 100, new string[] { "Высокое", "Среднее", "Низкое" });

            d.Add("Высокое", 0, 0);
            d.Add("Высокое", 1, 100);

            d.Add("Среднее", 0, 0);
            d.Add("Среднее", 1, 50);
            d.Add("Среднее", 0, 100);

            d.Add("Низкое", 1, 0);
            d.Add("Низкое", 0, 100);


            LinguisticVariable r = new LinguisticVariable("Расход", 0, 8, new string[] { "Большой", "Средний", "Малый" });

            r.Add("Большой", 0, 4);
            r.Add("Большой", 1, 6);
            r.Add("Большой", 0, 8);

            r.Add("Средний", 0, 2);
            r.Add("Средний", 1, 4);
            r.Add("Средний", 0, 6);

            r.Add("Малый", 0, 0);
            r.Add("Малый", 1, 2);
            r.Add("Малый", 0, 4);


            FuzzyOutput fo = new FuzzyOutput();

            fo.AddVariable(t);
            fo.AddVariable(d);
            fo.AddVariable(r);

            fo.AddParameter("Температура", 85);
            fo.AddParameter("Расход", 3.5);

            fo.AddRule(
                new FuzzyOutput.Term("Давление", "Низкое"),
                new FuzzyOutput.Expression(
                    new FuzzyOutput.Term("Температура", "Низкая"),
                    FuzzyOutput.Expression.operation.and,
                    new FuzzyOutput.Term("Расход", "Малый")));

            fo.AddRule(
              new FuzzyOutput.Term("Давление", "Среднее"),
              new FuzzyOutput.Expression(
                  new FuzzyOutput.Term("Температура", "Средняя")));

            fo.AddRule(
              new FuzzyOutput.Term("Давление", "Высокое"),
              new FuzzyOutput.Expression(
                  new FuzzyOutput.Term("Температура", "Высокая"),
                  FuzzyOutput.Expression.operation.or,
                  new FuzzyOutput.Term("Расход", "Большой")));


            fo.Output().Draw(Graph);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Lab1();
            //Lab2();
            //Lab3();
            //Lab4();
            //Lab5();
            //Lab6();
            Lab7();
        }
    }
}
