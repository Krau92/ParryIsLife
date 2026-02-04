using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class BossData
{
    public string bossName;
    public GameObject bossPrefab;
}

[CreateAssetMenu(fileName = "BossBank", menuName = "Scriptable Objects/BossBank")]
public class BossBank : ScriptableObject
{
    [SerializeField] private List<BossData> bossPrefabs;

    public GameObject GetBossPrefab(string bossName)
    {
        foreach (BossData bossData in bossPrefabs)
        {
            if (bossData.bossName == bossName)
            {
                Debug.Log("Boss prefab found for name: " + bossName);
                return bossData.bossPrefab;
            }
        }
        Debug.LogWarning("Boss prefab not found for name: " + bossName);
        return null;
    }
}
