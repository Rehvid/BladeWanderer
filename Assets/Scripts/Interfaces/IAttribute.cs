namespace RehvidGames.Interfaces
{
    public interface IAttribute
    {
        public float CurrentValue { get; set; }
        public float MaxValue { get;}

        public float GetInterpolatedOrRawValue();
    }
}