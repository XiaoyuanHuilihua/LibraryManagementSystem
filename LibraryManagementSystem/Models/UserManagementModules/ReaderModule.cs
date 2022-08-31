using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ValueObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ReaderModule
{
    public class ReaderModule
    {
        /// <summary>
        /// 读者个人信息管理模块
        /// </summary>

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReaderModule()
        {
            //处理
        }

        ///借书证办理
        public void ApplyLibraryCard(string readerName, string password, long phoneNumber, string readerIdCard)
        {
            DataRowCollection readers = Sql.Read("SELECT * FROM READER");
            int count = readers.Count;
            var readerId = Convert.ToInt32(readers[count - 1][0]) + 1;
            Sql.Execute(
                $"INSERT INTO READER" +
                $"(READER_ID, READER_NAME, READER_PASSWORD, PHONE_NUMBER, READER_IDCARD)" +
                $"VALUES('{readerId}', '{readerName}', '{password}', '{phoneNumber}', '{readerIdCard}')"
                );
        }

        ///修改密码
        public Boolean AlterPassword(string readerId, string currentPassword, string modifiedPassword)
        {
            DataRow reader;
            reader = Sql.Read($"SELECT * FROM READER WHERE READER_ID='{readerId}'")[0];
            if (String.Equals(reader[2], currentPassword))
            {
                Sql.Execute(
                $"UPDATE READER " +
                $"SET READER_PASSWORD='{modifiedPassword}' " +
                $"WHERE READER_ID='{readerId}'"
                );
                return true;
            }
            return false;
        }

        ///查看个人信息
        public DataRow ViewReaderInformation(string readerId)
        {
            return Sql.Read($"SELECT * FROM READER WHERE READER_ID='{readerId}'")[0];
        }

        ///编辑个人信息
        public void EditReaderInformation(string readerId, string readerName, string phoneNumber, string readerIdCard)///"这里是把所有信息分开传递还是放在一个集合中传递"
        {
            Sql.Execute(
                $"UPDATE READER " +
                $"SET READER_NAME='{readerName}',PHONE_NUMBER='{phoneNumber}', READER_IDCARD='{readerIdCard}' " +
                $"WHERE READER_ID='{readerId}'"
                );
        }

        ///查看借阅历史
        public DataRowCollection SearchBorrowedHistory(string readerId)
        {
            return Sql.Read($"SELECT * FROM RETURN WHERE READER_ID='{readerId}'");
        }

        ///查看当前借阅
        public DataRowCollection SearchBorrowingHistory(string readerId)
        {
            return Sql.Read($"SELECT * FROM BORROW WHERE READER_ID='{readerId}'");
        }

        ///查看座位功能
        public DataRowCollection ViewSeats()
        {
            return Sql.Read("SELECT * FROM SEAT");
        }

        ///座位预约功能
        public void ReserveSeat(string readerId, string seatId, DateTime seatReserveTime, DateTime seatOverdueTime)
        {
            string seatReserveId = $"r{Sql.Read("SELECT * FROM SEAT_RESERVE").Count + 1}";

            Sql.Execute(
               $"INSERT INTO SEAT_RESERVE " +
               $"(READER_ID, SEAT_ID, SEAT_RESERVE_TIME, SEAT_OVERDUE_TIME, SEAT_RESERVE_ID) " +
               $"VALUES('{readerId}', '{seatId}', TO_DATE('{seatReserveTime}', 'YYYY-MM-DD HH24:MI:SS'), TO_DATE('{seatOverdueTime}', 'YYYY-MM-DD HH24:MI:SS'), '{seatReserveId}')"
               );
            Sql.Execute(
                $"UPDATE SEAT " +
                $"SET SEAT_STATE='1' " +
                $"WHERE SEAT_ID='{seatId}'"
                );
        }

        ///读者留言功能
        public void ReaderComment(string readerId, string bookId, string content, DateTime commentTime)
        {
            int contentId = Sql.Read("SELECT * FROM COMMENTS").Count + 1;
            Sql.Execute(
               $"INSERT INTO COMMENTS" +
               $"(READER_ID, BOOK_ID, CONTENT, CONTENT_TIME, CONTENT_ID)" +
               $"VALUES('{readerId}', '{bookId}', '{content}', TO_DATE('{commentTime}', 'YYYY-MM-DD HH24:MI:SS'), '{contentId}')"
               );
        }

        ///书籍预约功能
        public void ReserveBook(string readerId, int bookId, DateTime bookReserveTime, DateTime bookOverdueTime)
        {
            string bookReserveId = $"br{Sql.Read("SELECT * FROM BOOK_RESERVE").Count + 1}";
            Sql.Execute(
               $"INSERT INTO BOOK_RESERVE" +
               $"(BOOK_RESERVE_ID, READER_ID, BOOK_ID, BOOK_RESERVE_TIME, BOOK_OVERDUE_TIME)" +
               $"VALUES('{bookReserveId}', '{readerId}', '{bookId}',  TO_DATE('{bookReserveTime}', 'YYYY-MM-DD HH24:MI:SS'),  TO_DATE('{bookOverdueTime}', 'YYYY-MM-DD HH24:MI:SS'))"
               );
            Sql.Execute(
                $"UPDATE BOOK " +
                $"SET LEND_RESERVE=LEND_RESERVE+1 " +
                $"WHERE BOOK_ID='{bookId}'"
                );
        }
    }
}
