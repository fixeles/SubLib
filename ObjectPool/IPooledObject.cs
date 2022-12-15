namespace UtilsSubmodule.ObjectPool
{
    public interface IPooledObject
    {
        void GetPooled();
        void ReleasePooled();
        bool IsActive();
        void Prepare();
    }
}