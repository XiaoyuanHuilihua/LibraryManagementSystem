using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Models.ReaderModule;
using LibraryManagementSystem.Models.UserManagementModules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace LibraryManagementSystem.Tests
{
    /// <summary>
    /// 测试ReaderModule类操作(函数)的测试类
    /// </summary>
    [TestClass]
    public class ReaderModuleTest
    {
        private ReaderModule _ReaderModule = new ReaderModule();
        private AdministratorModule _administratorModule = new AdministratorModule();

        /// <summary>
        /// 读者借书证办理或读者注册
        /// </summary>
        [TestMethod]
        public void 读者借书证办理或读者注册()
        {
            _ReaderModule.ApplyLibraryCard("李春华", "123456", 15151515555, "4568");
            var reader = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderName, "李春华")[0];
            Assert.AreEqual("李春华", reader.ReaderName);
            Assert.AreEqual("123456", reader.Password);
            Assert.AreEqual(15151515555, reader.PhoneNumber);
            Assert.AreEqual("4568", reader.ReaderIdCard);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        [TestMethod]
        public void 修改用户密码()
        {
            Boolean mark = _ReaderModule.AlterPassword("17", "123456", "111111");
            var reader = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderId, "17")[0];
            Assert.AreEqual("111111", reader.Password);
        }

        /// <summary>
        /// 查看个人信息
        /// </summary>
        [TestMethod]
        public void 查看个人信息()
        {
            _ReaderModule.ApplyLibraryCard("王静", "123456", 15151515555, "sample12345");//readerId = 9
            string readerId = "9";
            Console.WriteLine("展示个人信息");
            DataRow reader = _ReaderModule.ViewReaderInformation($"{readerId}");
            Assert.AreEqual("王静", reader[1]);
            Assert.AreEqual("123456", reader[2]);
            Assert.AreEqual(15151515555, System.Convert.ToInt64(reader[3]));
            Assert.AreEqual("sample12345", reader[4]);

            Sql.Execute(
                $"DELETE FROM READER " +
                $"WHERE READER_ID = '{readerId}'");
        }

        /// <summary>
        /// 编辑个人信息
        /// </summary>
        [TestMethod]
        public void 编辑个人信息()
        {
            _ReaderModule.EditReaderInformation("17", "郑春华", 123456789, "123456");
            var reader = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderId, "17")[0];
            Assert.AreEqual("郑春华", reader.ReaderName);
            Assert.AreEqual(123456789, reader.PhoneNumber);
            Assert.AreEqual("123456", reader.ReaderIdCard);
        }

        /// <summary>
        /// 查看借阅信息
        /// </summary>
        [TestMethod]
        public void 查看借阅信息()
        {
            ///查看历史借阅信息
            DataRowCollection old_records = _ReaderModule.SearchBorrowedHistory("16");
            for (var i = 0; i < old_records.Count; i++)
            {
                Assert.AreEqual("3", old_records[i][1]);///book_id
                Assert.AreEqual(DateTime.Parse("2022/8/19"), old_records[i][2]);
                Assert.AreEqual(DateTime.Parse("2022/8/19"), old_records[i][3]);
                Assert.AreEqual("r4", old_records[i][4]);
            }
            ///查看当前借阅信息

            DataRowCollection cur_records = _ReaderModule.SearchBorrowingHistory("16");
            for (var i = 0; i < cur_records.Count; i++)
            {
                Assert.AreEqual("3", cur_records[i][1]);///book_id
                Assert.AreEqual(DateTime.Parse("2022/8/12"), cur_records[i][2]);///借书时间
                Assert.AreEqual(DateTime.Parse("2022/8/26"), cur_records[i][3]);///还书时间
                Assert.AreEqual("b4", cur_records[i][4]);///borrow_id
                Assert.AreEqual(0, System.Convert.ToInt32(cur_records[i][5]));///continued_id
            }
        }

        /// <summary>
        /// 读者留言功能
        /// </summary>
        [TestMethod]
        public void 读者留言功能()
        {
            _ReaderModule.ReaderComment("17", "3", "this is interesting");
            DataRow comment = Sql.Read("SELECT * FROM COMMENTS WHERE READER_ID='17'")[0];
            Assert.AreEqual("17", comment[0]);
            Assert.AreEqual("3", comment[1]);
            Assert.AreEqual("this is interesting", comment[2]);
            Assert.AreEqual(DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")), comment[3]);
            Assert.AreEqual("1", comment[4]);

            Sql.Execute(
                $"DELETE FROM BOOK_RESERVE " +
                $"WHERE BOOK_RESERVE_ID = '{comment[0]}'");
        }

        /// <summary>
        /// 书籍预约
        /// </summary>
        [TestMethod]
        public void 书籍预约()
        {
            _ReaderModule.ReserveBook("17", "3");
            DataRow BR = Sql.Read("SELECT * FROM BOOK_RESERVE WHERE BOOK_RESERVE_ID='br4'")[0];
            Assert.AreEqual("br4", BR[0]);
            Assert.AreEqual("17", BR[1]);
            Assert.AreEqual("3", BR[2]);
            Assert.AreEqual(DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")), BR[3]);
            Assert.AreEqual(DateTime.Parse(DateTime.Now.AddDays(14).ToString("yyyy/MM/dd")), BR[4]);

            Sql.Execute(
                $"DELETE FROM BOOK_RESERVE " +
                $"WHERE BOOK_RESERVE_ID = '{BR[0]}'");
        }
    }
}
