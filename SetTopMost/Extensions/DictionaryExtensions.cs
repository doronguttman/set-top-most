using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetTopMost.Extensions
{
    internal static class DictionaryExtensions
    {
        public static (TKey key, TValue value) Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
        {
            return (pair.Key, pair.Value);
        }

        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
