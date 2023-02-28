
namespace XS.Core2.XsExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class CollectionExtensions
    {
        /// <summary>
        /// Returns a read-only <seealso cref="ISet{T}"/> wrapper for the specified set.
        /// </summary>
        public static ISet<T> AsReadOnly<T>(this ISet<T> set)
        {
            return set.IsReadOnly ? set : new ReadOnlySet<T>(set);
        }

        /// <summary>
        /// An readonly wrapper for ISet instances.
        /// </summary>
        private class ReadOnlySet<T> : ISet<T>
        {
            private ISet<T> _set;
            internal ReadOnlySet(ISet<T> set)
            {
                if (set == null)
                    throw new ArgumentNullException(nameof(set));
                _set = set;
            }

            public int Count => _set.Count;

            public bool IsReadOnly => true;

            public bool Contains(T item)
            {
                return _set.Contains(item);
            }

            bool ISet<T>.Add(T item)
            {
                throw new NotSupportedException();
            }

            void ICollection<T>.Add(T item)
            {
                throw new NotSupportedException();
            }

            void ICollection<T>.Clear()
            {
                throw new NotSupportedException();
            }

            void ICollection<T>.CopyTo(T[] array, int arrayIndex)
            {
                _set.CopyTo(array, arrayIndex);
            }

            void ISet<T>.ExceptWith(IEnumerable<T> other)
            {
                throw new NotSupportedException();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return _set.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _set.GetEnumerator();
            }

            void ISet<T>.IntersectWith(IEnumerable<T> other)
            {
                throw new NotSupportedException();
            }

            bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other)
            {
                return _set.IsProperSubsetOf(other);
            }

            bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other)
            {
                return _set.IsProperSupersetOf(other);
            }

            bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
            {
                return _set.IsSubsetOf(other);
            }

            bool ISet<T>.IsSupersetOf(IEnumerable<T> other)
            {
                return _set.IsSupersetOf(other);
            }

            bool ISet<T>.Overlaps(IEnumerable<T> other)
            {
                return _set.Overlaps(other);
            }

            bool ICollection<T>.Remove(T item)
            {
                throw new NotSupportedException();
            }

            bool ISet<T>.SetEquals(IEnumerable<T> other)
            {
                return _set.SetEquals(other);
            }

            void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
            {
                throw new NotSupportedException();
            }

            void ISet<T>.UnionWith(IEnumerable<T> other)
            {
                throw new NotSupportedException();
            }
        }
    }
}
