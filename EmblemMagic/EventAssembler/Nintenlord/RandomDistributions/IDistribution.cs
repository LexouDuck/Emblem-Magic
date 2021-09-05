using System;
namespace Nintenlord.RandomDistributions
{
    public interface IDistribution<out T>
    {
        T NextValue();
    }
}
