namespace RehvidGames.DataPersistence.Serializers
{
    using System;
    using System.IO;
    using RehvidGames.Interfaces;
    using UnityEngine;

    public class FileSerializer: ISerializer
    {
        private const string EncryptionCodeWord = "crocodile";
        private readonly bool _useEncryption;

        public FileSerializer(bool useEncryption)
        {
            _useEncryption = useEncryption;
        }
        
        
        public void Serialize<T>(string filePath, T objectToSerialize) where T : ISavableData
        {
            try
            {
                var serializedObject = JsonUtility.ToJson(objectToSerialize, true);

                if (_useEncryption)
                {
                    serializedObject = EncryptDecrypt(serializedObject);
                }
                    
                File.WriteAllText(filePath, serializedObject);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data  at {filePath}. Exception: {e}");
                throw;
            }
        }

        public T Deserialize<T>(string filePath) where T : ISavableData
        {
            try
            {
                var data = File.ReadAllText(filePath);

                if (_useEncryption)
                {
                    data = EncryptDecrypt(data);
                }

                return JsonUtility.FromJson<T>(data);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Error occured when trying to load data from file: " + filePath + "\n" + ex);
                throw;
            }
        }
        
        
        
        /// <summary>
        /// Encrypts or decrypts a given string by applying an XOR operation 
        /// with a repeating encryption key. This method works symmetrically,
        /// meaning the same function can be used for both encryption and decryption.
        /// </summary>
        private string EncryptDecrypt(string dataToEncrypt)
        {
            var modifiedData = "";
            for (int i = 0; i < dataToEncrypt.Length; i++)
            {
                modifiedData += (char) (dataToEncrypt[i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}