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
            string str = current.ConvertToAlias("�����й��ˡ�");
            Assert.AreEqual(str, "WSZGR");
        }

        [Test]
        public void TestSadDiff()
        {
            string str = current.ConvertToAlias("����");
            Assert.AreEqual(str, "CQ");
        }

        [Test]
        public void TestSadDiff2()
        {
            string str = current.ConvertToAlias("����", true);
            Assert.AreEqual(str, "ZL");
        }
    }
}