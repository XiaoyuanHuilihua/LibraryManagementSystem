using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    /// <summary>
    /// Bookcancel的ValueObject
    /// </summary>
    public class Bookcancel : ValueObject<Bookcancel>
    {
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="bookId">图书编号</param>
        /// <param name="cancelId">注销编号</param>
        /// <param name="cancelDate">注销日期</param>
        /// <param name="isbn">ISBN</param>
        public Bookcancel(string bookId, string cancelId, DateTime cancelDate, string isbn)
        {
            BookId = bookId;
            CancelId = cancelId;
            CancelDate = cancelDate;
            ISBN = isbn;
        }

        /// <summary>
        /// 图书编号
        /// </summary>
        public string BookId { get; }

        /// <summary>
        /// 注销编号
        /// </summary>
        public string CancelId { get; }

        /// <summary>
        /// 注销日期
        /// </summary>
        public DateTime CancelDate { get; }

        /// <summary>
        /// ISBN
        /// </summary>
        public string ISBN { get; }

        /// <inheritdoc/>
        protected override bool EqualsCore(Bookcancel other)
        {
            if (BookId != other.BookId)
            {
                return false;
            }
            if (CancelId != other.CancelId)
            {
                return false;
            }
            if (CancelDate != other.CancelDate)
            {
                return false;
            }
            if (ISBN != other.ISBN)
            {
                return false;
            }

            return true;
        }
    }
}
