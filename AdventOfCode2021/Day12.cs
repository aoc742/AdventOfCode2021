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
        public List<Node> Nodes = new List<Node>();

        public void Start()
        {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/../../Resources/Day12Input.txt");
            string[] separators = { "-" };
            List<string[]> assortedLines = lines.ToList().Select(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries)).ToList();

            foreach(var line in assortedLines)
            {
                Nodes.Add(new Node(line[0]));
                Nodes.Add(new Node(line[1]));
                Nodes[Nodes.Count()-2].AddConnection(Nodes[Nodes.Count()-1]);
                Nodes[Nodes.Count()-1].AddConnection(Nodes[Nodes.Count()-2]);
            }
            Nodes = Nodes.Distinct().ToList();

            // Goal: Get list of all possible paths that can be traversed. 
            // 1. A path must start at "start" (cannot go back to start after leaving)
            // 2. A path must end at "end" (once you hit "end" the path is stopped)
            // 3. small nodes (lowercase letters) can only be touched once per path
            var startNode = this.Nodes.First(node => node.Name == "start");
            var endNode = this.Nodes.First(node => node.Name == "end");

            // Begin
            LinkedList<Node> visited = new LinkedList<Node>();
            this.BreadthFirst(this.Nodes, visited);

        }

        public void BreadthFirst(List<Node> graph, LinkedList<Node> visited)
        {
            LinkedList<Node> nodes = new LinkedList<Node>(visited.Last().Connections.ToArray);

            // Examine adjacent nodes
            foreach (string node in nodes)
            {
                if (visited.Contains(node))
                {
                    continue;
                }

                if (node.Equals(endNode))
                {
                    visited.AddLast(node);

                    printPath(visited);

                    visited.RemoveLast();

                    break;
                }
            }

            // In breadth-first, recursion needs to come after visiting adjacent nodes
            foreach (String node in nodes)
            {
                if (visited.Contains(node) || node.Equals(endNode))
                {
                    continue;
                }

                visited.AddLast(node);

                // Recursion
                BreadthFirst(graph, visited);

                visited.RemoveLast();
            }
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
