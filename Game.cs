using System;

namespace Game15
{
    class Game
    {
        int size;
        int[,] map;
        int space_x, space_y;
        static Random rand = new Random();

        public Game (int size)
        {
            if (size < 2) size = 2;
            if (size > 5) size = 5;
            this.size = size;
            map = new int[size, size];
        }

        public void Start ()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    map[x, y] = Coords_to_position(x, y) + 1;
            space_x = size - 1;
            space_y = size - 1;
            map[space_x, space_y] = 0;
        }

        public int Space_position()
        {
            return Coords_to_position(space_x, space_y);
        }

        public void Shift (int position)
        {
            int x, y;
            Position_to_coords(position, out x, out y);
            if (Math.Abs(space_x - x) + Math.Abs(space_y - y) != 1)
                return;

            map[space_x, space_y] = map[x, y];
            map[x, y] = 0;
            space_x = x;
            space_y = y;
        }

        public void Shift_random ()
        {
            int x = space_x;
            int y = space_y;
            int a = rand.Next(0, 4);
            switch (a)
            {
                case 0: x--; break;
                case 1: x++; break;
                case 2: y--; break;
                case 3: y++; break;
            }
            Shift(Coords_to_position(x, y));
        }

        public bool Check_numbers ()
        {
            if (!(space_x == size - 1 &&
                space_y == size - 1))
                return false;
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (!(x == size - 1 && y == size - 1))    
                        if (map[x, y] != Coords_to_position(x, y) + 1)
                            return false;
            return true;
        }

        public int Get_number (int position)
        {
            int x, y;
            Position_to_coords(position, out x, out y);
            if (x < 0 || x >= size) return 0;
            if (y < 0 || y >= size) return 0;
            return map[x, y];
        }

        private int Coords_to_position (int x, int y)
        {
            if (x < 0) x = 0;
            if (x > size - 1) x = size - 1;
            if (y < 0) y = 0;
            if (y > size - 1) y = size - 1;
            return y * size + x;
        }

        public void Position_to_coords (int position, out int x, out int y)
        {
            if (position < 0) position = 0;
            if (position > size * size - 1)
            {
                position = size * size - 1;
            }

            x = position % size;
            y = position / size;
        }
    }
}
