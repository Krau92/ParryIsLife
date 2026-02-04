using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CombatResult
{
    public string bossName;
    public bool completed;
    public int maxScore;
    public int threatLevel;
}

[CreateAssetMenu(fileName = "SaveStateSO", menuName = "Scriptable Objects/SaveStateSO")]
public class SaveStateSO : ScriptableObject
{
    [SerializeField] private List<CombatResult> combatResults = new List<CombatResult>();
    public int saveSlot;

    public int GetTotalThreatLevel()
    {
        int totalThreatLevel = 0;
        foreach (var result in combatResults)
        {
            if (result.completed)
            {
                totalThreatLevel += result.threatLevel;
            }
        }
        return totalThreatLevel;
    }

    public void AddOrUpdateCombatResult(CombatResult newResult)
    {
        CombatResult existingResult = combatResults.Find(result => result.bossName == newResult.bossName);
        if (existingResult != null)
        {
            existingResult.completed |= newResult.completed;
            existingResult.maxScore = Mathf.Max(existingResult.maxScore, newResult.maxScore);
            existingResult.threatLevel = Mathf.Max(existingResult.threatLevel, newResult.threatLevel);
        }
        else
        {
            combatResults.Add(newResult);
        }

        ManageSaveData.SaveData(this.saveSlot, this);
    }



}
