using AxWMPLib;

namespace MediaUtility
{
    public partial class Form1 : Form
    {
        private readonly string imagePath = Path.Combine(Application.StartupPath, "bg.png");
        private readonly string videoPath = Path.Combine(Application.StartupPath, "bg.mp4");


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            axWindowsMediaPlayer1.Visible = false;
            timer1.Interval = 2000; // 2 seconds
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Handle image
            if (File.Exists(imagePath))
            {
                try
                {
        
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        axWindowsMediaPlayer1.Visible = false;
                        pictureBox1.Visible = true;
                        if (pictureBox1.Image != null)
                            pictureBox1.Image.Dispose();
                      
                        pictureBox1.Dock = DockStyle.Fill;

                        pictureBox1.Image = new Bitmap(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Image error: " + ex.Message);
                }
            }
            else
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null; // ✅ This clears the image
                }

            }

            // Handle video
            if (File.Exists(videoPath))
            {
                try
                {
                    if (axWindowsMediaPlayer1.URL != videoPath)
                    {
                        axWindowsMediaPlayer1.Dock = DockStyle.Fill;
                        axWindowsMediaPlayer1.Visible = true;
                        pictureBox1.Visible = false;


                        axWindowsMediaPlayer1.URL = videoPath;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Video error: " + ex.Message);
                }
            }
            else
            {
                // If file was removed but player still has URL, stop and clear
                if (!string.IsNullOrEmpty(axWindowsMediaPlayer1.URL))
                {
                    try
                    {
                        axWindowsMediaPlayer1.Ctlcontrols.stop();
                        axWindowsMediaPlayer1.URL = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Video clear error: " + ex.Message);
                    }

                }
            }
        }
    }
}

