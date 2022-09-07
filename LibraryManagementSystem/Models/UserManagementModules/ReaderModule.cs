using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ValueObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ReaderModule
{
    /// <summary>
    /// 读者个人信息管理模块
    /// </summary>
    public class ReaderModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReaderModule()
        {
            //处理
        }

        /// <summary>
        /// 判断readerId读者是否
        /// </summary>
        /// <param name="readerId"></param>
        /// <returns></returns>
        public Boolean userCheck(string readerId)
        {
            DataRow status = Sql.Read($"SELECT STATE FROM READER WHERE READER_ID='{readerId}'")[0];
            if (System.Convert.ToBoolean(status[0]))
                return true;
            return false;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="readerIdCard"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public List<Object> userLogin(string phone, string pwd)
        {
            List<Object> result = new List<object>();
            try
            {
                DataRow reader = Sql.Read($"SELECT * FROM READER WHERE PHONE_NUMBER='{phone}'")[0];
                result.Add(reader);
                if (String.Equals(pwd, reader[2]))
                {
                    Sql.Execute($"update reader set STATE=1  WHERE PHONE_NUMBER={phone}");
                    return result;
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 用户推出
        /// </summary>
        /// <param name="readerIdCard"></param>
        public void userLogout(string phone)
        {
            Sql.Execute($"update reader set STATE=0  WHERE PHONE_NUMBER={phone}");
        }

        /// <summary>
        /// 借书证办理
        /// </summary>
        /// <param name="readerName"></param>
        /// <param name="password"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="readerIdCard"></param>
        public Boolean userRegister(string readerName, string password, string phoneNumber, string readerIdCard)
        {
            try
            {
                DataRowCollection readers = Sql.Read("SELECT * FROM READER");
                int count = readers.Count;
                var readerId = Convert.ToString(count + 1);
                Sql.Execute(
                    $"INSERT INTO READER" +
                    $"(READER_ID, READER_NAME, READER_PASSWORD, PHONE_NUMBER, READER_IDCARD)" +
                    $"VALUES('{readerId}', '{readerName}', '{password}', '{phoneNumber}', '{readerIdCard}')"
                    );
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="readerId"></param>
        /// <param name="currentPassword"></param>
        /// <param name="modifiedPassword"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 查看个人信息
        /// </summary>
        /// <param name="readerId"></param>
        /// <returns></returns>
        public DataRow ViewReaderInformation(string readerId)
        {
            DataRowCollection readers = Sql.Read($"SELECT * FROM READER WHERE READER_ID='{readerId}'");
            DataRow reader = readers[0];
            return reader;
        }

        /// <summary>
        /// 编辑个人信息
        /// </summary>
        /// <param name="readerId"></param>
        /// <param name="readerName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="readerIdCard"></param>
        public void EditReaderInformation(string readerId, string readerName, int phoneNumber, string readerIdCard)
        {
            DataRowCollection rows = Sql.Read($"SELECT * FROM READER WHERE READER_ID = '{readerId}'");
            if (rows.Count == 0 || rows.Count >= 2)
            {
                throw new Exception();
            }
            DataRow reader = rows[0];
            Sql.Execute(
                $"UPDATE READER " +
                $"SET (READER_NAME = '{readerName}', " +
                $"PHONE_NUMBER = {phoneNumber}, " +
                $"READER_IDCARD = '{readerIdCard}') " +
                $"WHERE (READER_ID = '{readerId}')");
        }

        /// <summary>
        /// 查看借阅历史
        /// </summary>
        /// <param name="readerId"></param>
        /// <returns></returns>
        public DataRowCollection SearchBorrowedHistory(string readerId)
        {
            return Sql.Read($"SELECT * FROM RETURN WHERE READER_ID='{readerId}'");
        }

        /// <summary>
        /// 查看当前借阅
        /// </summary>
        /// <param name="readerId"></param>
        /// <returns></returns>
        public DataRowCollection SearchBorrowingHistory(string readerId)
        {
            return Sql.Read($"SELECT * FROM BORROW WHERE READER_ID='{readerId}'");
        }

        /// <summary>
        /// 查看座位功能
        /// </summary>
        /// <returns></returns>
        public DataRowCollection ViewSeats()
        {
            return Sql.Read("SELECT * FROM SEAT");
        }

        /// <summary>
        /// 座位预约功能
        /// </summary>
        /// <param name="readerId"></param>
        /// <param name="seatId"></param>
        /// <param name="seatReserveTime"></param>
        /// <param name="seatOverdueTime"></param>
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

        /// <summary>
        /// 读者留言功能
        /// </summary>
        /// <param name="readerId"></param>
        /// <param name="bookId"></param>
        /// <param name="content"></param>
        public void ReaderComment(string readerId, string bookId, string content)
        {
            string contentId = Convert.ToString(Sql.Read("SELECT * FROM COMMENTS").Count + 1);
            Sql.Execute(
               $"INSERT INTO COMMENTS" +
               $"(READER_ID, BOOK_ID, CONTENT, CONTENT_TIME, CONTENT_ID)" +
               $"VALUES('{readerId}', '{bookId}', '{content}', TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd")}', 'YYYY-MM-DD HH24:MI:SS'), '{contentId}')"
               );
        }

        public DataRowCollection ViewBooks()
        {
            return Sql.Read("SELECT * FROM BOOK");
        }

        /// <summary>
        /// 书籍预约功能
        /// </summary>
        /// <param name="readerId"></param>
        /// <param name="bookId"></param>
        public void ReserveBook(string readerId, string bookId)
        {
            string bookReserveId = $"br{Sql.Read("SELECT * FROM BOOK_RESERVE").Count + 1}";
            Sql.Execute(
               $"INSERT INTO BOOK_RESERVE" +
               $"(BOOK_RESERVE_ID, READER_ID, BOOK_ID, BOOK_RESERVE_TIME, BOOK_OVERDUE_TIME)" +
               $"VALUES('{bookReserveId}', " +
               $"'{readerId}', '{bookId}',  " +
               $"TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd")}', 'YYYY-MM-DD HH24:MI:SS'), " +
               $"TO_DATE('{DateTime.Now.AddDays(14).ToString("yyyy/MM/dd")}', 'YYYY-MM-DD HH24:MI:SS'))");

            Sql.Execute(
                $"UPDATE BOOK " +
                $"SET LEND_RESERVE=LEND_RESERVE+1 " +
                $"WHERE BOOK_ID='{bookId}'"
                );
        }
    }
}
