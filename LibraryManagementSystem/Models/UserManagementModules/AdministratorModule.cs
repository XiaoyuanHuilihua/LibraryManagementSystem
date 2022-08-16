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
        /// 构造函数
        /// </summary>
        public AdministratorModule()
        {
            //处理
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
                    rows = Sql.Read("SELECT * FROM reader WHERE(READER_ID = '@0') ", keyword);
                    break;

                case ReaderInfoKind.ReaderName:
                    rows = Sql.Read("SELECT * FROM reader WHERE reader_name like '%@0%'", keyword);
                    break;

                case ReaderInfoKind.PhoneNumber:
                    rows = Sql.Read("SELECT * FROM reader WHERE phone_number = @0", keyword);
                    break;

                case ReaderInfoKind.ReaderIdCard:
                    rows = Sql.Read("SELECT * FROM reader WHERE eader_idcard = @0", keyword);
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
                lists.Add(new Reader(rows[i]["reader_id"].ToString(), rows[i]["reader_name"].ToString(), rows[i]["reader_password"].ToString(), (uint)rows[i]["phone_number"], rows[i]["reader_idcard"].ToString()));
            }

            return lists;
        }

        /// <summary>
        /// 编辑帐户信息功能
        /// </summary>
        /// <param name="reader">相应的读者的ValueObject</param>
        /// <param name="infoKind">要修改的信息标识</param>
        /// <param name="updated">修改后的内容</param>
        public void UpdateReaderInfo(Reader reader, ReaderInfoKind infoKind, Object updated)
        {
            switch (infoKind)
            {
                case (ReaderInfoKind.ReaderId):
                    reader.ReaderId = (string)updated;
                    break;

                case (ReaderInfoKind.ReaderName):
                    reader.ReaderName = (string)updated;
                    break;

                case (ReaderInfoKind.Password):
                    reader.Password = (string)updated;
                    break;

                case (ReaderInfoKind.PhoneNumber):
                    reader.PhoneNumber = (long)updated;
                    break;

                case (ReaderInfoKind.ReaderIdCard):
                    reader.ReaderIdCard = (string)updated;
                    break;

                default:
                    throw new Exception();
                    break;
            }
        }

        /// <summary>
        /// 查看读者当前借阅功能
        /// </summary>
        /// <param name="readerId">读者帐号</param>
        /// <returns>读者的当前借阅信息（List型）</returns>
        public List<ReaderCuurrentBorrowing> SearchReadersCurrentBorrowing(string readerId)
        {
            DataRowCollection rows = Sql.Read("SELECT BORROW.READER_ID, BORROW.BOOK_ID, BOOK.BOOK_NAME, BORROW.BORROW_DATE, BORROW.LATEST_RETURN_DATE, BORROW.CONTINUED_STATE FROM BORROW, BOOK WHERE BORROW.BOOK_ID = BOOK.BOOK_ID AND(BORROW.READER_ID = '@0')", readerId);

            //当SQL文的搜索结果为0时
            if (rows.Count == 0)
            {
                throw new Exception("搜索结果为0。");
            }

            var lists = new List<ReaderCuurrentBorrowing>();

            for (int i = 0; i < rows.Count; i++)
            {
                lists.Add(new ReaderCuurrentBorrowing(rows[i]["BORROW.READER_ID"].ToString(), rows[i]["BORROW.BOOK_ID"].ToString(), rows[i]["BOOK.BOOK_NAME"].ToString(), (DateTime)rows[i]["BORROW.BORROW_DATED"], (DateTime)rows[i]["BORROW.LATEST_RETURN_DAT"], (ContinuedStates)rows[i]["BORROW.CONTINUED_STATE"]));
            }

            return lists;
        }
    }
}
