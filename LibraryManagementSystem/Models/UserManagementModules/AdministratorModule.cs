using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ValueObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static LibraryManagementSystem.Models.ValueObject.ReaderCuurrentBorrowing;

namespace LibraryManagementSystem.Models.UserManagementModules
{
    /// <summary>
    /// 用户管理模块-管理员模块
    /// </summary>
    public class AdministratorModule
    {
        /// <summary>
        /// 当使用查看功能时，识别分类
        /// </summary>
        public enum ReaderInfoKind
        {
            ReaderId,
            ReaderName,
            Password,
            PhoneNumber,
            ReaderIdCard,
        }

        /// <summary>
        /// 座位预约状态
        /// </summary>
        public enum SeatState
        {
            EmptySeat = 0,
            RESERVED = 1,
            unavailable = 2,
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AdministratorModule()
        {
            //处理
        }

        /// <summary>
        /// 判断管理员是否存在
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public Boolean CheckReturnReaderInfo(string adminId)
        {
            DataRow status = Sql.Read($"SELECT STATE FROM ADMINISTRATOR WHERE ADMIN_ID='{adminId}'")[0];
            if (System.Convert.ToBoolean(status[0]))
                return true;
            return false;
        }

        /// <summary>
        /// 判断adminId是否在线
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public Boolean CheckAdmin(string adminId)
        {
            DataRow status = Sql.Read($"SELECT STATE FROM ADMINISTRATOR WHERE READER_ID='{adminId}'")[0];
            if (System.Convert.ToBoolean(status[0]))
                return true;
            return false;
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="adminName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public Boolean adminLogin(string adminId, string pwd)
        {
            DataRow admin = Sql.Read($"SELECT ADMIN_PASSWORD FROM ADMINISTRATOR WHERE ADMIN_ID='{adminId}'")[0];
            if (String.Equals(pwd, admin[0]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 管理员推出
        /// </summary>
        /// <param name="readerIdCard"></param>
        public void AdminLogout(string adminId)
        {
            Sql.Execute($"update ADMINISTRATOR set STATE=0  WHERE ADMIN_ID='{adminId}'");
        }

        /// <summary>
        ///搜索帐户功能
        /// </summary>
        /// <param name="ReaderInfoKind">标识的分类</param>
        /// <param name="keyword">搜索词</param>
        /// <returns>读者信息（List型）</returns>
        public List<Reader> SearchReaderInfo(ReaderInfoKind ReaderInfoKind, dynamic keyword)
        {
            DataRowCollection rows;
            switch (ReaderInfoKind)
            {
                case ReaderInfoKind.ReaderId:
                    rows = Sql.Read(
                        $"SELECT * " +
                        $"FROM READER " +
                        $"WHERE(READER_ID = '{keyword}') ");
                    break;

                case ReaderInfoKind.ReaderName:
                    rows = Sql.Read($"SELECT * " +
                        $"FROM READER " +
                        $"WHERE READER_NAME like '%{keyword}%'");
                    break;

                case ReaderInfoKind.PhoneNumber:
                    rows = Sql.Read($"SELECT * " +
                        $"FROM READER " +
                        $"WHERE PHONE_NUMBER = {keyword}");
                    break;

                case ReaderInfoKind.ReaderIdCard:
                    rows = Sql.Read($"SELECT * " +
                        $"FROM READER " +
                        $"WHERE READER_IDCARD = '{keyword}'");
                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }

            //当SQL文的搜索结果为0时
            if (rows.Count == 0)
            {
                throw new Exception("搜索结果为0。");
            }

            var lists = new List<Reader>();

            for (int i = 0; i < rows.Count; i++)
            {
                lists.Add(new Reader(
                    Convert.ToString(rows[i]["READER_ID"]),
                    Convert.ToString(rows[i]["READER_NAME"]),
                    Convert.ToString(rows[i]["READER_PASSWORD"]),
                    (long)Convert.ToInt64(rows[i]["PHONE_NUMBER"]),
                    Convert.ToString(rows[i]["READER_IDCARD"])
                    ));
            }

            return lists;
        }

        /// <summary>
        /// 编辑帐户信息功能
        /// </summary>
        /// <param name="reader">相应的读者的ValueObject</param>
        /// <param name="infoKind">要修改的信息标识</param>
        /// <param name="updated">修改后的内容</param>
        public void UpdateReaderInfo(string readerId, string readerName, long phoneNumber, string readerIdCard)
        {
            Sql.Execute(
                $"UPDATE READER SET " +
                $"READER_ID ='{readerId}', " +
                $"READER_NAME ='{readerName}', " +
                $"PHONE_NUMBER ={phoneNumber}, " +
                $"READER_IDCARD ='{readerIdCard}' " +
                $"WHERE READER_ID = '{readerId}' ");
        }

        /// <summary>
        /// 查看读者当前借阅功能
        /// </summary>
        /// <param name="readerId">读者帐号</param>
        /// <returns>读者的当前借阅信息（List型）</returns>
        public List<ReaderCuurrentBorrowing> SearchReadersCurrentBorrowing(string readerId)
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT BORROW.READER_ID," +
                $" BORROW.BOOK_ID, BOOK.BOOK_NAME," +
                $" BORROW.BORROW_DATE, BORROW.LATEST_RETURN_DATE," +
                $" BORROW.CONTINUED_STATE" +
                $" FROM BORROW, BOOK " +
                $"WHERE BORROW.BOOK_ID = BOOK.BOOK_ID " +
                $"AND(BORROW.READER_ID = '{readerId}')");

            //当SQL文的搜索结果为0时
            if (rows.Count == 0)
            {
                throw new Exception("搜索结果为0。");
            }

            var lists = new List<ReaderCuurrentBorrowing>();

            for (int i = 0; i < rows.Count; i++)
            {
                //判断续借状态
                string identifyContinuedState = Convert.ToString(rows[i]["BORROW.CONTINUED_STATE"]);

                ContinuedStates continuedState = EnumHelper.ToEnum<ContinuedStates>(identifyContinuedState);

                lists.Add(
                    new ReaderCuurrentBorrowing(
                        Convert.ToString(rows[i]["BORROW.READER_ID"]),
                        Convert.ToString(rows[i]["BORROW.BOOK_ID"]),
                        Convert.ToString(rows[i]["BOOK.BOOK_NAME"]),
                        (DateTime)rows[i]["BORROW.BORROW_DATED"],
                        (DateTime)rows[i]["BORROW.LATEST_RETURN_DAT"],
                        continuedState));
            }

            return lists;
        }

        /// <summary>
        /// 查看读者借阅历史功能
        /// </summary>
        public DataRowCollection SearchBorrowedHistory(string readerId)
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT BORROW.READER_ID," +
                $" BORROW.BOOK_ID, BOOK.BOOK_NAME," +
                $" BORROW.BORROW_DATE, BORROW.LATEST_RETURN_DATE," +
                $" BORROW.CONTINUED_STATE" +
                $" FROM BORROW, BOOK " +
                $"WHERE BORROW.BOOK_ID = BOOK.BOOK_ID " +
                $"AND(BORROW.READER_ID = '{readerId}')");

            //当SQL文的搜索结果为0时
            if (rows.Count == 0)
            {
                throw new Exception("搜索结果为0。");
            }

            return rows;
        }

        /// <summary>
        /// 发送通知功能
        /// </summary>
        public void SendMessage(string messageId, string readerId, string message)
        {
            Sql.Execute(
                    $"INSERT INTO MESSAGE " +
                    $"(READER_ID, MESSAGE, MESSAGE_TIME, MESSAGE_ID) " +
                    $"VALUES('{readerId}'," +
                    $"'{readerId}'," +
                    $"'{Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd")}'," +
                    $"'{messageId}')");
        }

        /// <summary>
        /// 查看过期未还书的读者名单功能
        /// </summary>
        public DataRowCollection SearchTheListOfReadersWhoHaveNotReturnedBooks()
        {
            DataRowCollection rows = Sql.Read(
                $"SELECT * " +
                $"FROM BORROW " +
                $"WHERE(LATEST_RETURN_DATE < TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd")}','yyyy/MM/dd'))");
            return rows;
        }

        /// <summary>
        /// 查看所有读者信息
        /// </summary>
        /// <returns></returns>
        public DataRowCollection ViewAllReaderInformation()
        {
            DataRowCollection readers = Sql.Read($"SELECT READER_ID, READER_NAME, PHONE_NUMBER, READER_IDCARD FROM READER");
            return readers;
        }
    }
}
