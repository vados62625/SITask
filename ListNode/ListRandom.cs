using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SITask.ListNode
{
    internal class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;
        public void Add(string data)
        {
            //data = data.Replace("\n", "\\n")
            //    .Replace("\t", "\\t");
            ListNode node = new ListNode() { Data = data };
            if (Head == null)
            {
                Head = Tail = node;
                Head.Next = Tail;
                Tail.Previous = Head;
            }
            else
            {
                Tail.Next = node;
                node.Previous = Tail;
                Tail = node;
            }
            Count++;
        }
        private List<ListNode> ToList()
        {
            List<ListNode> list = new List<ListNode>();
            Loop(list.Add);
            return list;
        }
        private void Loop (Action<ListNode> method)
        {
            ListNode node = Head;
            while (node != null)
            {
                method(node);
                node = node.Next;
            }
        }
        private void SetRandom(ListNode node)
        {
            Random random = new Random();
            int index = random.Next(0, Count);
            node.Random = ToList()[index];
        }
        public void SetRandoms()
        {
            Loop(SetRandom);
        }
        public override string ToString()
        {
            string listToString = string.Empty;

            var list = ToList();

            foreach (ListNode node in list)
            {
                string nodeData = node.Data;
                listToString += $"{nodeData}\t{list.IndexOf(node.Random)}\n";
            }

            return listToString.Trim('\n');
        }
        public string Print()
        {
            string listToPrint = string.Empty;

            var list = ToList();

            foreach (ListNode node in list)
            {
                string nodeData = node.Data;
                string nodeRandomData = node.Random != null ? node.Random.Data : "no data";
                listToPrint += $"Node data: {nodeData}\tRandomNode data: {nodeRandomData}\n";
            }

            return listToPrint;
        }

        public void Serialize(Stream s)
        {
            if (!s.CanWrite)
            {
                throw new Exception("Cannot write to file");
            }

            byte[] buffer = Encoding.Default.GetBytes(ToString());
            s.Write(buffer, 0, buffer.Length);
        }

        public void Deserialize(Stream s)
        {
            if (!s.CanRead)
            {
                throw new Exception("Cannot open file");
            }

            //reading
            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            string data = Encoding.Default.GetString(buffer);

            //drop list
            Head = Tail = null;
            Count = 0;

            //restore list
            string[] nodes = data.Split('\n');
            List<string> randIndexes = new List<string>();

            foreach (string node in nodes)
            {
                Add(node.Split("\t")[0]);
                randIndexes.Add(node.Split("\t")[1]);
            }

            List<ListNode> list = ToList();
            for (int i = 0; i < list.Count; i++)
            {
                int randIndex = Convert.ToInt32(randIndexes[i]);
                list[i].Random = list[randIndex];
            }
        }
    }
}
