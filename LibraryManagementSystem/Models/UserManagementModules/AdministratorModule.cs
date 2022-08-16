using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ValueObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

        public AdministratorModule()
        {
            //处理
        }

        /// <summary>
        ///搜索帐户功能
        /// </summary>
        /// <param name="ReaderInfoKind">标识的分类</param>
        /// <param name="keyword">搜索词</param>
        /// <returns></returns>
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

            //SQL文の検索結果がなかった時
            if (rows.Count == 0)
            {
                throw new Exception("検索結果は0でした。");
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
    }
}
