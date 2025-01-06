namespace RehvidGames.DataPersistence.Data.State
{
    using Serializers;

    [System.Serializable]
    public class GameState: ISavableData
    {
        public PlayerAttributeState PlayerAttributeState = new();
        public PlayerState PlayerState = new();
        public InventoryState InventoryState = new();
        public GameSessionState SessionState = new();
    }
}