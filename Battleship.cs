using System;

class Battleship
{
    private string name1; // имя первого игрока
    private string name2; // имя второго игрока
    private char[,] player1 = new char[10, 10]; // поле обороны первого игрока
    private char[,] player2 = new char[10, 10]; // поле обороны второго игрока
    private char[,] help1 = new char[10, 10]; // поле атаки первого игрока
    private char[,] help2 = new char[10, 10]; // поле атаки второго игрока
    private int hp1 = 10; // очки здоровья первого игрока
    private int hp2 = 10; // очки здоровья второго игрока
    private string winner; // имя победителя в данной катке
    private string path = "TextFile.txt"; // название файла с таблицей лидеров
    public Battleship() // конструктор класса
    {                       // символ '.' означает пустое пространство
        name1 = "player1";
        name2 = "player2";
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j) // заполняем поля символом '.'
            {
                player1[i, j] = '.';
                player2[i, j] = '.';
                help1[i, j] = '.';
                help2[i, j] = '.';
            }
        }
    }
    public void play() // метод, отвечающий за запуск игры
    {
        Console.WriteLine("Введите имя первого игрока:");
        name1 = Console.ReadLine();
        Console.WriteLine("Введите имя второго игрока:");
        name2 = Console.ReadLine();
        Console.WriteLine($"Игрок {name1} размещает корабли на поле.");
        prepare_bf(ref player1);
        display(ref player1);
        Console.WriteLine($"Игрок {name1} закончил с размещением кораблей. Теперь очередь игрока {name2}.");
        prepare_bf(ref player2);
        display(ref player2);
        Console.WriteLine("Игроки разместили все свои корабли на поле.");
        Console.WriteLine();

        ref string cur_name = ref name1;
        ref char[,] cur_help = ref help1;
        ref char[,] cur_player = ref player2;
        ref int cur_hp = ref hp2;
        while (true)
        {
            if (hp1 == 0 || hp2 == 0) // игра заканчивается после того как хп одного из игроков опустится до 0
            {
                break;
            }
            else // иначе игра продолжается
            {
                Console.WriteLine($"Ход игрока {cur_name}.");
                Console.WriteLine("Введите координаты клетки для атаки в формате: номер_столбца номер_строки.");
                display(ref cur_help);
                string[] s_arr = Console.ReadLine().Split();
                int line = int.Parse(s_arr[1]) - 1;
                int column = (int)(char.Parse(s_arr[0]) - 'а');
                if (damage(ref cur_player, ref cur_help, ref cur_hp, line, column))
                {
                    Console.WriteLine("ПОПАДАНИЕ.");
                }
                else
                {
                    Console.WriteLine("Промах.");
                    if (cur_name == name1)
                    {
                        cur_name = ref name2;
                        cur_help = ref help2;
                        cur_player = ref player1;
                        cur_hp = ref hp1;
                    }
                    else
                    {
                        cur_name = ref name1;
                        cur_help = ref help1;
                        cur_player = ref player2;
                        cur_hp = ref hp2;
                    }
                }
            }
        }
        winner = cur_name;
        Console.WriteLine($"Победил игрок {cur_name}");
        update_leaderboard();

        Console.WriteLine();
        Console.WriteLine("Таблица лидеров:");
        display_leaderboard();
    }

    private bool damage(ref char[,] player, ref char[,] help, ref int hp, int line, int column) // метод, который проверяет и оповещает о попадании, либо промахе
    {
        if (player[line, column] == '#') // символ '#' означает наличие в этой клетке корабля
        {
            help[line, column] = 'x'; // символ 'x' означает попадание
            --hp;
            return true;
        }
        else
        {
            help[line, column] = 'o'; // символ 'o' означает промах 
            return false;
        }
    }

    private void prepare_bf(ref char[,] player) // метод, отвечающий за размещение кораблей на поле
    {
        Console.WriteLine("При размещении корабли не могут касаться друг друга сторонами и углами.");
        Console.WriteLine("У вас есть 3 двухпалубных корабля (они занимают либо 2 клетки по горизонтали, либо 2 клетки по вертикали)\nи 4 однопалубных (они занимают одну клетку).");
        Console.WriteLine("Сначала разместим двухпалубные корабли, а затем однопалубные.");
        Console.WriteLine();
        int count = 0;
        while (true)
        {
            display(ref player);
            Console.WriteLine("Введите координаты первой клетки в формате: номер_столбца номер_строки.");
            string[] s_arr = Console.ReadLine().Split(); // делим строку на две строки по пробелу
            int line1 = int.Parse(s_arr[1]) - 1;
            int column1 = (int)(char.Parse(s_arr[0]) - 'а');
        
            Console.WriteLine("Введите координаты второй клетки в формате: номер_столбца номер_строки.");
            s_arr = Console.ReadLine().Split();
            int line2 = int.Parse(s_arr[1]) - 1;
            int column2 = (int)(char.Parse(s_arr[0]) - 'а');
        
            if (Math.Abs(line1 - line2) + Math.Abs(column1 - column2) == 1 && (player[line1, column1] == '.' && player[line2, column2] == '.'))
            { // проверка на корректную возможность размещения двухпалубного корабля на поле
                if (line1 >= line2 && column1 >= column2) // позиционирование клеток (line1, column1) и (line2, column2) таким образом, что (line1, column1) будет выше и левее, чем (line2, column2)
                {
                    int cur_line = line1;
                    int cur_column = column1;
                    line1 = line2;
                    column1 = column2;
                    line2 = cur_line;
                    column2 = cur_column;
                }

                // определение индексов крайней левой верхней клетки
                int idx_line1;
                int idx_column1;
                if (line1 - 1 >= 0 && column1 - 1 >= 0)
                {
                    idx_line1 = line1 - 1;
                    idx_column1 = column1 - 1;
                }
                else if (line1 - 1 >= 0)
                {
                    idx_line1 = line1 - 1;
                    idx_column1 = column1;
                }
                else if (column1 - 1 >= 0)
                {
                    idx_line1 = line1;
                    idx_column1 = column1 - 1;
                }
                else
                {
                    idx_line1 = line1;
                    idx_column1 = column1;
                }

                // определение индексов крайней правой нижней клетки
                int idx_line2;
                int idx_column2;
                if (line2 + 1 <= 9 && column2 + 1 <= 9)
                {
                    idx_line2 = line2 + 1;
                    idx_column2 = column2 + 1;
                }
                else if (line2 + 1 <= 9)
                {
                    idx_line2 = line2 + 1;
                    idx_column2 = column2;
                }
                else if (column2 + 1 <= 9)
                {
                    idx_line2 = line2;
                    idx_column2 = column2 + 1;
                }
                else
                {
                    idx_line2 = line2;
                    idx_column2 = column2;
                }
                // символ '*' означает, что в эту клетку нельзя помещать корабль по правилам игры
                for (int i = idx_line1; i <= idx_line2; ++i) // заполнение клеток поля символом '*'
                {
                    for (int j = idx_column1; j <= idx_column2; ++j)
                    {
                        player[i, j] = '*';
                    }
                }
                player[line1, column1] = '#';
                player[line2, column2] = '#';
                ++count;
                if (count == 3)
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("Вы неправильно разместили двухпалубный корабль. Попробуйте снова.");
            }
        }
        Console.WriteLine("Все двухпалубные корабли размещены. Теперь перейдём к однопалубным.");
        count = 0;

        while (true)
        {
            display(ref player);
            Console.WriteLine("Введите координаты клетки в формате: номер_столбца номер_строки.");
            string[] s_arr = Console.ReadLine().Split();
            int line = int.Parse(s_arr[1]) - 1;
            int column = (int)(char.Parse(s_arr[0]) - 'а');
            if (player[line, column] == '.')
            {
                int idx_line1;
                int idx_column1;
                if (line - 1 >= 0 && column - 1 >= 0)
                {
                    idx_line1 = line - 1;
                    idx_column1 = column - 1;
                }
                else if (line - 1 >= 0)
                {
                    idx_line1 = line - 1;
                    idx_column1 = column;
                }
                else if (column - 1 >= 0)
                {
                    idx_line1 = line;
                    idx_column1 = column - 1;
                }
                else
                {
                    idx_line1 = line;
                    idx_column1 = column;
                }

                int idx_line2;
                int idx_column2;
                if (line + 1 <= 9 && column + 1 <= 9)
                {
                    idx_line2 = line + 1;
                    idx_column2 = column + 1;
                }
                else if (line + 1 <= 9)
                {
                    idx_line2 = line + 1;
                    idx_column2 = column;
                }
                else if (column + 1 <= 9)
                {
                    idx_line2 = line;
                    idx_column2 = column + 1;
                }
                else
                {
                    idx_line2 = line;
                    idx_column2 = column;
                }
                for (int i = idx_line1; i <= idx_line2; ++i)
                {
                    for (int j = idx_column1; j <= idx_column2; ++j)
                    {
                        player[i, j] = '*';
                    }
                }
                player[line, column] = '#';
                ++count;
                if (count == 4)
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("Вы неправильно разместили однопалубный корабль. Попробуйте снова.");
            }
        }
    }

    private void display(ref char[,] player) // метод, выводящий на экран размещённые на поле корабли
    {
        Console.Write("   ");
        char symb = 'а';
        for (int i = 0; i < 10; ++i)
        {
            Console.Write((char)(symb + i) + "  ");
        }
        Console.WriteLine();

        for (int i = 0; i < 10; ++i)
        {
            if (i == 9)
            {
                Console.Write((i+1) + " ");
            }
            else
            {
                Console.Write((i + 1) + "  ");
            }
            for (int j = 0; j < 10; ++j)
            {
                Console.Write(player[i,j].ToString() + "  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private void display_game_state() // вывести состояние игры
    {
        Console.WriteLine($"Поле игрока {name1}:");
        display(ref player1);

        Console.WriteLine($"Поле игрока {name2}:");
        display(ref player2);
    }

    private void update_leaderboard() // обновлене таблицы лидеров с учётом проведённого матча
    {
        int count = System.IO.File.ReadAllLines(path).Length;
        Player[] arr = new Player[count];
        using (StreamReader reader = new StreamReader(path))
        {
            string? line;
            int idx = 0;
            bool flag = false;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s_arr = line.Split();
                if (s_arr[0] == winner)
                {
                    flag = true;
                }
                arr[idx] = new Player(s_arr[0], int.Parse(s_arr[1]));
                idx++;
            }
            if (flag)
            {
                for (int i = 0; i < count; ++i)
                {
                    if (arr[i].name == winner)
                    {
                        arr[i].score += 5;
                        break;
                    }
                }
            }
            else
            {
                Array.Resize(ref arr, arr.Length + 1);
                arr[arr.Length - 1] = new Player(winner, 5);
                count++;
            }
        }

        Array.Sort(arr, (p1, p2) => p1.score.CompareTo(p2.score)); // сортировка по возрастанию

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            for (int i = count-1; i >= 0; --i)
            {
                 writer.WriteLine(arr[i].get_string());
            }
        }
    }

    private void display_leaderboard() // вывод на экран таблицы лидеров
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string? line;
            int idx = 0;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                idx++;
            }
        }
    }
}