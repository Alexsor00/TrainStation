using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "AvatarPrompt", menuName = "Prompts/Avatar Prompt", order = 2)]
public class AvatarPrompt : ScriptableObject
{
    public AvailablePrompts allPrompts;  // Referencia al ScriptableObject que contiene todos los prompts disponibles
    public int selectedPromptIndex;  // El índice del prompt seleccionado del desplegable

    // Obtener el prompt seleccionado
    public string GetSelectedPrompt()
    {
        if (allPrompts && allPrompts.promptOptions.Count > selectedPromptIndex)
        {
            return allPrompts.promptOptions[selectedPromptIndex].prompt;
        }
        return "";  // Devuelve una cadena vacía si no hay un prompt válido seleccionado
    }
}
