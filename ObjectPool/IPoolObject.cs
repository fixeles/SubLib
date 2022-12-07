namespace UtilsSubmodule.ObjectPool
{
    public interface IPoolObject
    {
        void SwitchActive(bool value);

        bool IsActive();
    }
}
