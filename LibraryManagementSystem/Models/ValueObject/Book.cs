using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    /// <summary>
    /// Book的ValueObject
    /// </summary>
    public class Book : ValueObject<Book>
    {
        /// <summary>
        /// 书籍的出借状态
        /// </summary>
        public enum BookLendingState
        {
            /// <summary>
            /// 有存货
            /// </summary>
            InStock,

            /// <summary>
            /// 出借
            /// </summary>
            lending,

            /// <summary>
            /// 已预约
            /// </summary>
            reserved,
        }

        public Book(string bookId, string isbn, string bookName, string author, string publisher, DateTime publishDate, string bookDetail, string pictureList, int price)
        {
            BookId = bookId;
            ISBN = isbn;
            BookName = bookName;
            Author = author;
            Publisher = publisher;
            PublishDate = publishDate;
            BookDetail = bookDetail;
            PictureList = pictureList;
            Price = price;
        }

        /// <summary>
        /// 图书编号
        /// </summary>
        public string BookId { get; }

        /// <summary>
        /// ISBN
        /// </summary>
        public string ISBN { get; }

        /// <summary>
        /// 书名
        /// </summary>
        public string BookName { get; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// 出版社
        /// </summary>
        public string Publisher { get; }

        /// <summary>
        /// 出版日期
        /// </summary>
        public DateTime PublishDate { get; }

        /// <summary>
        /// 出借/预约数量
        /// </summary>
        public BookLendingState LendReserve { get; }

        /// <summary>
        /// 图书详细
        /// </summary>
        public string BookDetail { get; }

        /// <summary>
        /// 图集
        /// </summary>
        public string PictureList { get; }

        /// <summary>
        /// 图书标价
        /// </summary>
        public int Price { get; }

        /// <inheritdoc/>
        protected override bool EqualsCore(Book other)
        {
            throw new NotImplementedException();
        }
    }
}
