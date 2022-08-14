using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    public class Administrator : ValueObject<Administrator>
    {
        public Administrator(string adminId, string adminName, string adminPassword)
        {
            AdminId = adminId;
            AdminName = adminName;
            AdminPassword = adminPassword;
        }

        /// <summary>
        /// 管理员帐号
        /// </summary>
        public string AdminId { get; }

        /// <summary>
        /// 管理员姓名
        /// </summary>
        public string AdminName { get; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        public string AdminPassword { get; }

        protected override bool EqualsCore(Administrator other)
        {
            throw new NotImplementedException();
        }
    }
}
