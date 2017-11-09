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

        public FormGame15()
        {
            InitializeComponent();
            game = new Game(4);
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

        private Button button (int position)
        {
            switch (position)
            {
                case 0: return button0;
                case 1: return button1;
                case 2: return button2;
                case 3: return button3;
                case 4: return button4;
                case 5: return button5;
                case 6: return button6;
                case 7: return button7;
                case 8: return button8;
                case 9: return button9;
                case 10: return button10;
                case 11: return button11;
                case 12: return button12;
                case 13: return button13;
                case 14: return button14;
                case 15: return button15;
                default: return null;
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
                int nr = game.get_number(position);
                button(position).Text = nr.ToString();
                button(position).Visible = (nr > 0);
            }
        }
    }
}
