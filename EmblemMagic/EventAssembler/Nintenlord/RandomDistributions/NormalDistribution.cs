using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.RandomDistributions
{
    public sealed class NormalDistribution : IDistribution<double>
    {
        readonly UniformDistribution uin;
        readonly WeibullDistribution weib;
        readonly double average;
        readonly double variance;
        double? next;

        public NormalDistribution(double average, double variance, Random random)
        {
            this.average = average;
            this.variance = variance;
            uin = UniformDistribution.FromStartLength(random, 0, Math.PI * 2);
            weib = new WeibullDistribution(random, 0, 1);//TODO Put corrent values
        }

        #region IDistribution<double> Members

        public double NextValue()
        {
            double result;
            if (next == null)
            {
                var uinD = uin.NextValue();
                var weibD = weib.NextValue();
                next = weibD * Math.Cos(uinD);
                result = weibD * Math.Sin(uinD);
            }
            else
            {
                result = next.Value;
                next = null;
            }
            return result;
        }

        #endregion
    }
}
