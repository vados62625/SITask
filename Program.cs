using SITask.ListNode;


var list= new ListRandom();
for (int i=1; i<=10; i++)
{
    list.Add(i.ToString());
    //list.Add(Console.ReadLine() ?? string.Empty);
}

list.SetRandoms();

Console.WriteLine(list.Print());

using (FileStream fstream = new FileStream("listRandom.txt", FileMode.OpenOrCreate))
{
    try
    {
        list.Serialize(fstream);
        Console.WriteLine("ListRandom object was successfully saved to file.\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

var newList = new ListRandom();

using (FileStream fstream = File.OpenRead("listRandom.txt"))
{
    try
    {
        newList.Deserialize(fstream);
        Console.WriteLine($"ListRandom object was successfully restored from file.\n\n{newList.Print()}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}