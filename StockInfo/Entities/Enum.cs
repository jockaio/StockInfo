using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfo.Entities
{
    public enum StrategyType
    {
        Ricoschett
    }

    public enum OutputSizeType
    {
        Compact,
        Full
    }

    static class OutputSizeTypeExtensions
    {
        public static string ToString(this OutputSizeType outputSizeType)
        {
            return outputSizeType.ToString().ToLower();
        }
    }
}
