using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chinese.PinYin.Entity;

namespace Chinese.PinYin
{
    public class ChinesePinyin
    {
        private static Task initTask = null;
        private static ChinesePinyin instance;

        static ChinesePinyin()
        {
            initTask = Entity.Chinese.ParseDictAsync();
        }

        public static ChinesePinyin Current
        {
            get
            {
                if (instance == null)
                {
                    lock (initTask)
                    {
                        using (initTask)
                        {
                            initTask.Wait();
                            initTask = null;
                        }
                        if (instance == null)
                        {
                            instance = new ChinesePinyin();
                        }
                    }
                }
                return instance;
            }
        }

        public List<Entity.PinYin> ConvertToPinYin(string chinese)
        {
            List<Entity.PinYin> result = new List<Entity.PinYin>();

            for (int i = 0; i < chinese.Length; i++)
            {
                char c = chinese[i];

                if (Entity.Chinese.ChineseDict.ContainsKey(c))
                {
                    PinYinList plist = Entity.Chinese.ChineseDict[c];
                    Entity.PinYin pinYin = plist.FindMax(chinese, i);
                    result.Add(pinYin);

                    i = i + pinYin.Length - 1;
                }
                else
                {
                    // 舍弃非汉字类字符
                    //result.Add(new Entity.PinYin()
                    //{
                    //    Pinyin = Convert.ToString(c),
                    //    Chinese = Convert.ToString(c)
                    //});
                }
            }

            return result;
        }

        /// <summary>
        /// 汉字转拼音首字母，大写
        /// </summary>
        /// <param name="chinese">汉字</param>
        /// <param name="upper">大写</param>
        /// <returns></returns>
        public string ConvertToAlias(string chinese, bool upper = true)
        {
            List<Entity.PinYin> result = ConvertToPinYin(chinese);

            return string.Join(string.Empty,
                result.Select(x => string.Join(string.Empty,
                    x.Pinyins.Select(p => (upper ? p.ToUpper() : p)[0]).ToArray())
                ).ToArray());
        }

    }
}
