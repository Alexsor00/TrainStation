using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public struct PromptOption
{
    public string displayName;  // El nombre a mostrar en el Dropdown
    public string prompt;  // El prompt asociado
}

[CreateAssetMenu(fileName = "AvailablePrompts", menuName = "Prompts/Available Prompts", order = 1)]
public class AvailablePrompts : ScriptableObject
{
    public List<PromptOption> promptOptions = new List<PromptOption>();  // Lista de opciones de prompts
    public int selectedPromptIndex = 0;  // El índice del prompt seleccionado

    public string GetCurrentPrompt()
    {
        if (promptOptions.Count > selectedPromptIndex)
        {
            return promptOptions[selectedPromptIndex].prompt;
        }
        return "";
    }
}
