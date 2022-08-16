using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    /// <summary>
    /// 当前借书表的ValueObject
    /// </summary>
    public class ReaderCuurrentBorrowing : ValueObject<ReaderCuurrentBorrowing>
    {
        /// <summary>
        /// 续借状态
        /// </summary>
        public enum ContinuedStates
        {
            yet,
            already,
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReaderCuurrentBorrowing(string readerId, string bookId, string bookName, DateTime borrowDate, DateTime latestReturnDate, ContinuedStates continuedState)
        {
            ReaderId = readerId;
            BookId = bookId;
            BookName = bookName;
            BorrowDate = borrowDate;
            LatestReturnDate = latestReturnDate;
            ContinuedState = continuedState;
        }

        /// <summary>
        ///读者帐号
        /// </summary>
        public string ReaderId { get; set; }

        /// <summary>
        ///图书编号
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        ///图书名
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        ///借书日期
        /// </summary>
        public DateTime BorrowDate { get; set; }

        /// <summary>
        ///最晚还书日期时间
        /// </summary>
        public DateTime LatestReturnDate { get; set; }

        /// <summary>
        ///续借状态
        /// </summary>
        public ContinuedStates ContinuedState { get; set; }

        /// <inheritdoc/>
        protected override bool EqualsCore(ReaderCuurrentBorrowing other)
        {
            return true;
        }
    }
}
