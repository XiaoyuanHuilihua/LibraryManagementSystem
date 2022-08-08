using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var vo = obj as T;
            if (vo == null)
            {
                return false;
            }

            return EqualsCore(vo);
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="vo1">vo1</param>
        /// <param name="vo2">vo2</param>
        /// <returns></returns>
        public static bool operator ==(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return Equals(vo2, vo2);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="vo1">vo1</param>
        /// <param name="vo2">vo2</param>
        /// <returns></returns>
        public static bool operator !=(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return !Equals(vo2, vo2);
        }

        /// <summary>
        /// EqualsCore
        /// </summary>
        /// <param name="other">other</param>
        /// <returns></returns>
        protected abstract bool EqualsCore(T other);

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>文字列</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns>値</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
