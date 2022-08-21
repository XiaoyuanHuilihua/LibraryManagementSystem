using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ValueObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static LibraryManagementSystem.Models.ValueObject.Book;

namespace LibraryManagementSystem.Models.ManagementBooksModules
{
    public class ManagementBooksModule
    {
        /// <summary>
        /// 图书录入功能
        /// </summary>
        public void ResisterBook(string bookName, string isbn, string author, string publisher, DateTime publishDate, string bookDetail, int price, string pictureList = null)
        {
            //生成图书编号
            int bookId = Sql.Read("SELECT * FROM BOOK").Count + 1;

            //TODO:要删除quantity属性
            var book = new Book(
                bookId.ToString(),
                isbn,
                bookName,
                author,
                publisher,
                publishDate,
                bookDetail,
                pictureList,
                price);

            Sql.Execute(
                $"INSERT INTO BOOK " +
                $"(BOOK_ID, ISBN, BOOK_NAME, AUTHOR, PUBLISHER, PUBLISH_DATE, BOOK_DETAIL, PRICE) " +
                $"VALUES('{book.BookId}'," +
                $"'{book.ISBN}'," +
                $"'{book.BookName}'," +
                $"'{book.Author}'," +
                $"'{book.Publisher}'," +
                $"'{book.PublishDate.ToString("yyyy/MM/dd")}'," +
                $"'{book.BookDetail}'," +
                $"{book.Price})");
        }
    }
}
