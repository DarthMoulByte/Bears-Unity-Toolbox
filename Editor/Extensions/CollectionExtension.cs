using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Sirenix.Utilities;
using UnityEngine;

namespace BearsUtilities
{
    public static class CollectionExtension
    {
        public static bool ContentsEquals<T>(this IEnumerable<T> thisEnumerable, IEnumerable<T> enumerable)
            where T : IEquatable<T>
        {
            if (thisEnumerable is IList<T>)
            {
                // this is fine
                return ContentsEquals((IList<T>) thisEnumerable, (IList<T>) enumerable);
            }

            // but... this you should not use this code outside the inspector, so wasteful...

            Debug.LogWarning("Util: This is extremely unoptimal code that should not be run at runtime");

            if (thisEnumerable == enumerable)
                return true;

            if (thisEnumerable.Count() != enumerable.Count())
                return false;

            for (int i = 0; i < thisEnumerable.Count(); i++)
            {
                if (!thisEnumerable.ElementAt(i).Equals(enumerable.ElementAt(i)))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the normalized value of the item's index in the given list, aka return the "progress" of an item when iterating through a list. For use with progress bars.
        /// </summary>
        public static float GetProgressAt<T>(this List<T> list, T item)
        {
            return list.Count.GetProgressAt(list.IndexOf(item));
        }
        
        /// <summary>
        /// Returns the normalized value of the item's index in the given array, aka return the "progress" of an item when iterating through a list. For use with progress bars.
        /// </summary>
        public static float GetProgressAt<T>(this T[] array, T item)
        {
            return array.Length.GetProgressAt(array.ToList().IndexOf(item));
        }
        
        /// <summary>
        /// Returns the normalized value of an index given a certain count, aka return the "progress" of an index when iterating through a list. For use with progress bars.
        /// </summary>
        public static float GetProgressAt(this int count, int current)
        {
            return (1f / count) * current;
        }

        public static string Joined(this IEnumerable<string> list, string separator = "\n")
        {
            return string.Join(separator, list.ToArray());
        }

        public static IList<T> RemoveNullEntries<T>(this IList<T> list)
            where T : class
        {
            if (list == null)
                return null;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                    list.RemoveAt(i);
            }

            return list;
        }

        public static IList<T> RemoveDuplicateEntries<T>(this IList<T> list)
            where T : class
        {
            if (list == null)
                return null;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                    list.RemoveAt(i);
            }

            return list.Distinct().ToList();
        }

        public static IList<T> RemoveEntries<T>(this IList<T> list, params T[] entries)
            where T : class
        {
            if (list == null)
                return null;

            foreach (var entry in entries)
            {
                list.Remove(entry);
            }

            return list;
        }

        public static bool ContentsEquals<T>(this IList<T> thisList, IList<T> list)
            where T : IEquatable<T>
        {
            if (thisList == list)
                return true;

            if (thisList.Count != list.Count)
                return false;

            for (int i = 0; i < thisList.Count; i++)
            {
                if (!thisList[i].Equals(list[i]))
                    return false;
            }

            return true;
        }

        public static int FindIndex<T>(this IList list, T type)
            where T : class
        {
            int index = -1;
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (System.Object.ReferenceEquals(list[i], type))
                    return i;
            }

            return index;
        }

        public static T GetRandom<T>(this IList list)
        {
            if (list != null && list.Count > 0)
            {
                return (T) list[UnityEngine.Random.Range(0, list.Count)];
            }

            return default(T);
        }
        public static List<T> GetRandomUnique<T>(this List<T> list, int amount)
        {
            if (amount == 0) return null;

            if (amount >= list.Count)
                return list.ToList();

            var li = list.ToList();
            
            li.Shuffle();

            return li.GetRange(0, amount);
        }

        public static T GetWrapped<T>(this List<T> list, int index)
        {
            if (list != null && list.Count > 0)
            {
                index = Mathf.Max(0, index);
                
                int i = index % list.Count;
                
                return list[i];
            }

            return default(T);
        }
        
