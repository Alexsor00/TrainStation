using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

[CreateAssetMenu(fileName = "New UserInteracting", menuName = "FSM/Decision/User Interacting")]

public class UserInteracting : FSMdecision
{
    public override bool Decide(FSMcontroller controller)
    {
   bool isSpeaking = MicrophoneManager.instance.IsSpeaking();

        return isSpeaking && AvatarDetector.instance.DetectAvatar() == controller.gameObject;
    }
}
