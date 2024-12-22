namespace RehvidGames.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SaveMenuTitles", menuName = "ScriptableObjects/SaveMenuTitles", order = 1)]
    public class SaveMenuTitle : ScriptableObject
    {
        [TextArea] public string OverrideSlotTitle;
        [TextArea] public string ClearSlotTitle;
        [TextArea] public string HeaderSlotTitle;
        [TextArea] public string HeaderLoadingSlotTitle;
    }
}