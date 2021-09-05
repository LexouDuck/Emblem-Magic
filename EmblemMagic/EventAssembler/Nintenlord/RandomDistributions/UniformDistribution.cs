using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.RandomDistributions
{
    public sealed class UniformDistribution : IDistribution<double>
    {
        readonly double start;
        readonly double length;
        readonly Random random;

        private UniformDistribution(Random random, double start, double length)
        {
            this.start = start;
            this.length = length;
            this.random = random;
        }

        public double NextValue()
        {
            return start + random.NextDouble() * length;
        }

        public static UniformDistribution FromStartEnd(Random random, double start, double end)
        {
            return new UniformDistribution(random, start, end - start);
        }

        public static UniformDistribution FromStartLength(Random random, double start, double length)
        {
            return new UniformDistribution(random, start, length);
        }
    }
}
