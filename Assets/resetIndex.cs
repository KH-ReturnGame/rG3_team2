using UnityEngine;

public class resetIndex : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("CheckpointIndex", 0);
    }
}
