namespace RehvidGames.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewSaveMenuTitles", menuName = "UI/SaveMenuTitles", order = 1)]
    public class SaveMenuTitleData : ScriptableObject
    {
        [SerializeField, TextArea] private string _overrideSlotTitle;
        [SerializeField, TextArea] private string _clearSlotTitle;
        [SerializeField, TextArea] private string _headerSlotTitle;
        [SerializeField, TextArea] private string _headerLoadingSlotTitle;

        public string OverrideSlotTitle => _overrideSlotTitle;
        public string ClearSlotTitle => _clearSlotTitle;
        public string HeaderSlotTitle => _headerSlotTitle;
        public string HeaderLoadingSlotTitle => _headerLoadingSlotTitle;
    }
}