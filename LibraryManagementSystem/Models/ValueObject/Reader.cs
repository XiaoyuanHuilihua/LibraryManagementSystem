using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    public class Reader : ValueObject<Reader>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="readerId">读者帐号</param>
        /// <param name="readerName">读者名</param>
        /// <param name="password">读者密码</param>
        /// <param name="phoneNumber">读者电话号码</param>
        /// <param name="readerIdCard">读者身份证号</param>
        public Reader(
            string readerId,
            string readerName,
            string password,
            long phoneNumber,
            string readerIdCard) : base()
        {
            ReaderId = readerId;
            ReaderName = readerName;
            Password = password;
            PhoneNumber = phoneNumber;
            ReaderIdCard = readerIdCard;
        }

        public Reader()
        {
        }

        /// <summary>
        /// 读者帐号
        /// </summary>
        public string ReaderId { get; set; }

        /// <summary>
        /// 读者名
        /// </summary>
        public string ReaderName { get; set; }

        /// <summary>
        /// 读者密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 读者电话号码
        /// </summary>
        public long PhoneNumber { get; set; }

        /// <summary>
        /// 读者身份证号
        /// </summary>
        public string ReaderIdCard { get; set; }

        /// <inheritdoc/>
        protected override bool EqualsCore(Reader other)
        {
            throw new NotImplementedException();
        }
    }
}
