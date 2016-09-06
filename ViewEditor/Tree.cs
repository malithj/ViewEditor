using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewEditor
{
    class Tree
    {
        public Tree(Tree left, Tree right)
        {
            treeLeft = left;
            treeRight = right;
        }
        public Tree(int number)
        {
            Value = number;
            treeLeft = null;
            treeRight = null;
        }
        public Tree treeLeft
        {
            get;
            set;
        }
        public Tree treeRight
        {
            get;
            set;
        }
        public int Value
        {
            get;
            set;
        }
    }
}
