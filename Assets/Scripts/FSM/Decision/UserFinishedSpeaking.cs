using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UserFinishedSpeaking", menuName = "FSM/Decision/User Finished Speaking")]

public class UserFinishedSpeaking : FSMdecision
{
    public override bool Decide(FSMcontroller controller)
    { 
        return  MicrophoneManager.instance.IsFinishedSpeaking();
    }
}
