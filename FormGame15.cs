using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game15
{
    public partial class FormGame15 : Form
    {
        Game game;
        const int game_size = 4;
        Button[] buttons;

        public FormGame15()
        {
            InitializeComponent();
            buttons = new Button[game_size * game_size] { button0,  button1,  button2,  button3,
                                                          button4,  button5,  button6,  button7,
                                                          button8,  button9,  button10, button11,
                                                          button12, button13, button14, button15 };
            game = new Game(game_size);
        }

        private void button0_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt16(((Button)sender).Tag);
            game.shift(position);
            refresh();
            if (game.check_numbers())
            {
                MessageBox.Show("Congratulations, you won!");
                start_game();
            }
        }

        private void menu_start_Click(object sender, EventArgs e)
        {
            start_game();
        }

        private void FormGame15_Load(object sender, EventArgs e)
        {
            start_game();
        }

        private void start_game()
        {
            game.start();
            for (int i = 0; i < 100; i++)
                game.shift_random();
            refresh();
        }

        private void refresh()
        {
            for (int position = 0; position < 16; position++)
            {
                int number = game.get_number(position);
                buttons[position].Text = number.ToString();
                buttons[position].Visible = (number > 0);
            }
        }
    }
}
