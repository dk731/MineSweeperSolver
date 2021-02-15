using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeperSolver
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;

        private Color c1, c2, c3, c4, c5, c0, cb, cf, cg, c6;
        private HashSet<Color> c00;
        public Form1()
        {
            InitializeComponent();
            c00 = new HashSet<Color>();
            c1 = Color.FromArgb(47, 47, 239);
            c2 = Color.FromArgb(72, 152, 72);
            c3 = Color.FromArgb(224, 96, 96);
            c4 = Color.FromArgb(47, 47, 143);
            c5 = Color.FromArgb(160, 96, 96);
            c6 = Color.FromArgb(96, 160, 160);
            c0 = Color.FromArgb(192, 192, 192);
            cb = Color.FromArgb(0, 0, 0);
            cf = Color.FromArgb(103, 103, 103);
            cg = Color.FromArgb(145, 145, 145);
            for (int i = 230; i < 255; i++)
            {
                c00.Add(Color.FromArgb(i,i,i));
            }

        }

        private void ClickOn(int x, int y)
        {

            Cursor.Position = new Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(25); // sleep 5 sec
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(25);

        }

        private void RefreshGame()
        {
            Cursor.Position = new Point(1175, 125);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(100); // sleep 5 sec
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(2000);
        }


        int dif = 6;
        private Tuple<HashSet<Point>, HashSet<Point>> returnVal;

        private void button2_Click(object sender, EventArgs e)
        {
            //webBrowser1.Navigate("https://minesweeper.online");
        }

        private HashSet<Point> pointsToClick, pointsToClickFlag, flagget;
        private int[,] numField;
        private Tuple<float, int>[,] field;
        private List<Point> minPoints;
        private List<Point> pointList, toRemove;


        private void button1_Click(object sender, EventArgs e)
        {
            field = new Tuple<float, int>[80, 80];
            numField = new int[80, 80];
            //RefreshGame();
            var rand = new Random();
            pointsToClick = new HashSet<Point>();
            pointsToClickFlag = new HashSet<Point>();
            flagget = new HashSet<Point>();
            minPoints = new List<Point>();
            Point lastClick = new Point(40, 40);
            pointList = new List<Point>();
            toRemove = new List<Point>();

        /*DirectBitmap myBitmap = new DirectBitmap(480, 480);
        Graphics g = Graphics.FromImage(myBitmap.Bitmap);
        g.CopyFromScreen(1056, 158, 0, 0, new Size(480, 480));
        myBitmap.Bitmap.Save(@"C:\Users\user\Desktop\test.png", ImageFormat.Png);
        return;*/


        startAgain:
            {

                toRemove.Clear();
                minPoints.Clear();
                flagget.Clear();
                pointsToClick.Clear();
                pointsToClickFlag.Clear();
                pointList.Clear();
                for (int m = 0; m < 80; m++)
                {
                    for (int n = 0; n < 80; n++)
                    {
                        pointList.Add(new Point(n, m));
                    }
                }


                RefreshGame();

                ClickLeft(40, 40);
                

                while (true)
                {

                    

                    for (int m = 0; m < 80; m++)
                    {
                        for (int n = 0; n < 80; n++)
                        {
                            numField[n, m] = 1;
                            field[n, m] = Tuple.Create(99999.0f, 0);
                        }
                    }


                    DirectBitmap myBitmap = new DirectBitmap(480, 480);
                    Graphics g = Graphics.FromImage(myBitmap.Bitmap);

                   
                   
                    
                    g.CopyFromScreen(1056, 158, 0, 0, new Size(480, 480));

                    myBitmap.Bitmap.Save(@"C:\Users\user\Desktop\test.png", ImageFormat.Png);

                    pointsToClick.Clear();
                    pointsToClickFlag.Clear();

                    foreach(Point a in pointList)
                    {
                        if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c0)
                        {
                            if (c00.Contains(myBitmap.GetPixel(a.X * dif, a.Y * dif)))
                            {
                                numField[a.X, a.Y] = 0; // pixels is under question
                            }
                            else
                            {
                                numField[a.X, a.Y] = -1; // pixel is clear
                            }
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c1)
                        {
                            numField[a.X, a.Y] = 1;
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c2)
                        {
                            numField[a.X, a.Y] = 2;
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == cf)
                        {
                            numField[a.X, a.Y] = -2; // pixel is flag
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c3)
                        {
                            numField[a.X, a.Y] = 3;
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c4)
                        {
                            numField[a.X, a.Y] = 4;
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c5)
                        {
                            numField[a.X, a.Y] = 5;
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == c6)
                        {
                            numField[a.X, a.Y] = 6;
                        }
                        else if (myBitmap.GetPixel(a.X * dif + 3, a.Y * dif + 3) == cb)
                        {
                            goto startAgain;

                        }
                    }
                            

                    foreach (Point a in pointList)
                    {
                        if (numField[a.X, a.Y] != 666)
                        {
                            doWork(a.X, a.Y, numField[a.X, a.Y]);
                        }

                    }

                      

                    if (pointsToClick.Count() == 0 && pointsToClickFlag.Count() == 0)
                    {
                        double min = 1000000.0f;
                        minPoints.Clear();
                        double tmp;
                        foreach (Point a in pointList)
                        {
                            if (field[a.X, a.Y].Item2 != 0)
                            {
                                tmp = Math.Sqrt(field[a.X, a.Y].Item1 / field[a.X, a.Y].Item2);
                                Debug.WriteLine(tmp);
                                if (tmp < min)
                                {
                                    min = tmp;
                                    minPoints.Clear();
                                    minPoints.Add(new Point(a.X, a.Y));
                                }
                                else if (tmp == min)
                                {
                                    minPoints.Add(new Point(a.X, a.Y));
                                }
                            }
                        }

                        try
                        {
                            pointsToClick.Add(minPoints[rand.Next(minPoints.Count())]);
                        }
                        catch (Exception ee)
                        {

                        }

                    }
                            

                       
                    

                    

                    foreach (Point a in pointsToClick)
                    {
                        
                        ClickLeft(a.X, a.Y);
                    }

                    foreach (Point a in pointsToClickFlag)
                    {
                        
                        ClickRight(a.X, a.Y);
                    }
                    foreach(Point a in toRemove)
                    {
                        pointList.Remove(a);
                    }
                    toRemove.Clear();





                }
            }
            
            
        }

        private void doWork(int x, int y, int n)
        {

            if (!(n > 0 ))
            {
                return;
            }

            returnVal = CheckAround(x, y);
            if (returnVal.Item1.Count() == 0)
                return;
            if (returnVal.Item2.Count() == n)
            {
                pointsToClick.UnionWith(returnVal.Item1);
            }
            else if (returnVal.Item1.Count() + returnVal.Item2.Count() == n)
            {
                returnVal.Item1.ExceptWith(flagget);
                flagget.UnionWith(returnVal.Item1);
                pointsToClickFlag.UnionWith(returnVal.Item1);
            }
            else
            {
                float val = (n - returnVal.Item2.Count()) / (float)returnVal.Item1.Count();
                foreach (Point p in returnVal.Item1)
                {
                    if (field[p.X, p.Y].Item2 == 0)
                    {
                        field[p.X, p.Y] = Tuple.Create((val * val), 1);
                    }
                    else
                    {
                        field[p.X, p.Y] = Tuple.Create(field[p.X, p.Y].Item1 + (val * val), field[p.X, p.Y].Item2 + 1);
                    }
                }
            }

        }

        private void ClickRight(int x, int y)
        {

            Cursor.Position = new Point(1058 + x*dif, 160 + y*dif);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            Thread.Sleep(5);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            Cursor.Position = new Point(100, 100);
            //Thread.Sleep(10);
            //Thread.Sleep(1000);
        }

        private void ClickLeft(int x, int y)
        {

            Cursor.Position = new Point(1058 + x * dif, 160 + y * dif);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(5);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Cursor.Position = new Point(100, 100);
            //Thread.Sleep(10);   
            //Thread.Sleep(1000);

        }

        private Tuple<HashSet<Point>, HashSet<Point>> CheckAround(int x, int y)
        {
            HashSet<Point> tmpFlag = new HashSet<Point>();
            HashSet<Point> tmpField = new HashSet<Point>();

            
            try
            {
                switch (numField[x + 1, y + 1])
                {
                    case 0:
                        tmpField.Add(new Point(x + 1, y + 1));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x + 1, y + 1));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x, y + 1])
                {
                    case 0:
                        tmpField.Add(new Point(x, y + 1));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x, y + 1));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x - 1, y + 1])
                {
                    case 0:
                        tmpField.Add(new Point(x - 1, y + 1));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x - 1, y + 1));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x - 1, y])
                {
                    case 0:
                        tmpField.Add(new Point(x - 1, y));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x - 1, y));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x - 1, y - 1])
                {
                    case 0:
                        tmpField.Add(new Point(x - 1, y - 1));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x - 1, y - 1));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x, y - 1])
                {
                    case 0:
                        tmpField.Add(new Point(x, y - 1));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x, y - 1));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x + 1, y - 1])
                {
                    case 0:
                        tmpField.Add(new Point(x + 1, y - 1));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x + 1, y - 1));
                        break;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                switch (numField[x + 1, y])
                {
                    case 0:
                        tmpField.Add(new Point(x + 1, y));
                        break;
                    case -2:
                        tmpFlag.Add(new Point(x + 1, y));
                        break;
                }
            }
            catch (Exception e)
            {

            }






            if(tmpField.Count() == 0)
            {
                numField[x, y] = 666;
                toRemove.Add(new Point(x, y));
            }

            return Tuple.Create(tmpField, tmpFlag);
        }

    }

    

    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}