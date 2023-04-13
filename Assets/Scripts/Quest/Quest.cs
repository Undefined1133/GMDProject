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

    private void SetPlayerManager()
    {
        playerManager = PlayerManager.instance;
        gameAudioManager = GameAudioManager.instance;
    }

    public void Complete()
    {
        SetPlayerManager();
        if (gameAudioManager != null)
        {
            Debug.Log("Should play the quest complete sound");
            gameAudioManager.OnQuestCompletedPlay();
        }
        isActive = false;
        Debug.Log("Quest was completed :D");
    }
}
