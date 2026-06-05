using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIBossInfo : MonoBehaviour
{
    [SerializeField] SaveStateSO saveState;
    [SerializeField] PinpointInteraction pinpointInteraction;
    [SerializeField] TMP_Text bossName, maxScoreText;
    [SerializeField] GameObject completedIcon;
    int threatLevel;
    int maxScore;
    bool completed;
    
    [SerializeField] private List<Image> threatDisplays;
    [SerializeField] private Color threatColor = new Color(1f, 0.5f, 0.5f, 0.8f);
    [SerializeField] private Color threatOffColor = new Color(1f, 0.5f, 0.5f, 0f);

    void OnEnable()
    {
        UpdateUI();
        CombatEvents.OnBossDefeated += UpdateUI;
    }

    void OnDisable()
    {
        CombatEvents.OnBossDefeated -= UpdateUI;
    }

    void UpdateUI()
    {
        saveState.GetBossInfo(pinpointInteraction.bossName, out completed, out maxScore, out threatLevel);
        bossName.text = pinpointInteraction.bossName;
        completedIcon.SetActive(completed);
        for (int i = 0; i < threatDisplays.Count; i++)
        {
            threatDisplays[i].color = i < threatLevel ? threatColor : threatOffColor;
        }

        maxScoreText.text = $"Max Score: {maxScore}";
    }
}
