using UnityEngine;
using System.Collections.Generic;
using System;


[CreateAssetMenu(fileName = "BossBank", menuName = "Scriptable Objects/BossBank")]
public class BossBank : ScriptableObject
{
    [SerializeField] private List<GameObject> bossPrefabs;

    public GameObject GetBossPrefab(string bossName)
    {
        foreach (GameObject bossPrefab in bossPrefabs)
        {
            Boss bossComponent = bossPrefab.GetComponent<Boss>();
            if (bossComponent.bossName == bossName)
            {
                Debug.Log("Boss prefab found for name: " + bossName);
                return bossPrefab;
            }
        }
        Debug.LogWarning("Boss prefab not found for name: " + bossName);
        return null;
    }
}
