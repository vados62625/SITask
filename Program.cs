using SITask.ListNode;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text;

ListRandom list= new ListRandom();
for (int i=1; i<=10; i++)
{
    list.Add(i.ToString());
    //list.Add(Console.ReadLine() ?? string.Empty);
}
list.SetRandoms();
Console.WriteLine(list.Print());
using (FileStream fstream = new FileStream("serialize.txt", FileMode.OpenOrCreate))
{
    list.Serialize(fstream);
    Console.WriteLine("Текст записан в файл");
}
ListRandom newList = new ListRandom();
using (FileStream fstream = File.OpenRead("serialize.txt"))
{
    newList.Deserialize(fstream);
    Console.WriteLine($"Структура списка восстановлена\n{newList.Print()}");
}