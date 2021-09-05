using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.RandomDistributions
{
    public sealed class FormulaeDistribution<T1,TResult> : IDistribution<TResult>
    {
        readonly Func<T1, TResult> formulae;
        readonly IDistribution<T1> baseDistribution;

        public FormulaeDistribution(IDistribution<T1> baseDistribution, Func<T1, TResult> formulae)
        {
            this.formulae = formulae;
            this.baseDistribution = baseDistribution;
        }

        #region IDistribution<T2> Members

        public TResult NextValue()
        {
            return formulae(baseDistribution.NextValue());
        }

        #endregion
    }

    public sealed class FormulaeDistribution<T1, T2, TResult> : IDistribution<TResult>
    {
        readonly Func<T1, T2, TResult> formulae;
        readonly IDistribution<T1> baseDistribution1;
        readonly IDistribution<T2> baseDistribution2;

        public FormulaeDistribution(
            IDistribution<T1> baseDistribution1, 
            IDistribution<T2> baseDistribution2,
            Func<T1, T2, TResult> formulae)
        {
            this.formulae = formulae;
            this.baseDistribution1 = baseDistribution1;
            this.baseDistribution2 = baseDistribution2;
        }

        #region IDistribution<T2> Members

        public TResult NextValue()
        {
            return formulae(baseDistribution1.NextValue(), baseDistribution2.NextValue());
        }

        #endregion
    }

    public sealed class FormulaeDistribution<T1, T2, T3, TResult> : IDistribution<TResult>
    {
        readonly Func<T1, T2, T3, TResult> formulae;
        readonly IDistribution<T1> baseDistribution1;
        readonly IDistribution<T2> baseDistribution2;
        readonly IDistribution<T3> baseDistribution3;

        public FormulaeDistribution(
            IDistribution<T1> baseDistribution1,
            IDistribution<T2> baseDistribution2,
            IDistribution<T3> baseDistribution3,
            Func<T1, T2, T3, TResult> formulae)
        {
            this.formulae = formulae;
            this.baseDistribution1 = baseDistribution1;
            this.baseDistribution2 = baseDistribution2;
            this.baseDistribution3 = baseDistribution3;
        }

        #region IDistribution<T2> Members

        public TResult NextValue()
        {
            return formulae(
                baseDistribution1.NextValue(),
                baseDistribution2.NextValue(), 
                baseDistribution3.NextValue()
                );
        }

        #endregion
    }
}
