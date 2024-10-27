namespace RehvidGames.Animator
{
    using UnityEngine;

    public static class AnimatorParameter
    {
        public static readonly int XSpeed = Animator.StringToHash("XSpeed");
        public static readonly int YSpeed = Animator.StringToHash("YSpeed");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Interaction = Animator.StringToHash("Interaction");
        public static readonly int HideWeapon = Animator.StringToHash("HideWeapon");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int DrawWeapon = Animator.StringToHash("DrawWeapon");
        public static readonly int PickUp = Animator.StringToHash("PickUp");
        public static readonly int HasEquippedWeapon = Animator.StringToHash("HasEquippedWeapon");
        public static readonly int Dodge = Animator.StringToHash("Dodge");
        public static readonly int Death = Animator.StringToHash("Death");
        
        public static string GetParameterName(int hash)
        {
            return hash switch
            {
                _ when hash == XSpeed => "XSpeed",
                _ when hash == YSpeed => "YSpeed",
                _ when hash == Attack => "Attack",
                _ when hash == Interaction => "Interaction",
                _ when hash == HideWeapon => "HideWeapon",
                _ when hash == Jump => "Jump",
                _ when hash == DrawWeapon => "DrawWeapon",
                _ when hash == HasEquippedWeapon => "HasEquippedWeapon",
                _ when hash == PickUp => "PickUp",
                _ when hash == Dodge => "Dodge",
                _ when hash == Death => "Death",
                _ => null
            };
        }
    }
}