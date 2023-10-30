using UnityEditor;
using UnityEngine;
using OpenAI;

[CustomEditor(typeof(Whisper))]
public class WhisperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Whisper whisper = (Whisper)target;

        // Mostrar las propiedades predeterminadas
        DrawDefaultInspector();

        // Mostrar el desplegable para los prompts
        string[] displayNames = new string[whisper.prompts.Keys.Count];
        whisper.prompts.Keys.CopyTo(displayNames, 0);

        int currentIndex = System.Array.IndexOf(displayNames, whisper.selectedPromptDisplayName);
        if (currentIndex == -1) currentIndex = 0;

        int newIndex = EditorGUILayout.Popup("Prompt", currentIndex, displayNames);

        if (currentIndex != newIndex)
        {
            whisper.selectedPromptDisplayName = displayNames[newIndex];
            EditorUtility.SetDirty(whisper);  // Marcar el objeto para guardarse
        }

        EditorGUILayout.Space();
    }
}
