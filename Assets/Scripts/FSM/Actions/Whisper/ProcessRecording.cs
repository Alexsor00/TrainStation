using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

[CreateAssetMenu(fileName = "New ProcessRecording", menuName = "FSM/Action/Process Recording")]

public class ProcessRecording : FSMaction
{


   public override void Act(FSMcontroller controller)
    {
        // Obtener el componente Whisper
        Whisper whisper = controller.GetComponent<Whisper>();
        // Verificar si el avatar tiene el componente Whisper
        if (whisper != null)
        {
           whisper.ProcessRecording();
        }
    }
}
