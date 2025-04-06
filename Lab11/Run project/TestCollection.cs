using System.Diagnostics;
using GameLibrary;

class TestCollection
{
    int repeats;
    TableGame[] array;
    Queue<Game> gameQueue = new Queue<Game>();
    Queue<string> stringQueue = new Queue<string>();
    SortedDictionary<Game, TableGame> gameDictionary = new SortedDictionary<Game, TableGame>();
    SortedDictionary<string, TableGame> stringDictionary = new SortedDictionary<string, TableGame>();
    private long
        sgq = 0,
        ssq = 0,
        sgdk = 0,
        ssdk = 0,
        sgdv = 0,
        ssdv = 0;
    int refinden = 0;

    public TestCollection(int size = 1000, int repeat = 1)
    {
        repeats = repeat;
        Recreate(size);
    }
    public void Recreate(int size = 1000)
    {
        array = new TableGame[size];
        gameQueue = new Queue<Game>();
        stringQueue = new Queue<string>();
        gameDictionary = new SortedDictionary<Game, TableGame>();
        stringDictionary = new SortedDictionary<string, TableGame>();
        for (int i = 0; i < size; i++)
        {
            TableGame tableGame = new TableGame();
            array[i] = tableGame;
            tableGame.RandomInit();
            gameQueue.Enqueue((TableGame)tableGame.Clone());
            stringQueue.Enqueue(tableGame.Name);
            gameDictionary[tableGame.GetBase()] = (TableGame)tableGame.Clone();
            stringDictionary[tableGame.Name] = (TableGame)tableGame.Clone();
        }
    }
    public void Find()
    {
        Console.WriteLine("First:");
        Find(array[0]);
        Console.WriteLine("Center:");
        Find(array[array.Length / 2]);
        Console.WriteLine("Last:");
        Find(array[^1]);
    }
    public void Find(TableGame game)
    {
        sgq += Find(game, gameQueue).ElapsedTicks / repeats;
        ssq += Find(game.Name, stringQueue).ElapsedTicks / repeats;
        sgdk += FindKey(game.GetBase(), gameDictionary).ElapsedTicks / repeats;
        ssdk += FindKey(game.Name, stringDictionary).ElapsedTicks / repeats;
        sgdv += FindVal(game, gameDictionary).ElapsedTicks / repeats;
        ssdv += FindVal(game, stringDictionary).ElapsedTicks / repeats;
        refinden++;
    }
    public void Show()
    {

        Console.WriteLine(
            $"""
            Find in game queue taken {sgq / refinden}.
            Find in string queue taken {ssq / refinden}.
            Find key in game dict taken {sgdk / refinden}.
            Find key in string dict taken {ssdk / refinden}.
            Find value in game dict taken {sgdv / refinden}.
            Find value in string dict taken {ssdv / refinden}.
            """
        );
    }
    public Stopwatch Find<T>(T obj, Queue<T> queue)
    {
        Stopwatch stopwatch = new Stopwatch();
        bool found = false;
        stopwatch.Start();
        for (int i = 0; i < repeats; i++) found = queue.Contains(obj);
        stopwatch.Stop();
        if (!found) Console.WriteLine("Object not found!");
        return stopwatch;
    }
    public Stopwatch FindKey<T>(T key, SortedDictionary<T, TableGame> dict)
    {
        Stopwatch stopwatch = new Stopwatch();
        bool found = false;
        stopwatch.Start();
        for (int i = 0; i < repeats; i++) found = dict.ContainsKey(key);
        stopwatch.Stop();
        if (!found) Console.WriteLine("Object not found!");
        return stopwatch;
    }
    public Stopwatch FindVal<T>(TableGame val, SortedDictionary<T, TableGame> dict)
    {
        Stopwatch stopwatch = new Stopwatch();
        bool found = false;
        stopwatch.Start();
        for (int i = 0; i < repeats; i++) found = dict.ContainsValue(val);
        stopwatch.Stop();
        if (!found) Console.WriteLine("Object not found!");
        return stopwatch;
    }
}