namespace RehvidGames.Interfaces
{
    using Player;

    public interface IInteractable
    {
        public void Interact(Player player);
        public bool CanInteract(Player player);
    }
}