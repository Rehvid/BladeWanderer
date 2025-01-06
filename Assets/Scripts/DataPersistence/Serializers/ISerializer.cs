namespace RehvidGames.DataPersistence.Serializers
{
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the given object to the specified file path.
        /// </summary>
        void Serialize<T>(string filePath, T data) where T : ISavableData;

        /// <summary>
        /// Deserializes data from the specified file path to the given type.
        /// </summary>
        T Deserialize<T>(string filePath) where T : ISavableData;
    }
}