using System.Collections.Generic;

public interface IPoolObject
{
    void SwitchActive(bool value);

    bool IsActive();
}
