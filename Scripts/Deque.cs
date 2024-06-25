#nullable enable
namespace UniT.Utilities
{
    using System.Collections;
    using System.Collections.Generic;

    public class Deque<T> : ICollection<T>
    {
        private readonly LinkedList<T> items = new LinkedList<T>();

        public int Count => this.items.Count;

        public void PushFront(T item)
        {
            this.items.AddFirst(item);
        }

        public void PushBack(T item)
        {
            this.items.AddLast(item);
        }

        public T PopFront()
        {
            var item = this.PeekFront();
            this.items.RemoveFirst();
            return item;
        }

        public T PopBack()
        {
            var item = this.PeekBack();
            this.items.RemoveLast();
            return item;
        }

        public T PeekFront()
        {
            return this.items.First.Value;
        }

        public T PeekBack()
        {
            return this.items.Last.Value;
        }

        public void Clear()
        {
            this.items.Clear();
        }

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() => this.items.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.items.GetEnumerator();

        void ICollection<T>.Add(T item) => this.items.AddLast(item);

        bool ICollection<T>.Contains(T item) => this.items.Contains(item);

        bool ICollection<T>.Remove(T item) => this.items.Remove(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => this.items.CopyTo(array, arrayIndex);

        #endregion
    }
}