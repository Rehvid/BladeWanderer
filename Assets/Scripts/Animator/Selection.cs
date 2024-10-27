namespace RehvidGames
{
    using UnityEngine;
    public class Selection : StateMachineBehaviour
    {
        [SerializeField] private int _count;
        [SerializeField] private string parameterName;
        
        public override void OnStateEnter(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
