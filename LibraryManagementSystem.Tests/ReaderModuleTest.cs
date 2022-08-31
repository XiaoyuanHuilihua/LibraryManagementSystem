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

        [TestMethod]
        public void 修改用户密码()
        {
            Boolean mark = _ReaderModule.AlterPassword("17", "123456", "111111");
            var reader = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderId, "17")[0];
            Assert.AreEqual("111111", reader.Password);
        }

        [TestMethod]
        public void 查看个人信息()
        {
            Console.WriteLine("展示个人信息");
            DataRow reader = _ReaderModule.ViewReaderInformation("17");
            Assert.AreEqual("李春华", reader[1]);
            Assert.AreEqual("123456", reader[2]);
            Assert.AreEqual(15151515555, System.Convert.ToInt64(reader[3]));
            Assert.AreEqual("4568", reader[4]);
        }

        [TestMethod]
        public void 编辑个人信息()
        {
            _ReaderModule.EditReaderInformation("17", "郑春华", "123456789", "123456");
            var reader = _administratorModule.SearchReaderInfo(AdministratorModule.ReaderInfoKind.ReaderId, "17")[0];
            Assert.AreEqual("郑春华", reader.ReaderName);
            Assert.AreEqual(123456789, reader.PhoneNumber);
            Assert.AreEqual("123456", reader.ReaderIdCard);
        }

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

        [TestMethod]
        public void 查看座位功能()
        {
            DataRowCollection seats = _ReaderModule.ViewSeats();
            for (var i = 0; i < seats.Count; i++)
            {
                Assert.AreEqual(0, System.Convert.ToInt32(seats[i][1]));
                Assert.AreEqual("100", seats[i][2]);
            }
        }

        [TestMethod]
        public void 座位预约功能()
        {
            _ReaderModule.ReserveSeat("17", "s1", DateTime.Parse("2022/8/19"), DateTime.Parse("2022/8/19"));
            DataRow reserve = Sql.Read("SELECT * FROM SEAT_RESERVE WHERE SEAT_RESERVE_ID='r6'")[0];
            Assert.AreEqual("17", reserve[0]);///reader_id
            Assert.AreEqual("s1", reserve[1]);///seat_id
            Assert.AreEqual(DateTime.Parse("2022/8/19"), reserve[2]);
            Assert.AreEqual(DateTime.Parse("2022/8/19"), reserve[3]);
            Assert.AreEqual("r6", reserve[4]);

            DataRow seat = Sql.Read("SELECT SEAT_STATE FROM SEAT WHERE SEAT_ID='s1'")[0];
            Assert.AreEqual(1, System.Convert.ToInt32(seat[0]));
        }

        [TestMethod]
        public void 读者留言功能()
        {
            _ReaderModule.ReaderComment("17", "3", "this is interesting", DateTime.Parse("2022/8/6"));
            DataRow comment = Sql.Read("SELECT * FROM COMMENTS WHERE READER_ID='17'")[0];
            Assert.AreEqual("17", comment[0]);
            Assert.AreEqual("3", comment[1]);
            Assert.AreEqual("this is interesting", comment[2]);
            Assert.AreEqual(DateTime.Parse("2022/8/6"), comment[3]);
            Assert.AreEqual("1", comment[4]);
        }

        [TestMethod]
        public void 书籍预约()
        {
            _ReaderModule.ReserveBook("17", 3, DateTime.Parse("2022/8/6"), DateTime.Parse("2022/8/6"));
            DataRow BR = Sql.Read("SELECT * FROM BOOK_RESERVE WHERE BOOK_RESERVE_ID='br4'")[0];
            Assert.AreEqual("br4", BR[0]);
            Assert.AreEqual("17", BR[1]);
            Assert.AreEqual("3", BR[2]);
            Assert.AreEqual(DateTime.Parse("2022/8/6"), BR[3]);
            Assert.AreEqual(DateTime.Parse("2022/8/6"), BR[4]);
        }
    }
}
