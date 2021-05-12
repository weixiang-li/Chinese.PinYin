using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese.PinYin.Entity
{
    /// <summary>
    /// 汉语拼音
    /// </summary>
    internal class Chinese
    {
        internal static Dictionary<char, PinYinList> ChineseDict { get; private set; }


        /// <summary>
        /// 加载字典
        /// </summary>
        /// <returns></returns>
        internal static async Task ParseDictAsync()
        {
            var sources = await Resources.Resources.ReadResouceAsync();

            ChineseDict = sources.Select(s => PinYin.Parse(s))
                .GroupBy(x => x.Chinese[0]).ToDictionary(x => x.Key, x => new PinYinList(x.ToList()));

        }

    }

    internal class PinYinList
    {
        /// <summary>
        /// 同一组拼音，超过该数值，则转换为字典数据
        /// </summary>
        private const int ListMaxCapacity = 18;
        private bool ListMode = true;

        /// <summary>
        /// 拼音
        /// </summary>
        private List<PinYin> pinYins = new List<PinYin>();

        private Dictionary<char, List<PinYin>> pinYinDict;

        /// <summary>
        /// 单子拼音
        /// </summary>
        internal PinYin DefaultPinyin
        {
            get; private set;
        }

        internal PinYinList(List<PinYin> pinYinList)
        {
            this.pinYins = pinYinList;
            this.ParseToDict();
        }

        internal void ParseToDict()
        {
            if (this.pinYins.Count(x => x.Length == 1) <= 1)
            {
                this.DefaultPinyin = pinYins.FirstOrDefault(x => x.Length == 1);
            }
            else
            {
                //多音字, 取使用率最多的。
                this.DefaultPinyin = this.pinYins.Select(x => new
                {
                    Value = x,
                    Key = x.Pinyin.Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries)[0]
                })
                .GroupBy(x => x.Key)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Where(y => y.Value.Length == 1).Select(y => y.Value).FirstOrDefault())
                .Where(x => x != null)
                .FirstOrDefault();
            }
            pinYins.RemoveAll(x => x.Length == 1);

            // 转换成字典，加速查询速度
            if (pinYins.Count > ListMaxCapacity)
            {
                this.ListMode = false;
                List<PinYin> tmpList = pinYins.FindAll(x => x.Length > 1);

                pinYins.Clear();
                pinYins = null;

                pinYinDict = new Dictionary<char, List<PinYin>>();
                foreach (var item in tmpList)
                {
                    char c = item.Chinese[1];
                    if (!pinYinDict.ContainsKey(c))
                    {
                        pinYinDict[c] = new List<PinYin>();
                    }
                    pinYinDict[c].Add(item);
                }
            }
        }

        internal PinYin FindMax(string chinese, int i)
        {
            PinYin pinYin = this.DefaultPinyin;
            if (this.ListMode)
            {
                foreach (var item in this.pinYins)
                {
                    if (pinYin.Length < item.Length && (i + item.Length) <= chinese.Length)
                    {
                        string s = chinese.Substring(i, item.Length);
                        if (s == item.Chinese)
                        {
                            pinYin = item;
                        }
                    }
                }
            }
            else
            {
                if ((i + 2) <= chinese.Length)
                {
                    char c = chinese[i + 1];
                    if (pinYinDict.ContainsKey(c))
                    {
                        var tmpList = pinYinDict[c];

                        foreach (var item in tmpList)
                        {
                            if (pinYin.Length < item.Length && (i + item.Length) <= chinese.Length)
                            {
                                string s = chinese.Substring(i, item.Length);
                                if (s == item.Chinese)
                                {
                                    pinYin = item;
                                }
                            }
                        }
                    }
                }
            }

            return pinYin;
        }
        

    }
}
