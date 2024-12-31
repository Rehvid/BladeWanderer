namespace RehvidGames.Interfaces
{
    using Characters.Player;
    
    public interface IInteractable
    {
        public void Interact(PlayerController player);
        public bool CanInteract(PlayerController player);
    }
}