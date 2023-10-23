using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AwayFromPerson", menuName = "FSM/Decision/Away From Person")]

public class AwayFromPerson : FSMdecision
{
	public float distance;
	public bool insideDistance;

	public override bool Decide(FSMcontroller controller)
	{
		if(AvatarDetector.instance.DetectAvatar() != controller.gameObject){
		 MicrophoneManager.instance.StartRecording();
         return true;
		}
		return false;

	}
}
