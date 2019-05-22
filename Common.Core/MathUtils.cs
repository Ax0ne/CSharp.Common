using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Core
{
    /// <summary>
    /// 数学相关的工具类
    /// </summary>
    public static class MathUtils
    {
        #region Private Fields
        /// <summary>
        ///     最小金额1分
        /// </summary>
        private const int MIN = 1;

        ///// <summary>
        /////     最大金额1000RMB
        ///// </summary>
        //private const int MAX = 100000;

        #endregion

        /// <summary>
        ///     生成随机数（类似微信群红包）
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="total">总金额（单位：分）100元=10000分</param>
        /// <returns></returns>
        public static List<int> GenerateRandomNumber(int count, int total)
        {
            if (total < MIN)
                throw new ArgumentOutOfRangeException(nameof(total) + ":不能超过最大或最小金额");
            if (MIN * count > total)
                throw new ArgumentOutOfRangeException(nameof(total) + $":总金额太小，最低平均{MIN / 100d}元");
            if (count == 1)
                return new List<int> { total };
            var result = new List<int>();
            var remainMoney = total;
            while (count >= 1)
            {
                if (count == 1)
                {
                    result.Add(remainMoney);
                    break;
                }

                var max = remainMoney / count * 2;
                var money = (int)(max * new Random().NextDouble() / 100 * 100);
                money = money < MIN ? MIN : money;
                result.Add(money);
                remainMoney -= money;
                --count;
            }

            return result.OrderBy(s => Guid.NewGuid()).ToList();
        }
    }
}
