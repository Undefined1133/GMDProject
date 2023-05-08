using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;
    public Item itemReward;
    public bool isActive;
    public QuestGoal goal;
    public PlayerManager playerManager;
    public GameAudioManager gameAudioManager;
    public GameObject currentQuestWindow;
 

    private void SetPlayerManager()
    {
        playerManager = PlayerManager.instance;
        gameAudioManager = GameAudioManager.instance;
    }

    public void Complete()
    {
        SetPlayerManager();
        currentQuestWindow.SetActive(false);
        if (gameAudioManager != null)
        {
            gameAudioManager.OnQuestCompletedPlay();
        }
        isActive = false;
    }
}
