using System;
using System.Collections.Generic;

namespace System
{
    public static class SystemExtensionsMethods
    {
        public static string FormatWith (this string self, params object[] args)
        {
            return string.Format (self, args);
        }

        public static Value GetOrAdd<Key, Value> (this Dictionary<Key, Value> self, Key key, Value value)
        {
            Value existVal;

            if (self.TryGetValue (key, out existVal))
                return existVal;
            else
                self.Add (key, value);

            return value;
        }
    }
}

