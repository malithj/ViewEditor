using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewEditor
{
    class Node
    {
        public Node(Tree t, int freq)
        {
            tree = t;
            frequency = freq;
        }
        private Node next
        {
            get;
            set;
        }
        public Tree tree
        {
            get;
            set;
        }
        public int frequency
        {
            get;
            set;
        }
    }
}
