using Chinese.PinYin;
using NUnit.Framework;

namespace TestChinese
{
    public class Tests
    {
        ChinesePinyin current;

        [SetUp]
        public void Setup()
        {
            current = ChinesePinyin.Current;
        }

        [Test]
        public void TestToAlias()
        {
            string str = current.ConvertToAlias("我是中国人。");
            Assert.AreEqual(str, "WSZGR");
        }

        [Test]
        public void TestSadDiff()
        {
            string str = current.ConvertToAlias("重庆");
            Assert.AreEqual(str, "CQ");
        }

        [Test]
        public void TestSadDiff2()
        {
            string str = current.ConvertToAlias("重量", true);
            Assert.AreEqual(str, "ZL");
        }
    }
}