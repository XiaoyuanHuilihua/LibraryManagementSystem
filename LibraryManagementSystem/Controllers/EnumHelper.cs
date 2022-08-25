using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    public static class EnumHelper
    {
        /// <summary>
        /// 指定の列挙体の列挙子に変換する
        /// </summary>
        /// <typeparam name="T">列挙体</typeparam>
        /// <param name="value">列挙子の文字列</param>
        /// <returns>列挙子</returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
        }
    }
}
