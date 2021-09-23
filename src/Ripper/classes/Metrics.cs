using System;

namespace Ripper
{
    public class Metrics
    {
        public DateTime lastUpdate = DateTime.Now;
        public int previousCount = 0;
        public int keysPerSecond = 0;
        public int hightestRate = 0;

        public long contenderCount = 0;
    }
}