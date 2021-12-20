using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal class Day12
    {
        private Node _startNode;
        private Node _endNode;
        public List<Node> Nodes = new List<Node>();
        List<LinkedList<Node>> Paths = new List<LinkedList<Node>>();
        
        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day12Input.txt");
            string[] separators = { "-" };
            List<string[]> assortedLines = lines.ToList().Select(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries)).ToList();

            foreach(var line in assortedLines)
            {
                if (Nodes.FirstOrDefault(n => n.Name == line[0]) == null)
                {
                    Nodes.Add(new Node(line[0]));
                }
                if (Nodes.FirstOrDefault(n => n.Name == line[1]) == null)
                {
                    Nodes.Add(new Node(line[1]));
                }
                var first = Nodes.First(n => n.Name == line[0]);
                var second = Nodes.First(n => n.Name == line[1]);
                first.AddConnection(second);
                second.AddConnection(first);
            }
            Nodes = Nodes.Distinct().ToList();

            // Goal: Get list of all possible paths that can be traversed. 
            // 1. A path must start at "start" (cannot go back to start after leaving)
            // 2. A path must end at "end" (once you hit "end" the path is stopped)
            // 3. small nodes (lowercase letters) can only be touched once per path
            this._startNode = this.Nodes.First(node => node.Name == "start");
            this._endNode = this.Nodes.First(node => node.Name == "end");

            // Begin
            LinkedList<Node> visited = new LinkedList<Node>();
            visited.AddFirst(this._startNode);
            this.BreadthFirst(this.Nodes, visited);

        }

        public void BreadthFirst(List<Node> graph, LinkedList<Node> visited)
        {
            LinkedList<Node> nodes = new LinkedList<Node>(visited.Last().Connections.ToArray());

            // In breadth-first, recursion needs to come after visiting adjacent nodes
            foreach (Node node in nodes)
            {
                // stopping criteria
                if (node.Equals(this._startNode) || node.Equals(this._endNode))
                {
                    continue;
                }
                if (visited.Contains(node) && char.IsLower(node.Name[0]))
                {
                    continue;
                }

                visited.AddLast(node);

                // Recursion
                BreadthFirst(graph, visited);

                visited.RemoveLast();
            }

            this.Paths.Add(visited);
        }

        public class Node
        {
            public string Name { get; private set; }

            public HashSet<Node> Connections { get; private set; } = new HashSet<Node>();

            public Node(string name)
            {
                this.Name = name;
            }

            public void AddConnection(Node node)
            {
                Connections.Add(node);
            }
        }
    }
}
