using UnityEngine;

public class GameData
{
    private static GameData instance;

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }
            return instance;
        }
    }

    private GameData()
    {
        if (instance != null)
            return;
    }

    public bool IsPaused { get; set; }
    public float SpeedConstant = 1;
    public int Penalty { get; set; }
}
