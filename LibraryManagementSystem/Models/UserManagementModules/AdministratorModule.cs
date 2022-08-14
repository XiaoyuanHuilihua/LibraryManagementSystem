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
        public enum SearchKind
        {
            ReaderId,
            ReaderName,
            PhoneNumber,
            ReaderIdCard,
        }

        public AdministratorModule()
        {
            //处理
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchKind">标识的分类</param>
        /// <param name="keyword">搜索词</param>
        /// <returns></returns>
        public List<Reader> SearchReaderInfo(SearchKind searchKind, dynamic keyword)
        {
            DataRowCollection rows;
            switch (searchKind)
            {
                case SearchKind.ReaderId:
                    rows = Sql.Read("SELECT * FROM reader WHERE(READER_ID = '@0') ", (int)keyword);
                    break;

                case SearchKind.ReaderName:
                    rows = Sql.Read("SELECT * FROM reader WHERE reader_name like '%@0%'", keyword);
                    break;

                case SearchKind.PhoneNumber:
                    rows = Sql.Read("SELECT * FROM reader WHERE phone_number = @0", keyword);
                    break;

                case SearchKind.ReaderIdCard:
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
    }
}
