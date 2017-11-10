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
        bool show_numbers;

        public FormGame15()
        {
            InitializeComponent();
            buttons = new Button[game_size * game_size] { button0,  button1,  button2,  button3,
                                                          button4,  button5,  button6,  button7,
                                                          button8,  button9,  button10, button11,
                                                          button12, button13, button14, button15 };
            game = new Game(game_size);
            show_numbers = false;
            Load_puzzle();
        }

        /// <summary>
        /// Starts a new game session with current picture
        /// </summary>
        private void Start_game()
        {
            game.Start();
            for (int i = 0; i < 1000; i++)
                game.Shift_random();
            Refresh_table();
        }

        /// <summary>
        /// Handles clicks on table tiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button0_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt16(((Button)sender).Tag);
            game.Shift(position);
            Refresh_table();
            if (game.Check_numbers())
            {
                MessageBox.Show("Congratulations, you won!");
                Start_game();
            }
        }

        /// <summary>
        /// Handles initial game session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGame15_Load(object sender, EventArgs e)
        {
            Start_game();
        }

        /// <summary>
        /// Handles New Game menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGame_Click(object sender, EventArgs e)
        {
            Start_game();
        }

        /// <summary>
        /// Handles the Import Image menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportImage_Click(object sender, EventArgs e)
        {
            DialogResult path = openFileDialog1.ShowDialog();

            if (path == DialogResult.OK)
            {
                string picture = openFileDialog1.FileName;
                try
                {
                    Load_puzzle(picture);
                    Start_game();
                }
                catch (IOException)
                {
                }
            }
        }

        /// <summary>
        /// Handles Show Image menu button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowImage_Click(object sender, EventArgs e)
        {
            help.Show();
        }

        /// <summary>
        /// Hides the complete image upon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, EventArgs e)
        {
            help.Hide();
        }

        /// <summary>
        /// Handles Show Numbers checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowNumbers_Click(object sender, EventArgs e)
        {
            show_numbers = !show_numbers;
            Refresh_table();
        }

        /// <summary>
        /// Updates the tile images and numbers according to game logic module
        /// </summary>
        private void Refresh_table()
        {
            for (int position = 0; position < game_size * game_size; position++)
            {
                int number = game.Get_number(position);

                if (show_numbers)
                {
                    buttons[position].Text = number.ToString();
                }
                else
                {
                    buttons[position].ResetText();
                }

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
        /// Loads the image into the program, scales it and cuts into tiles
        /// </summary>
        /// <param name="path"></param>
        private void Load_puzzle(string path = "codeboys.jpg")
        {
            if (String.Compare(path, "codeboys.jpg") == 0)
            {
                path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, path);
            }

            try
            {
                puzzle = Image.FromFile(path);
            }
            catch (System.OutOfMemoryException)
            {
                MessageBox.Show("This file is not supported by the program. Please choose another image file.");
            }
            
            puzzle = ResizeImage(puzzle, tableLayoutPanel1.Width, tableLayoutPanel1.Height);

            help.Image = puzzle;

            plates = new Bitmap[game_size * game_size];

            int widthPart = (int)((double)puzzle.Width / game_size + 0.5);
            int heightPart = (int)((double)puzzle.Height / game_size + 0.5);

            for (int i = 0; i < game_size; i++)
            {
                for (int j = 0; j < game_size; j++)
                {
                    plates[i * game_size + j] = new Bitmap(widthPart, heightPart);
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
