using System.Collections;

public interface IInitializable
{
    IEnumerator Initialize();

    bool IsInitialized { get; }
}