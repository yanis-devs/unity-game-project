using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;

    private void Update()
    {
        if (mapManager != null)
        {
            mapManager.ManageMap();
        }
        else
        {
            Debug.LogError("GameManager: mapManager reference is missing!");
        }
    }
}