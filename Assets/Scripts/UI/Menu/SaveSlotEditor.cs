#if UNITY_EDITOR

namespace RehvidGames.UI.Menu
{
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(SaveSlot))]
    public class SaveSlotEditor : Editor
    {
         public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SaveSlot saveSlot = (SaveSlot)target;

            if (GUILayout.Button("Generate New GUID"))
            {
                saveSlot.GenerateGuid();
                EditorUtility.SetDirty(saveSlot); 
            }
        }
    }
}
#endif