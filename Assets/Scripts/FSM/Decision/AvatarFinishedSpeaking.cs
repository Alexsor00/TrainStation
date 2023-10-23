using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

[CreateAssetMenu(fileName = "New AvatarFinishedSpeaking", menuName = "FSM/Decision/Avatar Finished Speaking")]

public class AvatarFinishedSpeaking : FSMdecision
{

     public override bool Decide(FSMcontroller controller)
    { 
        Whisper whisper = controller.GetComponent<Whisper>();
        bool avatarFinished =  whisper.AvatarFinishedSpeaking();
        if(whisper != null && avatarFinished) return true;
        return false;
    }
}
