using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralTools
{
    public delegate bool CompareDelegate<T>(T x, T y);
    public class CompareTools<T> : IEqualityComparer<T>
    {
        private CompareDelegate<T> _compare;
        /// <summary>
        /// 通用去除List集合重复数据
        /// listResult = list.Distinct(new Compare<ColumnDicModel>((x, y) => (x != null && y != null) && (x.DicCode == y.DicCode))).ToList()
        /// </summary>
        /// <param name="d"></param>
        public CompareTools(CompareDelegate<T> d)
        {
            this._compare = d;
        }

        public bool Equals(T x, T y)
        {
            if (_compare != null)
            {
                return this._compare(x, y);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
