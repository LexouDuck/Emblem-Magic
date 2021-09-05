using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.Collections.Trees
{
    public static class TreeHelpers
    {
        public static IEnumerable<T> DepthFirstEnumerator<T>(this IValuedTree<T> tree)
        {
            foreach (var child in tree.GetChildren())
            {
                foreach (var value in child.DepthFirstEnumerator())
                {
                    yield return value;
                }
            }
            if (tree.HasValue)
            {
                yield return tree.Value;
            }
        }

        public static IEnumerable<T> BreadthFirstEnumerator<T>(this IValuedTree<T> tree)
        {
            if (tree.HasValue)
            {
                yield return tree.Value;
            }
            foreach (var child in tree.GetChildren())
            {
                foreach (var value in child.BreadthFirstEnumerator())
                {
                    yield return value;
                }
            }
        }

        public static IEnumerable<T> NthEnumerator<T>(this IValuedTree<T> tree, int position)
        {
            foreach (var child in tree.GetChildren())
            {
                if (position == 0 && tree.HasValue)
                {
                    yield return tree.Value;
                }
                foreach (var value in child.BreadthFirstEnumerator())
                {
                    yield return value;
                }
                position--;
            }

            if (position >= 0 && tree.HasValue)
            {
                yield return tree.Value;
            }
        }


        public static IEnumerable<T> DepthFirstEnumerator<T>(this T tree)
            where T : ITree<T>
        {
            foreach (var child in tree.GetChildren())
            {
                foreach (var value in child.DepthFirstEnumerator())
                {
                    yield return value;
                }
            }
            yield return tree;
        }

        public static IEnumerable<T> BreadthFirstEnumerator<T>(this T tree)
            where T : ITree<T>
        {
            yield return tree;
            foreach (var child in tree.GetChildren())
            {
                foreach (var value in child.BreadthFirstEnumerator())
                {
                    yield return value;
                }
            }
        }

        public static IEnumerable<T> NthEnumerator<T>(this T tree, int position)
            where T : ITree<T>
        {
            foreach (var child in tree.GetChildren())
            {
                if (position == 0)
                {
                    yield return tree;
                }
                foreach (var value in child.BreadthFirstEnumerator())
                {
                    yield return value;
                }
                position--;
            }

            if (position >= 0)
            {
                yield return tree;
            }
        }

    }
}
