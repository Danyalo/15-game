using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Game15
{
    public partial class FormGame15 : Form
    {
        Game game;
        const int game_size = 4;
        Button[] buttons;
        Image puzzle;
        Bitmap[] plates;

        public FormGame15()
        {
            InitializeComponent();
            buttons = new Button[game_size * game_size] { button0,  button1,  button2,  button3,
                                                          button4,  button5,  button6,  button7,
                                                          button8,  button9,  button10, button11,
                                                          button12, button13, button14, button15 };
            game = new Game(game_size);
        }

        private void Load_puzzle(string path = "codeboys.jpg")
        {
            if (String.Compare(path, "codeboys.jpg") == 0)
            {
                path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, path);
            }
            puzzle = Image.FromFile(path);
            puzzle = ResizeImage(puzzle, tableLayoutPanel1.Width, tableLayoutPanel1.Height);

            help.Image = puzzle;

            plates = new Bitmap[game_size * game_size];

            int widthPart = (int)((double)puzzle.Width / game_size + 0.5);
            int heightPart = (int)((double)puzzle.Height / game_size + 0.5);
            
            for (int i = 0; i < game_size; i++)
            {
                for (int j = 0; j < game_size; j++)
                {
                    plates[i*game_size + j] = new Bitmap(widthPart, heightPart);
                    Graphics g = Graphics.FromImage(plates[i * game_size + j]);
                    g.DrawImage(puzzle, new Rectangle(0, 0, widthPart, heightPart), new Rectangle(j * widthPart, i * heightPart, widthPart, heightPart), GraphicsUnit.Pixel);
                    g.Dispose();
                }
            }

            for (int i = 0; i < game_size * game_size; i++)
            {
                buttons[i].Image = plates[i];
            }
        }

        private void Button0_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt16(((Button)sender).Tag);
            game.Shift(position);
            Refresh();
            if (game.Check_numbers())
            {
                MessageBox.Show("Congratulations, you won!");
                Start_game();
            }
        }

        private void Menu_start_Click(object sender, EventArgs e)
        {
            Start_game();
        }

        private void FormGame15_Load(object sender, EventArgs e)
        {
            Start_game();
        }

        private void showImage_Click(object sender, EventArgs e)
        {
            help.Show();
        }

        private void help_Click(object sender, EventArgs e)
        {
            help.Hide();
        }

        private void Start_game()
        {
            game.Start();
            Load_puzzle();
            for (int i = 0; i < 1000; i++)
                game.Shift_random();
            Refresh();
        }

        private void Refresh()
        {
            for (int position = 0; position < game_size * game_size; position++)
            {
                int number = game.Get_number(position);

                //buttons[position].Text = number.ToString();       // for casualfags

                if (number == 0)
                {
                    buttons[position].Visible = false;
                }
                else
                {
                    buttons[position].Image = plates[number - 1];
                    buttons[position].Visible = true;
                }
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
