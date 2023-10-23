using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
public class MicrophoneManager : MonoBehaviour
{
    private enum SpeakingState
{
    NotSpeaking,
    Speaking,
    FinishedSpeaking,
    Idle
}
    public static MicrophoneManager instance;
private float silenceThreshold = 0.01f; // Umbral de silencio
    private AudioClip clip;
    public bool isRecording = false;
private SpeakingState currentState = SpeakingState.NotSpeaking;

private const float SPEAKING_DELAY = 1f;
private float speakingDelayTimer = 0f;

    void Awake()
    {
        instance = this;
        StartRecording(); // Comienza la grabación al iniciar el juego.
    }

    public void StartRecording()
    {
        if (isRecording) 
        {
            Debug.Log("Is already recording");
            return;
        }
    Debug.Log("Start Recording");

        clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        isRecording = true;
    }

    public AudioClip GetCurrentAudioClip()
    {
        return clip;
    }




public bool IsUserCurrentlySpeaking()
{
    return GetAverageVolume() > silenceThreshold;
}

public bool CheckUserSpeakingStatus()
{
    bool currentlySpeaking = IsUserCurrentlySpeaking();
    switch (currentState)
    {
        case SpeakingState.NotSpeaking:
            if (currentlySpeaking)
            {
                currentState = SpeakingState.Speaking;
                speakingDelayTimer = SPEAKING_DELAY;
            }
            break;

        case SpeakingState.Speaking:
            if (!currentlySpeaking)
            {
                speakingDelayTimer -= Time.deltaTime;

                if (speakingDelayTimer <= 0)
                {
                    currentState = SpeakingState.FinishedSpeaking;
                }
            }
            else
            {
                speakingDelayTimer = SPEAKING_DELAY; // Reset the timer if the user starts speaking again
            }
            break;

        case SpeakingState.FinishedSpeaking:
            if (currentlySpeaking)
            {
                currentState = SpeakingState.Speaking;
                speakingDelayTimer = SPEAKING_DELAY;
            }
            break;
    }

    return currentState == SpeakingState.FinishedSpeaking;
}



public float GetAverageVolume()
{
    float[] data = new float[256];
    int offset = Microphone.GetPosition(null) - 256; // near the end of the buffer
    if (offset < 0){
 return 0;
    }
    clip.GetData(data, offset);
    float a = 0;
    foreach (float s in data)
    {
        a += Mathf.Abs(s);
    }
    return a / 256;
}

public void StopRecording()
{
    if (!isRecording) // Si no estamos grabando, no hacemos nada
    {
        return;
    }

    #if !UNITY_WEBGL
    Debug.Log("Stop Recording");
    Microphone.End(null); // Detenemos la grabación del micrófono
    isRecording = false; // Actualizamos el flag
    #endif
}
public bool IsSpeaking()
{
    return currentState == SpeakingState.Speaking;
}
public bool IsFinishedSpeaking()
{
    if (currentState == SpeakingState.FinishedSpeaking)
    {
        currentState = SpeakingState.NotSpeaking; // Reset state after checking
        return true;
    }
    return false;
}

  void Update()
    {
        CheckUserSpeakingStatus();
    }
}
