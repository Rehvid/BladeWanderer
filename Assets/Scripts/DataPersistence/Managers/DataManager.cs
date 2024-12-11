namespace RehvidGames.DataPersistence.Managers
{
    public class DataManager <T> where T: class, new()
    {
        public T Data { get; set; }
        
        public void ResetData() => Data = new T();
        
        public bool HasData() => Data != null;
    }
}