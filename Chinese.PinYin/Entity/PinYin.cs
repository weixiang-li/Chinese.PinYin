using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese.PinYin.Entity
{
    /// <summary>
    /// 拼音和下一个汉字
    /// </summary>
    public class PinYin
    {
        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin { get; internal protected set; }
        /// <summary>
        /// 汉字
        /// </summary>
        public string Chinese { get; internal protected set; }
        /// <summary>
        /// 汉字长度
        /// </summary>
        public int Length
        {
            get { return this.Chinese.Length; }
        }
        /// <summary>
        /// 拆分成单个拼音
        /// </summary>
        public List<string> Pinyins
        {
            get
            {
                return Pinyin?.Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        internal static PinYin Parse(string res)
        {
            int blankIndex = res.IndexOf(' ');
            PinYin pinYin = new PinYin()
            {
                Pinyin = res.Substring(0, blankIndex),
                Chinese = res.Substring(blankIndex + 1)
            };
            return pinYin;
        }

    }

}
