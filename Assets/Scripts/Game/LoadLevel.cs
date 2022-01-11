using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    void Awake()
    {
        Instantiate(Resources.Load(MemoryManager.GetGameResources()));
    }
}