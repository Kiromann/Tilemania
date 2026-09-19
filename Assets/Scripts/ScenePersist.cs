using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numberGamePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numberGamePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetGamePersist()
    {
        Destroy(gameObject);
    }
}
