using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkies
{
    public static class IEnumerableExtensions
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector )
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach( TSource element in source )
            {
                if( seenKeys.Add( keySelector( element ) ) )
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Returns items from a list until a condition is met, optionally returning the item that matched the condition
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The list of elements</param>
        /// <param name="predicate">The condition to test for.  Items will be returned until this condition is met</param>
        /// <param name="includeMatchedElement">Set to true to return the item that matched the condition.  Defaults to false</param>
        /// <returns></returns>
        public static IEnumerable<TSource> TakeUntil<TSource>( this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool includeMatchedElement = false )
        {
            foreach( TSource element in source )
            {
                if( predicate( element ) == false )
                {
                    yield return element;
                }

                else
                {
                    if( includeMatchedElement )
                    {
                        yield return element;
                    }
                    break;
                }
            }
        }

        public sealed class Item<T>
        {
            public int Index { get; set; }
            public T Value { get; set; }
            public bool IsLast { get; set; }
            public bool IsFirst { get; set; }
            public bool IsAlternate { get; set; }
        }

        public static IEnumerable<Item<T>> Each<T>( this IEnumerable<T> enumerable )
        {

            Item<T> item = null;
            foreach( T value in enumerable )
            {
                Item<T> next = new Item<T>();
                next.Index = 0;
                next.Value = value;
                next.IsLast = false;
                next.IsFirst = next.Index == 0;
                next.IsAlternate = next.Index % 2 == 0;

                if( item != null )
                {
                    next.Index = item.Index + 1;
                    yield return item;
                }
                item = next;
            }

            if( item != null )
            {
                item.IsLast = true;
                yield return item;
            }

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
