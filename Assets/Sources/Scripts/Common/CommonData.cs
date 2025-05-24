namespace Sources.Scripts.Common
{
    public enum EActionType
    {
        Stone = 0,
        Scissors = 1,
        Paper = 2,
    }

    public enum EBattleResult
    {
        Draw = 0,
        PlayerWins = 1,
        EnemyWins = 2,
    }
    
    public enum EUnitAnimation
    {
        Idle,
        Run,
        Attack,
        Aiming,
        Die,
    }

    public class CommonData
    {
        public static int playerHealth;
        
        public const string PLAYERPREFS_LEVEL_NUMBER = "LevelNumber";
        public static int levelNumber;
    }
}