        public static T GetWrapped<T>(this T[] arr, int index)
        {
            if (arr != null && arr.Length > 0)
            {
                index = Mathf.Max(0, index);
                
                int i = index % arr.Length;
                
                return arr[i];
            }

            return default(T);
        }

        public static void Move(this IList list, int iIndexToMove, int direction)
        {
            direction = Mathf.Clamp(direction, -1, 1);

            // upwards
            if (direction == 1 && iIndexToMove > 0)
            {
                var old = list[iIndexToMove - 1];
                list[iIndexToMove - 1] = list[iIndexToMove];
                list[iIndexToMove] = old;
            }
            // downwards
            else if (direction == -1 && iIndexToMove < list.Count - 1)
            {
                var old = list[iIndexToMove + 1];
                list[iIndexToMove + 1] = list[iIndexToMove];
                list[iIndexToMove] = old;
            }
            else
            {
                Debug.LogError("Util: Direction was not in bounds 1 or -1, value was: " + direction);
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> items, T item)
        {
            var enumerableItem = new T[1] {item};
            return items.Concat(enumerableItem);
        }

        public static IEnumerable<T> RemoveItems<T>(this IEnumerable<T> items, T item)
            where T : IEquatable<T>
        {
            List<T> list = new List<T>();

            foreach (var originalItem in items)
            {
                if (!originalItem.Equals(item))
                    list.Add(originalItem);
            }

            return list;
        }

        public static IEnumerable<T> ReplaceItems<T>(this IEnumerable<T> items, T item, T newItem)
            where T : IEquatable<T>
        {
            List<T> list = new List<T>();

            foreach (var originalItem in items)
            {
                if (!originalItem.Equals(item))
                    list.Add(newItem);
            }

            return list;
        }

        public static void Swap(this IList list, int firstIndex, int secondIndex)
        {
            if (list != null && firstIndex >= 0 &&
                firstIndex < list.Count && secondIndex >= 0 &&
                secondIndex < list.Count)
            {
                if (firstIndex == secondIndex)
                {
                    return;
                }

                var temp = list[firstIndex];
                list[firstIndex] = list[secondIndex];
                list[secondIndex] = temp;
            }
        }

        public static bool IsEven(this int var) { return var % 2 == 0; }

        public static bool IsOdd(this int var) { return !IsEven(var); }


        public static T FindFirstOfType<T>(this IEnumerable<T> collection, System.Type subType)
        {
            if (collection == null)
                return default(T);

            foreach (var item in collection)
            {
                var type = item.GetType();
                if (type.IsSubclassOf(subType) || type == subType)
                    return item;
            }

            return default(T);
        }

        public static List<T> FindAllOfType<T>(this IEnumerable<T> collection, System.Type subType)
        {
            List<T> list = new List<T>();

            if (collection == null)
                return list;

            foreach (var item in collection)
            {
                var type = item.GetType();
                if (type.IsSubclassOf(subType) || type == subType)
                    list.Add(item);
            }

            return list;
        }


        public static bool ExistsAt(this IList list, int index)
        {
            if (list == null)
                return false;

            return (list.Count > index && list[index] != null);
        }

        public static T RandomElement<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomElement(new System.Random());
        }

