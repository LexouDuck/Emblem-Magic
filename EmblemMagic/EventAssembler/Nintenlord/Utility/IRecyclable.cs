namespace Nintenlord.Utility
{
    public interface IRecyclable
    {
        bool Used
        {
            get;
        }
    }

    public interface IRecycler
    {
        void Recycle(IRecyclable item);
    }
}
