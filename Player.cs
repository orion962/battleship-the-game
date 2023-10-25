struct Player // структура описывающая игрока, есть 2 поля (имя и счёт)
{
    public string name = "John";
    public int score = 16;
    public Player(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
    public string get_string()
    {
        return $"{name} {score}";
    }
}
