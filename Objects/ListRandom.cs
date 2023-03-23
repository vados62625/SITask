using System.Text;

namespace SITask.ListNode
{
    internal class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;
        public void Add(string data)
        {
            var node = new ListNode() { Data = data };
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
            var list = new List<ListNode>();
            Loop(list.Add);
            return list;
        }
        private void Loop (Action<ListNode> method)
        {
            var node = Head;
            while (node != null)
            {
                method(node);
                node = node.Next;
            }
        }
        private void SetRandom(ListNode node)
        {
            var random = new Random();
            var index = random.Next(0, Count);
            node.Random = ToList()[index];
        }
        public void SetRandoms()
        {
            Loop(SetRandom);
        }
        public override string ToString()
        {
            var listToString = string.Empty;

            var list = ToList();

            foreach (var node in list)
            {
                var nodeData = node.Data;
                var nodeRandomIndex = node.Random != null ? list.IndexOf(node.Random) : -1;
                listToString += $"{nodeData}\t{nodeRandomIndex}\n";
            }

            return listToString.Trim('\n');
        }
        public string Print()
        {
            var listToPrint = string.Empty;

            var list = ToList();

            foreach (var node in list)
            {
                var nodeData = node.Data;
                var nodeRandomData = node.Random != null ? node.Random.Data : "no data";
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

            //clear file
            s.SetLength(0);

            //writing
            var buffer = Encoding.Default.GetBytes(ToString());
            s.Write(buffer, 0, buffer.Length);
        }

        public void Deserialize(Stream s)
        {
            if (!s.CanRead)
            {
                throw new Exception("Cannot open file");
            }

            //reading
            var buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            var data = Encoding.Default.GetString(buffer);

            //drop list
            Head = Tail = null;
            Count = 0;

            //restore list
            var nodes = data.Split('\n');
            var randIndexes = new List<string>();

            foreach (var node in nodes)
            {
                Add(node.Split("\t")[0]);
                randIndexes.Add(node.Split("\t")[1]);
            }

            var list = ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var randIndex = Convert.ToInt32(randIndexes[i]);
                if (randIndex > -1)
                {
                    list[i].Random = list[randIndex];
                }
            }
        }
    }
}
