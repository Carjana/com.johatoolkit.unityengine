#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JohaToolkit.Unity.DataStructures
{
    public class BinaryTree<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Value;
            public Node? ParentNode = null;
            public Node? LeftChild = null;
            public Node? RightChild = null;
            public Node(T value) => Value = value;
        }
        
        private Node? _root = null;

        public int Count { get; private set; }

        public void Add(T newValue)
        {
            if(_root == null)
            {
                _root = new Node(newValue);
            }
            else
            {
                AddRecursive(_root, newValue);
            }

            Count++;
        }

        private void AddRecursive(Node nodeToCheck, T newValue)
        {
            if(nodeToCheck.Value.CompareTo(newValue) > 0)
            {
                // check Left
                if (nodeToCheck.LeftChild == null)
                {
                    // found slot
                    nodeToCheck.LeftChild = new Node(newValue);
                    nodeToCheck.LeftChild.ParentNode = nodeToCheck;
                }
                else
                {
                    // not found, go to left
                    AddRecursive(nodeToCheck.LeftChild, newValue);
                }
            }
            else
            {
                // check right
                if (nodeToCheck.RightChild == null)
                {
                    // found slot
                    nodeToCheck.RightChild = new Node(newValue);
                    nodeToCheck.RightChild.ParentNode = nodeToCheck;
                }
                else
                {
                    // not found, go to right
                    AddRecursive(nodeToCheck.RightChild, newValue);
                }
                
            }
        }

        public T[]? ToArray()
        {
            if(Count == 0)
            {
                return null;
            }

            List<T> allNodes = new List<T>();
            
            Debug.Assert(_root != null, nameof(_root) + " != null");
            ToArrayRecursive(_root, allNodes);

            return allNodes.ToArray();
        }

        private void ToArrayRecursive(Node node, List<T> nodeList)
        {
            if (node.LeftChild != null)
            {
                // has left Child
                ToArrayRecursive(node.LeftChild, nodeList);
            }

            nodeList.Add(node.Value);

            if (node.RightChild != null)
            {
                // has right Child
                ToArrayRecursive(node.RightChild, nodeList);
            }
        }

        public bool Remove(T value)
        {
            // ganz rechte Node wird neue Node (wenn ich Node mittendrin lösche)

            if(Count == 0)
            {
                // Tree is empty
                return false;
            }

            // search element to delete
            Node? nodeToRemove = GetNode(value);
            if(nodeToRemove != null)
            {
                // Node exists

                // Check root
                if (Count == 1)
                {
                    // Only Root exists;
                    _root = null;
                }// check leaf
                else if (nodeToRemove.RightChild == null && nodeToRemove.LeftChild == null)
                {
                    // Parent not Null because left child is null, right child is null AND Count > 1!!

                    Debug.Assert(nodeToRemove.ParentNode != null, "nodeToRemove.ParentNode != null");
                    if (nodeToRemove.ParentNode.Value.CompareTo(nodeToRemove.Value) > 0)
                    {
                        // is Left child
                        nodeToRemove.ParentNode.LeftChild = null;
                    }
                    else
                    {
                        nodeToRemove.ParentNode.RightChild = null;
                    }
                }
                else
                {
                    // check, if we need most left/most right node -> node value closest to parent Node
                    // Parent Check!!!
                    if(nodeToRemove.ParentNode == null)
                    {
                        // remove root -> set new root
                        if(nodeToRemove.RightChild == null)
                        {
                            // tree is only left side
                            Debug.Assert(nodeToRemove.LeftChild != null, "nodeToRemove.LeftChild != null");
                            Node rightLeaf = FindRightLeaf(nodeToRemove.LeftChild);
                            nodeToRemove.Value = rightLeaf.Value;
                            if (rightLeaf.LeftChild != null)
                            {
                                Debug.Assert(rightLeaf.ParentNode != null, "rightLeaf.ParentNode != null");
                                rightLeaf.ParentNode.RightChild = rightLeaf.LeftChild;
                            }
                            else
                            {
                                Debug.Assert(rightLeaf.ParentNode != null, "rightLeaf.ParentNode != null");
                                rightLeaf.ParentNode.RightChild = null;
                            }
                        }
                        else
                        {
                            // only right child
                            // tree is only right side
                            Node leftLeaf = FindLeftLeaf(nodeToRemove.RightChild);
                            nodeToRemove.Value = leftLeaf.Value;
                            if (leftLeaf.RightChild != null)
                            {
                                Debug.Assert(leftLeaf.ParentNode != null, "leftLeaf.ParentNode != null");
                                leftLeaf.ParentNode.LeftChild = leftLeaf.RightChild;
                            }
                            else
                            {
                                Debug.Assert(leftLeaf.ParentNode != null, "leftLeaf.ParentNode != null");
                                leftLeaf.ParentNode.LeftChild = null;
                            }
                        }
                    }
                    else if(nodeToRemove.ParentNode.Value.CompareTo(nodeToRemove.Value) > 0)
                    {
                        // Node To Remove Value is smaller than parent Value
                        // -> Want node most right
                        Node rightLeaf = FindRightLeaf(nodeToRemove);
                        nodeToRemove.Value = rightLeaf.Value;
                        if(rightLeaf.LeftChild != null)
                        {
                            Debug.Assert(rightLeaf.ParentNode != null, "rightLeaf.ParentNode != null");
                            rightLeaf.ParentNode.RightChild = rightLeaf.LeftChild;
                        }
                        else
                        {
                            Debug.Assert(rightLeaf.ParentNode != null, "rightLeaf.ParentNode != null");
                            rightLeaf.ParentNode.RightChild = null;
                        }
                    }
                    else
                    {
                        // NodeToRemove Value is greater (or equal) than parent Value
                        // -> Want node most left
                        Node leftLeaf = FindLeftLeaf(nodeToRemove);
                        nodeToRemove.Value = leftLeaf.Value;
                        if (leftLeaf.RightChild != null)
                        {
                            Debug.Assert(leftLeaf.ParentNode != null, "leftLeaf.ParentNode != null");
                            leftLeaf.ParentNode.LeftChild = leftLeaf.RightChild;
                        }
                        else
                        {
                            Debug.Assert(leftLeaf.ParentNode != null, "leftLeaf.ParentNode != null");
                            leftLeaf.ParentNode.LeftChild = null;
                        }
                    }
                }

                Count--;
                return true;
            }
            else
            {
                // Node doesn't exist
                return false;
            }
        }

        private Node FindRightLeaf(Node startNode)
        {
            Node? curr = startNode;
            while (curr.RightChild != null)
            {
                curr = curr.RightChild;
            }
            return curr;
        }

        private Node FindLeftLeaf(Node startNode)
        {
            Node? curr = startNode;
            while (curr.LeftChild != null)
            {
                curr = curr.LeftChild;
            }
            return curr;
        }

        public bool NodeValueExists(T value)
        {
            if(Count == 0)
            {
                return false;
            }

            Debug.Assert(_root != null, nameof(_root) + " != null");
            return SearchNodeRecursive(_root, value) != null;
        }

        private Node? GetNode(T value)
        {
            if (Count == 0)
            {
                return null;
            }

            Debug.Assert(_root != null, nameof(_root) + " != null");
            return SearchNodeRecursive(_root, value);
        }

        private Node? SearchNodeRecursive(Node searchNode, T value)
        {
            int comparison = searchNode.Value.CompareTo(value);
            
            if (comparison == 0)
            {
                // value Found
                return searchNode;
            }
            else if (comparison > 0)
            {
                // search Left
                if(searchNode.LeftChild == null)
                {
                    // leaf -> not found
                    return null;
                }

                return SearchNodeRecursive(searchNode.LeftChild, value);
            }
            else
            {
                // search right
                if(searchNode.RightChild == null)
                {
                    // leaf -> not found
                    return null;
                }

                return SearchNodeRecursive(searchNode.RightChild, value);
            }
        }
    }
}