        public static T RandomElement<T>(this IEnumerable<T> enumerable, System.Random rand)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }

        public static int FindSum<TKey>(this IDictionary<TKey, int> dict)
        {
            var sum = 0;

            foreach (var d in dict)
            {
                sum += d.Value;
            }

            return sum;
        }

        public static string ToString(this IDictionary source, string separator)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source.Count == 0)
            {
                return "{}";
            }

            var sb = new StringBuilder("{");
            foreach (DictionaryEntry x in source)
            {
                sb.Append(x.Key).Append(separator);
                var valueAsDictionary = x.Value as IDictionary;
                if (valueAsDictionary != null)
                {
                    sb.Append(valueAsDictionary.ToString(separator));
                }
                else
                {
                    sb.Append(x.Value);
                }

                sb.Append(',');
            }

            sb[sb.Length - 1] = '}';
            return sb.ToString();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = UnityEngine.Random.Range(0, n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static public TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue result;
            dict.TryGetValue(key, out result);
            return result;
        }

        static public TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue def)
        {
            TValue result;
            if (dict.TryGetValue(key, out result))
                return result;
            else
                return def;
        }

        static public Dictionary<TKey, TDest> ConvertValues<TKey, TSource, TDest>(this IDictionary<TKey, TSource> dict,
            System.Func<TSource, TDest> convertor)
        {
            var dictionary = new Dictionary<TKey, TDest>();

            foreach (var pair in dict)
                dictionary.Add(pair.Key, convertor(pair.Value));
            return dictionary;
        }

        public static bool ContainsType<TType, TSource>(this List<TSource> source) where TType : class
        {
            for (int index = 0; index < source.Count; index++)
            {
                var source1 = source[index];
                if (source1 is TType)
                    return true;
            }

            return false;
        }

        public static void AddOrUpdate<T>(this IList<T> list, T value)
        {
            int index = list.IndexOf(value);
            if (index != -1)
            {
                list[index] = value;
            }
            else
            {
                list.Add(value);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TKey : class
        {
            if (key == null)
            {
                Debug.LogError("Utils: Key was null!");
                return;
            }

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : class
            where TValue : class, new()
        {
            if (key == null)
            {
                Debug.LogError("Utils: Key was null!");
                return default(TValue);
            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                var value = new TValue();
                dictionary.Add(key, value);
                return value;
            }
        }

        public static List<TResult> CreateAll<TSource, TResult>(this List<TSource> source,
            Func<TSource, TResult> createFunc)
        {
            var result = new List<TResult>(source.Count);
            for (int index = 0; index < source.Count; index++)
            {
                var source1 = source[index];
                result.Add(createFunc(source1));
            }

            return result;
        }

        public static TResult[] CreateAllAsArray<TSource, TResult>(this List<TSource> source,
            Func<TSource, TResult> createFunc)
        {
            var result = new TResult[source.Count];
            var index = 0;
            for (int i = 0; i < source.Count; i++)
            {
                var source1 = source[i];
                result[index++] = createFunc(source1);
            }

            return result;
        }

        public static IEnumerable<TResult> CastAll<TResult>(this IEnumerable source) { return source.Cast<TResult>(); }

        public static TResult FindFirst<TResult>(this List<TResult> source) where TResult : class
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            return source[0];
        }


        public static TResult FindLast<TResult>(this List<TResult> source) where TResult : class
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            return source[source.Count - 1];
        }

        public static TResult FindFirst<TResult>(this TResult[] source) where TResult : class
        {
            if (source == null || source.Length == 0)
            {
                return null;
            }

            return source[0];
        }


        public static TResult FindLast<TResult>(this TResult[] source) where TResult : class
        {
            if (source == null || source.Length == 0)
            {
                return null;
            }

            return source[source.Length - 1];
        }

        public static bool IsNullOrEmpty(this ICollection collection)
        {
            bool isValid = collection != null && collection.Count != 0;
            return !isValid;
        }
        
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            bool isValid = array != null && array.Length != 0;
            return !isValid;
        }

        /// <summary>
        /// IsNullOrEmpty alternative for generic enumerables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEnumerableNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                return true;

            foreach (var item in collection)
                return false;

            return true;
        }

        public static bool IsAll<T>(this List<T> source, Predicate<T> match)
        {
            if (source == null)
            {
                return false;
            }

            if (match == null)
            {
                return false;
            }

            foreach (var source1 in source)
            {
                if (!match(source1))
                {
                    return false;
                }
            }

            return true;
        }

        public static int FindMax(this List<int> list)
        {
            int max = 0;
            for (int index = 0; index < list.Count; index++)
            {
                var i = list[index];
                if (i > max)
                {
                    max = i;
                }
            }

            return max;
        }

        public static float FindMax(this List<float> list)
        {
            float max = 0;
            for (int index = 0; index < list.Count; index++)
            {
                var i = list[index];
                if (i > max)
                {
                    max = i;
                }
            }

            return max;
        }

        public static TSource Random<TSource>(this List<TSource> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T FindComponent<T>(this List<GameObject> items, Predicate<T> match) where T : Component
        {
            var components = new List<T>();
            for (int index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var comp = item.GetComponent<T>();
                if (comp != null)
                {
                    components.Add(comp);
                }
            }

            return components.Find(match);
        }

        public static T FindMaxMapped<T>(this IEnumerable<T> list, Func<T, float> predicate)
        {
            var max = Mathf.NegativeInfinity;
            T maxT = default(T);

            foreach (var item in list)
            {
                float val = predicate(item);
                if (val > max)
                {
                    max = val;
                    maxT = item;
                }
            }

            return maxT;
        }

        public static T FindMinMapped<T>(this IEnumerable<T> list, Func<T, float> predicate)
        {
            var min = Mathf.Infinity;
            T minT = default(T);

            foreach (var item in list)
            {
                float val = predicate(item);
                if (val < min)
                {
                    min = val;
                    minT = item;
                }
            }

            return minT;
        }


        public static int FindMax<TSource>(this IEnumerable<TSource> list, Func<TSource, int> predicate)
        {
            var max = 0;

            foreach (var item in list)
            {
                max = Mathf.Max(max, predicate(item));
            }

            return max;
        }

        public static int FindMax<TSource>(this List<TSource> list, Func<TSource, int> predicate)
        {
            var max = 0;

            for (int index = 0; index < list.Count; index++)
            {
                var item = list[index];
                max = Mathf.Max(max, predicate(item));
            }

            return max;
        }

        public static TSource FindMax<TSource>(this IEnumerable<TSource> list, Func<TSource, TSource, bool> predicate)
            where TSource : class
        {
            TSource max = null;

            foreach (var i in list) // (int index = 0; index < list.Count; index++)
            {
                //var i = list[index];
                if (predicate(i, max))
                {
                    max = i;
                }
            }

            return max;
        }

        public static int FindSum(this List<int> list)
        {
            var sum = 0;

            for (int index = 0; index < list.Count; index++)
            {
                sum += list[index];
            }

            return sum;
        }

        public static int FindSum<TSource>(this IEnumerable<TSource> list, Func<TSource, int> predicate)
        {
            var sum = 0;

            foreach (var item in list)
            {
                sum += predicate(item);
            }

            return sum;
        }

        public static int ItemCount<TSource>(this List<TSource> collection) where TSource : class
        {
            var count = 0;
            for (int index = 0; index < collection.Count; index++)
            {
                var source = collection[index];
                var i = source;
                if (i != null)
                {
                    count++;
                }
            }

            return count;
        }

        public static int CountAllTrue(this IEnumerable<bool> collection)
        {
            var count = 0;

            foreach (var i in collection)
                if (i)
                    count++;

            return count;
        }

        public static int CountAll<TSource>(this IEnumerable<TSource> collection, Predicate<TSource> match)
        {
            var count = 0;

            foreach (var i in collection)
                if (match(i))
                    count++;

            return count;
        }

        public static List<TResult> CreateList<TKey, TSource, TResult>(this Dictionary<TKey, TSource> list,
            Func<TSource, TResult> createFunc) where TResult : class
        {
            var result = new List<TResult>(list.Count);
            foreach (var source in list)
            {
                var create = createFunc(source.Value);
                if (create != null)
                {
                    result.Add(create);
                }
            }

            return result;
        }

        public static List<TResult> CreateList<TSource, TResult>(this List<TSource> list,
            Func<TSource, TResult> createFunc)
        {
            var result = new List<TResult>(list.Count);
            for (int index = 0; index < list.Count; index++)
            {
                result.Add(createFunc(list[index]));
            }

            return result;
        }

        public static List<TResult> CreateList<TSource, TResult>(this TSource[] array,
            Func<TSource, TResult> createFunc)
        {
            var result = new List<TResult>(array.Length);
            for (int index = 0; index < array.Length; index++)
            {
                result.Add(createFunc(array[index]));
            }

            return result;
        }

        public static List<TResult> CreateList<TSourceKey, TSourceValue, TResult>(
            this Dictionary<TSourceKey, TSourceValue>.ValueCollection collection,
            Func<TSourceValue, TResult> createFunc)
        {
            var result = new List<TResult>(collection.Count);
            foreach (var obj in collection)
            {
                result.Add(createFunc(obj));
            }

            return result;
        }

        public static TResult[] CreateArray<TSource, TResult>(this List<TSource> list,
            Func<TSource, TResult> createFunc)
        {
            var result = new TResult[list.Count];
            for (int index = 0; index < list.Count; index++)
            {
                result[index] = createFunc(list[index]);
            }

            return result;
        }

        public static TResult[] CreateArray<TSource, TResult>(this TSource[] array, Func<TSource, TResult> createFunc)
        {
            var result = new TResult[array.Length];
            for (int index = 0; index < array.Length; index++)
            {
                result[index] = createFunc(array[index]);
            }

            return result;
        }

        public static TResult[] CreateArray<TSourceKey, TSourceValue, TResult>(
            this Dictionary<TSourceKey, TSourceValue>.ValueCollection collection,
            Func<TSourceValue, TResult> createFunc)
        {
            var result = new TResult[collection.Count];
            var index = 0;
            foreach (var obj in collection)
            {
                result[index++] = createFunc(obj);
            }

            return result;
        }

        public static TResult[] CreateArray<TSourceKey, TSourceValue, TResult>(
            this Dictionary<TSourceKey, TSourceValue>.KeyCollection collection, Func<TSourceKey, TResult> createFunc)
        {
            var result = new TResult[collection.Count];
            var index = 0;
            foreach (var obj in collection)
            {
                result[index++] = createFunc(obj);
            }

            return result;
        }

        public static TSourceKey[] CreateArray<TSourceKey, TSourceValue>(
            this Dictionary<TSourceKey, TSourceValue>.KeyCollection collection)
        {
            var result = new TSourceKey[collection.Count];
            var index = 0;
            foreach (var obj in collection)
            {
                result[index++] = obj;
            }

            return result;
        }

        public static List<TSource> CreateList<TSource>(this IEnumerable<TSource> source) { return source.ToList(); }

        public static TSource FindFirstOrDefault<TSource>(this List<TSource> source, Func<TSource, bool> predicate)
            where TSource : class
        {
            if (source == null)
                return null;
            if (predicate == null)
            {
                if (source.Count > 0)
                {
                    return source[0];
                }

                return null;
            }

            for (int index = 0; index < source.Count; index++)
            {
                TSource source1 = source[index];
                if (predicate(source1))
                {
                    return source1;
                }
            }

            return null;
        }

        public static TSource FindFirstOrDefault<TSource>(this TSource[] source, Func<TSource, bool> predicate)
            where TSource : class
        {
            if (source == null)
                return null;
            if (predicate == null)
            {
                if (source.Length > 0)
                {
                    return source[0];
                }

                return null;
            }

            for (int index = 0; index < source.Length; index++)
            {
                TSource source1 = source[index];
                if (predicate(source1))
                {
                    return source1;
                }
            }

            return null;
        }

        public static TSource FindFirstOrDefault<TKey, TSource>(this Dictionary<TKey, TSource> source,
            Func<KeyValuePair<TKey, TSource>, bool> predicate) where TSource : class
        {
            if (source == null)
                return null;
            if (predicate == null)
            {
                if (source.Count > 0)
                {
                    return source.First().Value;
                }

                return null;
            }

            foreach (var source1 in source)
            {
                if (predicate(source1))
                {
                    return source1.Value;
                }
            }

            return null;
        }
    }
}