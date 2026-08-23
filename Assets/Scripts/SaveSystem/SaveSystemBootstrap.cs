using UnityEngine;

public class SaveSystemBootstrap : MonoBehaviour
{
    private void Awake()
    {
        SaveService.Instance.Load();
    }
}
