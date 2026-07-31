using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Tooltip("Set bosses in order")]
    public List<PinpointInteraction> bossPinpoints;

    void OnEnable()
    {
        GameEvents.OnResultsClosed += CheckAllPinpoints;
    }

    void OnDisable()
    {
        GameEvents.OnResultsClosed -= CheckAllPinpoints;
    }

    void Start()
    {
        CheckAllPinpoints();
#if UNITY_EDITOR
        ValidatePinpoints();
#endif
    }

#if UNITY_EDITOR
    void ValidatePinpoints()
    {
        GameObject[] allPinpoints = GameObject.FindGameObjectsWithTag("Pinpoint");
        foreach (GameObject go in allPinpoints)
        {
            PinpointInteraction pi = go.GetComponent<PinpointInteraction>();
            if (pi != null && !bossPinpoints.Contains(pi))
            {
                Debug.LogError($"PinpointInteraction en '{go.name}' no está asignado en la lista bossPinpoints de MapManager.", go);
            }
        }
    }
#endif

    void CheckAllPinpoints()
    {
        for(int i = 0; i < bossPinpoints.Count; i++)
        {
            bossPinpoints[i].CheckRequirement();
        }
    }
}
