using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public GameObject currentQuestWindow;
    private PlayerManager playerManager;
    public GameObject questWindow;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI currentQuestTitle;
    public TextMeshProUGUI currentQuestDescription;
    public TextMeshProUGUI currentQuestExperienceText;
    public TextMeshProUGUI currentQuestGoldText;
    public TextMeshProUGUI currentQuestStatusText;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        questTitle.text = quest.title;
        questDescription.text = quest.description;
        experienceText.text = quest.experienceReward.ToString();
        goldText.text = quest.goldReward.ToString();
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
        currentQuestWindow.SetActive(true);
        
        currentQuestTitle.text = quest.title;
        currentQuestDescription.text = quest.description;
        currentQuestExperienceText.text = quest.experienceReward.ToString();
        currentQuestGoldText.text = quest.goldReward.ToString();
        currentQuestStatusText.text = quest.goal.currentAmount.ToString() + "/" + quest.goal.requiredAmount.ToString();
        playerManager.quest = quest;
    }

    public void Cancel()
    {
        questWindow.SetActive(false);
    }
}
