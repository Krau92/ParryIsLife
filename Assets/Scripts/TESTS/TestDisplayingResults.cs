using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestDisplayingResults : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private List<Image> alertDisplays;
    [SerializeField] private Color alertColor = new Color(1f, 0.5f, 0.5f, 0.8f);
    [SerializeField] private Color alertOffColor = new Color(1f, 0.5f, 0.5f, 0f);
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text scoreText;
    const float waitBeforeNextScreen = 1f;
    float timer = 0f;


    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        InputManager.anyInputReceived += HandleAnyInputReceived;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        InputManager.anyInputReceived -= HandleAnyInputReceived;
    }

    void Update()
    {
        if (resultPanel.activeSelf)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.ShowingResults)
        {
            timer = waitBeforeNextScreen;
            resultPanel.SetActive(true);
            GetCombatResults();
        }
        else
        {
            resultPanel.SetActive(false);
        }
    }

    private void HandleAnyInputReceived()
    {
        if (resultPanel.activeSelf && timer <= 0f)
        {
            resultPanel.SetActive(false);
            CombatEvents.OnResultsClosed?.Invoke();
        }
    }

    private void GetCombatResults()
    {
        CombatResult results = combatManager.CombatResults;
        if(!results.completed)
        {
            resultText.text = "You Failed!";
            scoreText.text = " ";
            foreach (var display in alertDisplays)
            {
                display.color = alertOffColor;
            }
        }
        else
        {
            resultText.text = $" {results.bossName} Defeated";
            string scoreString = results.maxScore.ToString();
            scoreText.text = $"Score: {scoreString}";
            int alertLevel = results.threatLevel;
            for (int i = 0; i < alertDisplays.Count; i++)
            {
                alertDisplays[i].color = i < alertLevel ? alertColor : alertOffColor;
            }
        }
    }
}
