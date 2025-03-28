#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace JohaToolkit.UnityEngine.DataStructures.Lists
{
    public class SimpleDoubleList<T> : IEnumerable
    {
        private class ListItem
        {
            public T Value;
            public ListItem? Next = null;
            public ListItem? Previous = null;
            public ListItem(T value) => Value = value;
        }

        private ListItem? _root = null;

        public int Count { private set; get; }

        public T this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public void Add(T newItem)
        {
            if (_root == null)
            {
                _root = new ListItem(newItem);
            }
            else
            {
                // Go to end of List
                ListItem lastListEntry = GetListItem(Count - 1);
                
                ListItem newListEntry = new ListItem(newItem);
                lastListEntry.Next = newListEntry;
                newListEntry.Previous = lastListEntry;
            }

            Count++;
        }

        public void RemoveAt(int index)
        {
            if (!IsIndexInRange(index))
            {
                throw new IndexOutOfRangeException();
            }

            ListItem itemToRemove = GetListItem(index);

            if (index == Count - 1)
            {
                // Remove Last Element
                GetListItem(Count - 1).Next = null;
            }
            else if (index == 0)
            {
                //Remove Root
                Debug.Assert(_root != null, nameof(_root) + " != null");
                if (_root.Next == null)
                {
                    _root = null;
                }
                else
                {
                    ListItem next = _root.Next;
                    next.Previous = null;
                    _root = next;
                }
            }
            else
            {
                Debug.Assert(itemToRemove.Next != null, "itemToRemove.Next != null");
                Debug.Assert(itemToRemove.Previous != null, "itemToRemove.Previous != null");
                ListItem next = itemToRemove.Next;
                ListItem previous = itemToRemove.Previous;
                previous.Next = next;
                next.Previous = previous;
            }

            Count--;
        }

        public void Insert(int index, T value)
        {
            if (!IsIndexInRange(index))
            {
                throw new IndexOutOfRangeException();
            }

            ListItem newItem = new ListItem(value);

            if (index == 0)
            {
                // Insert at Root
                if(_root == null)
                {
                    // No root
                    _root = newItem;
                }
                else
                {
                    // root available
                    newItem.Next = _root;
                    _root.Previous = newItem;
                    _root = newItem;
                }
            }
            else
            {
                // Insert somewhere else
                ListItem previousItem = GetListItem(index-1);

                Debug.Assert(previousItem.Next != null, "previousItem.Next != null");
                ListItem nextItem = previousItem.Next;

                newItem.Next = nextItem;
                newItem.Previous = previousItem;
                nextItem.Previous = newItem;
                previousItem.Next = newItem;
            }

            Count++;
        }

        public void Swap(int indexA, int indexB)
        {
            if (!IsIndexInRange(indexA) || !IsIndexInRange(indexB))
            {
                throw new IndexOutOfRangeException();
            }

            ListItem itemA = GetListItem(indexA);
            ListItem itemB = GetListItem(indexB);

            T tempValue = itemA.Value;
            itemA.Value = itemB.Value;
            itemB.Value = tempValue;
        }

        private ListItem GetListItem(int index)
        {
            if (_root == null || !IsIndexInRange(index))
            {
                throw new IndexOutOfRangeException();
            }

            ListItem item = _root;

            for (int i = 0; i < index; i++)
            {
                item = item.Next ?? throw new IndexOutOfRangeException();
            }

            return item;
        }

        private T Get(int index)
        {
            if (!IsIndexInRange(index))
            {
                throw new IndexOutOfRangeException();
            }
            return GetListItem(index).Value;
        }

        private void Set(int index, T item)
        {
            if (!IsIndexInRange(index))
            {
                throw new IndexOutOfRangeException();
            }
            GetListItem(index).Value = item;
        }

        private bool IsIndexInRange(int index) => !(index < 0 || index >= Count);

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return Get(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
