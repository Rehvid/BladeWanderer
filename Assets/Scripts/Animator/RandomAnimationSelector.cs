namespace RehvidGames.Animator
{
    using UnityEngine;

    public class RandomAnimationSelector : StateMachineBehaviour
    {
        [SerializeField] private int _count;
        [SerializeField] private string parameterName;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            int selection = Random.Range(0, _count);
            int currentSelection = animator.GetInteger(parameterName);

            if (_count > 1)
            {
                while (selection == currentSelection)
                {
                    selection = Random.Range(0, _count);
                }
            }
            animator.SetInteger(parameterName, selection);
        }
        
    }
}
