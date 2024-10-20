namespace RehvidGames.Data.Serializable
{
    public enum AnimatorParameterType
    {
        Trigger,
        Bool,
        Float,
        Int
    }
    
    [System.Serializable]
    public class AnimationData
    {
        public string AnimationName;
        public AnimatorParameterType ParameterType;
        public object Value;
    }
}