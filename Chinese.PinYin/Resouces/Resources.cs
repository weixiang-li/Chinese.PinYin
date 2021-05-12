using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chinese.PinYin.Resources
{
    /// <summary>
    /// 读取内嵌资源
    /// </summary>
    internal class Resources
    {

        /// <summary>
        /// 读取词库
        /// </summary>
        internal static async Task<IEnumerable<string>> ReadResouceAsync()
        {
            return await Task.Run(ReadResouce);
        }

        /// <summary>
        /// 读取嵌入的词库压缩文件
        /// </summary>
        internal static IEnumerable<string> ReadResouce()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (string resName in asm.GetManifestResourceNames())
            {
                using (Stream sm = asm.GetManifestResourceStream(resName))
                {
                    sm.Position = 0;
                    using (StreamReader reader = new StreamReader(sm))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                yield return line;
                            }
                        }
                    }
                }
            }
        }
    }
}
