using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHA3.Tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            GenerateImage();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateValues();
            GenerateImage();
        }

        private CancellationTokenSource cancelGenerateImage;

        private void GenerateImage()
        {
            if (cancelGenerateImage != null)
                cancelGenerateImage.Cancel();

            cancelGenerateImage = new CancellationTokenSource();
            var token = cancelGenerateImage.Token;

            SetProgress(0);

            var UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();

            var progress = new IdleWorker(UISyncContext);
            var preview = new IdleWorker(UISyncContext, TimeSpan.FromMilliseconds(10));

            Task taskCoverageAndCycles = null;
            taskCoverageAndCycles = new Task(() =>
            {
                try
                {

                    var k = (int)numericUpDown1.Value;
                    var w = Math.Pow(2, (double)numericUpDownL.Value);
                    var b = w * k * k;

                    if (k < 2)
                        return;

                    if (k > 1000)
                        return;

                    int points = 0;
                    int repeatPoints = 0;

                    var g = new BitsGraphics(k, k);

                    var white = Color.White.ToArgb();
                    var red = Color.Red.ToArgb();
                    var black = Color.Black.ToArgb();
                    g.Clear(white);

                    int total = k * k * 2;
                    int x = 1;
                    int y = 0;
                    for (int t = 0; t < k * k - 1; t++)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        var prev = g.GetPixel(x, y);
                        if (prev != white)
                            break;

                        if (points % 1000 == 0)
                            progress.DoIfIdle(() => SetProgress(points * 100 / total));

                        if (points % 1000 == 0)
                            preview.DoIfIdle(() => UpdateImage(g));

                        g.SetPixel(red, x, y);
                        points++;

                        int x2 = y;
                        int y2 = (2 * x + 3 * y) % k;
                        x = x2;
                        y = y2;
                    }

                    total = points * 2;
                    int x0 = x;
                    int y0 = y;

                    for (int t = points; t < k * k - 1; t++)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        g.SetPixel(black, x, y);
                        repeatPoints++;

                        if (repeatPoints % 1000 == 0)
                            progress.DoIfIdle(() => SetProgress((points + repeatPoints) * 100 / total));

                        if (repeatPoints % 1000 == 0)
                            preview.DoIfIdle(() => UpdateImage(g));

                        int x2 = y;
                        int y2 = (2 * x + 3 * y) % k;
                        x = x2;
                        y = y2;

                        if (x == x0 && y == y0)
                            break;
                    }

                    progress.DoIfIdle(() => SetProgress(100));

                    taskCoverageAndCycles.ContinueWith(prevTask =>
                    {
                        UpdateImage(g);
                        textBox1.Text = points.ToString();
                        textBox2.Text = repeatPoints.ToString();
                        textBoxCoverage.Text = string.Format("{0}%", points * 100 / (k * k));
                    }, UISyncContext);
                }
                finally
                {
                    if (!token.IsCancellationRequested)
                    {
                        taskCoverageAndCycles.ContinueWith(prevTask =>
                        {
                            labelProgress.Visible = false;
                        }, UISyncContext);
                    }
                }
            }, token);

            var muls = new[]
            {
                0,
                0x00ffffff, // 255/1
                0x00555555, // 255/3
                0x00242424, // 255/7
                0x00111111, // 255/15
                0x00080808, // 255/31
                0x00040404, // 255/63
                0x00020202, // 255/127
                0x00010101, // 255/255
                0x00008008, // 4095/511
                0x00004004, // 4095/1023
                0x00002002, // 4095/2047
                0x00001001, // 4095/4095
            };

            Task taskCoverageAndDisplacement = null;
            taskCoverageAndDisplacement = new Task(() =>
            {
                try
                {

                    var k = (int)numericUpDown1.Value;
                    var l = (int)numericUpDownL.Value;
                    var w = (uint)Math.Pow(2, (double)numericUpDownL.Value);
                    var b = (uint)(w * k * k);
                    int mask = (int)((uint)(0xffffffff) >> (32 - l));
                    if (l == 0) mask = 0;

                    int mul = l >= muls.Length ? (int)(0xffffffff / (uint)mask) : muls[l];

                    if (k < 2)
                        return;

                    if (k > 1000)
                        return;

                    int points = 0;

                    var g = new BitsGraphics(k, k);

                    g.Clear(Color.Black.ToArgb());

                    int total = k * k;
                    int x = 1;
                    int y = 0;
                    for (int t = 0; t < k * k; t++)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        if (points % 1000 == 0)
                            progress.DoIfIdle(() => SetProgress(points * 100 / total));

                        if (points % 1000 == 0)
                            preview.DoIfIdle(() => UpdateImage(g));

                        var prev = g.GetPixel(x, y);
                        var displace = (t + 1) * (t + 2) / 2;
                        g.SetPixel(((prev + displace) & mask) * mul, x, y);
                        points++;

                        int x2 = y;
                        int y2 = (2 * x + 3 * y) % k;
                        x = x2;
                        y = y2;
                    }

                    progress.DoIfIdle(() => SetProgress(100));

                    taskCoverageAndDisplacement.ContinueWith(prevTask =>
                    {
                        UpdateImage(g);
                    }, UISyncContext);
                }
                finally
                {
                    if (!token.IsCancellationRequested)
                    {
                        taskCoverageAndDisplacement.ContinueWith(prevTask =>
                        {
                            labelProgress.Visible = false;
                        }, UISyncContext);
                    }
                }
            }, token);

            var tasks = new[] { taskCoverageAndCycles, taskCoverageAndDisplacement };

            tasks[comboBox1.SelectedIndex].Start(TaskScheduler.Default);
        }

        private void SetProgress(int perc)
        {
            labelProgress.Text = string.Format("Working... {0}%", perc);
            labelProgress.Visible = true;
            labelProgress.Update();
        }

        private void UpdateImage(BitsGraphics g)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            var w = g.Width;
            int zoom = Math.Max(1, Math.Min(this.pictureBox1.Width, this.pictureBox1.Height) / w);
            pictureBox1.Image = g.WrapAround(w / 2, w / 2).Zoom(zoom).ToBitmap();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            GenerateImage();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var panels = new[] { tableLayoutPanel1, tableLayoutPanel2 };
            for (int itPanel = 0; itPanel < panels.Length; itPanel++)
                panels[itPanel].Visible = comboBox1.SelectedIndex == itPanel;
            GenerateImage();
        }

        private void numericUpDownL_ValueChanged(object sender, EventArgs e)
        {
            UpdateValues();
            GenerateImage();
        }

        private void UpdateValues()
        {
            var k = (int)numericUpDown1.Value;
            var w = Math.Pow(2, (double)numericUpDownL.Value);
            var b = w * k * k;
            this.textBoxW.Text = string.Format("{0}", w);
            this.textBoxB.Text = string.Format("{0}", b);
        }
    }

    class IdleWorker
    {
        private int idle = 1;
        private DateTime finished;
        private readonly TaskScheduler taskScheduler;
        private readonly TimeSpan minInterval;

        public IdleWorker(TaskScheduler taskScheduler, TimeSpan? minInterval = null)
        {
            this.taskScheduler = taskScheduler;
            this.minInterval = minInterval ?? TimeSpan.Zero;
        }

        public void DoIfIdle(Action action)
        {
            if (DateTime.Now - this.finished < this.minInterval)
                return;

            if (Interlocked.CompareExchange(ref this.idle, 0, 1) == 1)
            {
                var task = new Task(action);
                task.ContinueWith(t =>
                {
                    this.idle = 1;
                    finished = DateTime.Now;
                });
                task.Start(this.taskScheduler);
            }
        }
    }

    class BitsGraphics
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        int[] bits;

        public BitsGraphics(int width, int height)
        {
            Width = width;
            Height = height;
            this.bits = new int[height * width];
        }

        public void FillRectangle(int color, int x, int y, int width, int height)
        {
            var pt0 = this.GetCoords(x, y);
            for (int itY = 0; itY < height; itY++)
            {
                for (int itX = 0; itX < width; itX++)
                    this.bits[pt0 + itX] = color;
                pt0 += this.Width;
            }
        }

        private int GetCoords(int x, int y)
        {
            return y * this.Width + x;
        }

        public void Clear(int color)
        {
            for (int it = 0; it < this.bits.Length; it++)
                this.bits[it] = color;
        }

        public Image ToBitmap()
        {
            var bitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppRgb);
            var bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            var pNative = bmData.Scan0;
            Marshal.Copy(this.bits, 0, pNative, this.Width * this.Height);
            bitmap.UnlockBits(bmData);
            return bitmap;
        }

        public int GetPixel(int x, int y)
        {
            return this.bits[this.GetCoords(x, y)];
        }

        public BitsGraphics Zoom(int amount)
        {
            var result = new BitsGraphics(this.Width * amount, this.Height * amount);
            var pt0 = 0;
            var clonedBits = this.GetBits();
            for (int itY = 0; itY < result.Height; itY++)
            {
                for (int itX = 0; itX < result.Width; itX++)
                {
                    result.bits[pt0 + itX] = clonedBits[this.GetCoords(itX / amount, itY / amount)];
                }
                pt0 += result.Width;
            }
            return result;
        }

        public void SetPixel(int color, int x, int y)
        {
            this.bits[this.GetCoords(x, y)] = color;
        }

        public int[] GetBits()
        {
            var result = new int[this.bits.Length];
            Buffer.BlockCopy(this.bits, 0, result, 0, this.bits.Length * 4);
            return result;
        }

        public BitsGraphics WrapAround(int x, int y)
        {
            x = x % this.Width;
            x += this.Width;
            x = x % this.Width;
            y = y % this.Height;
            y += this.Height;
            y = y % this.Height;

            var pt0s = 0;
            var pt0d = this.Width * y;

            var result = new BitsGraphics(this.Width, this.Height);
            for (int itY = 0; itY < this.Height; itY++)
            {
                Buffer.BlockCopy(this.bits, pt0s * 4, result.bits, (pt0d + x) * 4, (this.Width - x) * 4);
                Buffer.BlockCopy(this.bits, (pt0s + this.Width - x) * 4, result.bits, pt0d * 4, x * 4);

                pt0s = pt0s + this.Width;
                pt0d = (pt0d + this.Width) % this.bits.Length;
            }

            return result;
        }
    }
}
