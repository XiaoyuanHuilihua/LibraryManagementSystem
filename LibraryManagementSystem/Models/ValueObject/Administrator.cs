using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models.ValueObject
{
    public class Administrator : ValueObject<Administrator>
    {
        public Administrator(string admin_Id, string admin_Name, string admin_Password)
        {
            Admin_Id = admin_Id;
            Admin_Name = admin_Name;
            Admin_Password = admin_Password;
        }

        /// <summary>
        /// 管理员帐号
        /// </summary>
        public string Admin_Id { get; }

        /// <summary>
        /// 管理员姓名
        /// </summary>
        public string Admin_Name { get; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        public string Admin_Password { get; }

        protected override bool EqualsCore(Administrator other)
        {
            throw new NotImplementedException();
        }
    }
}
