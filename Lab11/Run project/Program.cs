
using System.Collections;
using System.Diagnostics;
using InputLibrary;
using GameLibrary;
using System.Runtime.CompilerServices;

static Queue CloneQueue(Queue queue)
{
    Queue newqueue = new Queue();
    foreach (var o in queue)
    {
        if (o is ICloneable cloneable)
        {
            newqueue.Enqueue(cloneable.Clone());
        }
        else newqueue.Enqueue(null);
    }
    return newqueue;
}
static List<T> CloneList<T>(List<T> list) where T : ICloneable
{
    List<T> newlist = new List<T>();
    foreach (T o in list) newlist.Add(o);
    return newlist;
}
Type getType()
{
    Console.WriteLine("Input type of game (any / table / video / vr)");
    return ConsoleInput.MEAN_PART.transform(
        Transforms.AddMessage(
            Transforms.Dict<string, Type>(
                new Dictionary<string, Type>{
                    {"any", typeof(Game)},
                    {"table", typeof(TableGame)},
                    {"video", typeof(VideoGame)},
                    {"vr", typeof(VRGame)}
                }
            ),
            "Type must be any, table, video or vr"
        )
    ).get();
}
{
    //Task 1
    Console.WriteLine("Task1");
    Queue queue = new Queue();
    Console.WriteLine("Input command (stop/add/remove/show/count/min/max/clone/sort)");
    bool stop = false;
    do
    {
        switch (ConsoleInput.MEAN_PART.check(Checks.Into(["stop", "add", "remove", "show", "count", "min", "max", "clone", "sort"]), "Command must be stop, add, remove, show, count, min, max, clone or sort").get())
        {
            case "stop":
                stop = true;
                break;
            case "add":
                Type type = getType();
                Game game = (Game)type.GetConstructor(new Type[0]).Invoke(new object[0]);
                Console.WriteLine("Input method of creation (random / input)");
                ConsoleInput.MEAN_PART.transform(
                    Transforms.AddMessage(
                        Transforms.Dict<string, Action>(
                            new Dictionary<string, Action>{
                            {"random", () => game.RandomInit()},
                            {"input", () => game.Init()}
                            }
                        ),
                        "Method of creation must be input or random"
                    )
                ).get()();
                queue.Enqueue(game);
                break;
            case "remove":
                if (queue.Count == 0) Console.WriteLine("Cant remove from empty stack.");
                else
                {
                    Console.WriteLine("Removed element:");
                    ((Game)queue.Dequeue()).Show();
                }
                break;
            case "show":
                if (queue.Count == 0) Console.WriteLine("Queue is empty");
                else
                {
                    foreach (object o in queue)
                    {
                        ((Game)o).Show();
                    }
                }
                break;
            case "count":
                Type type_c = getType();
                int count = 0;
                foreach (var o in queue)
                {
                    if (o.GetType().IsSubclassOf(type_c)) count++;
                }
                Console.WriteLine($"There is {count} games of this type.");
                break;
            case "min":
                Game? mn = null;
                foreach (var o in queue)
                {
                    if (mn == null || (mn.MinPlayers > ((Game)o).MinPlayers))
                    {
                        mn = (Game)o;
                    }
                }
                if (mn != null)
                    Console.WriteLine($"Minimal necessary count of players is {mn.MinPlayers} (for game named {mn.Name})");
                else
                    Console.WriteLine("Stack is empty");
                break;
            case "max":
                Game? mx = null;
                foreach (var o in queue)
                {
                    if (mx == null || (mx.MaxPlayers < ((Game)o).MaxPlayers))
                    {
                        mx = (Game)o;
                    }
                }
                if (mx != null)
                    Console.WriteLine($"Maximal aliviable count of players is {mx.MaxPlayers} (for game named {mx.Name})");
                else
                    Console.WriteLine("Stack is empty");
                break;
            case "clone":
                queue = CloneQueue(queue);
                break;
            case "sort":
                object[] array = new object[queue.Count];
                queue.CopyTo(array, 0);
                Array.Sort(array, (a, b) => ((Game)a).Name.CompareTo(((Game)b).Name));
                queue = new Queue(array);
                break;
        }
    } while (!stop);
}
{
    //Task 2
    Console.WriteLine("Task2");
    List<Game> list = new List<Game>();
    Console.WriteLine("Input command (stop/add/remove/show/count/min/max/sort/clone)");
    bool stop = false;
    do
    {
        switch (ConsoleInput.MEAN_PART.check(Checks.Into(["stop", "add", "show", "remove", "count", "min", "max", "clone", "sort"]), "Command must be stop, add, remove, show, count, min, max, clone or sort").get())
        {
            case "stop":
                stop = true;
                break;
            case "add":
                Type type = getType();
                Game game = (Game)type.GetConstructor(new Type[0]).Invoke(new object[0]);
                Console.WriteLine("Input method of creation (random / input)");
                ConsoleInput.MEAN_PART.transform(
                    Transforms.AddMessage(
                        Transforms.Dict<string, Action>(
                            new Dictionary<string, Action>{
                            {"random", () => game.RandomInit()},
                            {"input", () => game.Init()}
                            }
                        ),
                        "Method of creation must be input or random"
                    )
                ).get()();
                list.Add(game);
                break;
            case "remove":
                if (list.Count == 0) Console.WriteLine("Cant remove from empty stack.");
                else
                {
                    int index = ConsoleInput.NATURAL.transform(ConsoleInput.NoMore(list.Count)).get() - 1;
                    Game el = list[index];
                    list.RemoveAt(index);
                    Console.WriteLine("Removed element:");
                    el.Show();
                }
                break;
            case "show":
                if (list.Count == 0) Console.WriteLine("List is empty");
                else
                {
                    foreach (object o in list)
                    {
                        ((Game)o).Show();
                    }
                }
                break;
            case "count":
                Type type_c = getType();
                int count = 0;
                foreach (var o in list)
                {
                    if (o.GetType().IsSubclassOf(type_c)) count++;
                }
                Console.WriteLine($"There is {count} games of this type.");
                break;
            case "min":
                Game? mn = null;
                foreach (var o in list)
                {
                    if (mn == null || (mn.MinPlayers > ((Game)o).MinPlayers))
                    {
                        mn = (Game)o;
                    }
                }
                if (mn != null)
                    Console.WriteLine($"Minimal necessary count of players is {mn.MinPlayers} (for game named {mn.Name})");
                else
                    Console.WriteLine("Stack is empty");
                break;
            case "max":
                Game? mx = null;
                foreach (var o in list)
                {
                    if (mx == null || (mx.MaxPlayers < ((Game)o).MaxPlayers))
                    {
                        mx = (Game)o;
                    }
                }
                if (mx != null)
                    Console.WriteLine($"Maximal aliviable count of players is {mx.MaxPlayers} (for game named {mx.Name})");
                else
                    Console.WriteLine("Stack is empty");
                break;
            case "clone":
                list = CloneList(list);
                break;
            case "sort":
                list.Sort((a, b) => a.Name.CompareTo(b.Name));
                break;
        }
    } while (!stop);
}
{
    //Task 3
    Console.WriteLine("Task 3");
    Console.WriteLine("Input count:");
    int count = ConsoleInput.NATURAL.get();
    Console.WriteLine("Input repeats:");
    int rep =ConsoleInput.NATURAL.get();
    TestCollection 
        testStart = new TestCollection(count, rep),
        testCenter = new TestCollection(count, rep),
        testEnd = new TestCollection(count, rep),
        testAll = new TestCollection(count, rep);
    Console.WriteLine("Input runs:");
    int runs = ConsoleInput.NATURAL.get();
    for (int i = 0; i < runs; i++)
    {
        testCenter.TestCenter();
        testEnd.TestLast();
        testStart.TestStart();
        testAll.Test();
        foreach(var coll in new TestCollection[]{testStart, testCenter, testEnd, testAll}) {
            coll.Recreate();
        }
    }
    Console.WriteLine("Start:");
    testStart.Show();
    Console.WriteLine("Center:");
    testCenter.Show();
    Console.WriteLine("Last:");
    testEnd.Show();
    Console.WriteLine("All:");
    testAll.Show();
}