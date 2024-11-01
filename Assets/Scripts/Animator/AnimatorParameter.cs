namespace RehvidGames.Animator
{
    using UnityEngine;

    public static class AnimatorParameter
    {
        #region Movement
            public static readonly int XSpeed = Animator.StringToHash("XSpeed");
            public static readonly int YSpeed = Animator.StringToHash("YSpeed");
            public static readonly int Jump = Animator.StringToHash("Jump");
            public static readonly int JumpRun = Animator.StringToHash("JumpRun");
        #endregion

        #region Combat
            public static readonly int Attack = Animator.StringToHash("Attack");
            public static readonly int Dodge = Animator.StringToHash("Dodge");
            public static readonly int HitDirection = Animator.StringToHash("HitDirection");
            public static readonly int HitDirectionLeft = Animator.StringToHash("HitDirectionLeft");
            public static readonly int HitDirectionRight = Animator.StringToHash("HitDirectionRight");
            public static readonly int HitDirectionFront = Animator.StringToHash("HitDirectionFront");
            public static readonly int HitDirectionBack = Animator.StringToHash("HitDirectionBack");
        #endregion

        #region Interaction
            public static readonly int Interaction = Animator.StringToHash("Interaction");
            public static readonly int HideWeapon = Animator.StringToHash("HideWeapon");
            public static readonly int DrawWeapon = Animator.StringToHash("DrawWeapon");
            public static readonly int PickUp = Animator.StringToHash("PickUp");
            public static readonly int HasEquippedWeapon = Animator.StringToHash("HasEquippedWeapon");
        #endregion
        
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
                _ when hash == JumpRun => "JumpRun",
                _ when hash == DrawWeapon => "DrawWeapon",
                _ when hash == HasEquippedWeapon => "HasEquippedWeapon",
                _ when hash == PickUp => "PickUp",
                _ when hash == Dodge => "Dodge",
                _ when hash == Death => "Death",
                _ when hash == HitDirection => "HitDirection",
                _ when hash == HitDirectionLeft => "HitDirectionLeft",
                _ when hash == HitDirectionRight => "HitDirectionRight",
                _ when hash == HitDirectionFront => "HitDirectionFront",
                _ when hash == HitDirectionBack => "HitDirectionBack",
                _ => null
            };
        }
    }
}