using System;
using UnityEngine;
using UnityEngine.UI;

namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private string prompt;
        public bool isMale = true;
        private readonly string fileName = "output.wav";
        private readonly ChatGPTService chatGPTService = new ChatGPTService();
        private readonly TextToSpeech textToSpeech = new TextToSpeech();  // Referencia al componente TextToSpeech.
        public bool isRecording;
        private OpenAIApi openai = new OpenAIApi();
   



  private bool wasPlayingLastFrame = false;
        public bool hasAvatarFinishedSpeaking = false;


        public bool AvatarFinishedSpeaking()
        {
            if(wasPlayingLastFrame && !audioSource.isPlaying)
            {
                wasPlayingLastFrame = false;
                hasAvatarFinishedSpeaking = true;
               MicrophoneManager.instance.StartRecording();
                return true; // Avatar terminó de hablar

            }

            wasPlayingLastFrame = audioSource.isPlaying;
            hasAvatarFinishedSpeaking = false;
            return false;
        }

     public async void ProcessRecording()
{
    try
    {
        // Termina la grabación del micrófono
               MicrophoneManager.instance.StopRecording();


        // Convertir el audio clip a formato WAV
        byte[] data = SaveWav.Save(fileName, MicrophoneManager.instance.GetCurrentAudioClip());

        // Crear solicitud de transcripción
        var req = new CreateAudioTranscriptionsRequest
        {
            FileData = new FileData() { Data = data, Name = "audio.wav" },
            Model = "whisper-1",
            Language = "es"
        };

        // Solicitar transcripción al servicio OpenAI
        var res = await openai.CreateAudioTranscription(req);
        Debug.Log(res.Text);

        // Obtener respuesta del modelo de chat usando el texto transcribido
        string chatGPTResponse = await chatGPTService.ProcessExternalText(res.Text, prompt);
       // Debug.Log(chatGPTResponse);

        // Convertir respuesta a voz y reproducirla
        textToSpeech.ConvertTextToSpeech(chatGPTResponse, audioSource, isMale);

        // Reanudar la grabación (si es necesario continuar la conversación)

    }
    catch (Exception e)
    {
        // Log any exception that might occur
        Debug.LogError("Error while processing the recording: " + e.Message);
    }
}






    }
}
