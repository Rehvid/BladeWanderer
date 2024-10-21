namespace RehvidGames.Collectable
{
    using System;
    using Player;
    using UnityEngine;
    
    public class Soul: Collectable
    {
        [SerializeField] private int _amount = 5;
        [SerializeField] private float _lifetime = 10f;
        
        public override void Collect(Player player)
        {
            player.Attributes.AddSouls(_amount);
            Destroy(gameObject);
        }

        private void Update()
        {
            if (Time.time >= _lifetime)
            {
                Debug.LogWarning("Soul - Remove"); //TODO: Jeśli minęło połowe czasu to zmniejsz size
            }   
        }
    }
}