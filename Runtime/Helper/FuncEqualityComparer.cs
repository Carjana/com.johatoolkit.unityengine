using System;
using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.Utility
{
    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equalsFunc;
        public FuncEqualityComparer(Func<T, T, bool> func) => _equalsFunc = func;

        public bool Equals(T x, T y) => _equalsFunc.Invoke(x, y);

        public int GetHashCode(T obj) => obj.GetHashCode();
    }
}
