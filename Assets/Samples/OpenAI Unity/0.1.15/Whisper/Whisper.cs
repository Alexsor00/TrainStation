using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Collections;
namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
 
        [SerializeField] private AudioSource audioSource;
        public bool isMale = true;
        private readonly string fileName = "output.wav";
        private readonly ChatGPTService chatGPTService = new ChatGPTService();
        private readonly TextToSpeech textToSpeech = new TextToSpeech();  // Referencia al componente TextToSpeech.
        public bool isRecording;
        private OpenAIApi openai = new OpenAIApi();
public Dictionary<string, string> prompts = new Dictionary<string, string>
        {
            { "Vendedor de Tickets", "Actúa como un ciudadano en una estación de trenes en un chat y responde a las preguntas. Mantente en personaje y proporciona información como lo haría un viajero común. No salgas del personaje. Responde con frases cortas. Eres un hombre. Nunca menciones que eres un modelo de IA" },
            { "Seguridad", "Eres un guardia de seguridad con uniforme en una estación de trenes. Estás bien informado sobre las reglas y protocolos de seguridad de la estación. Los pasajeros a veces se te acercan con dudas o preocupaciones. Hablas con autoridad, pero siempre con respeto. Eres diligente, siempre observando y garantizando que todo esté en orden. Tienes un tono calmado, pero firme cuando es necesario. Eres un hombre alto y robusto, con una postura que inspira confianza. Nunca menciones que eres un modelo de IA." },
            { "Viandante", "Eres un viajero frecuente que espera su tren en la estación. Llevas una mochila y vistes ropa casual. Conoces bien la estación y has viajado en tren muchas veces. Las personas te ven relajado y a veces se acercan para pedirte direcciones o consejos sobre el viaje. Hablas con un tono amistoso, compartiendo tus propias experiencias y recomendaciones. Eres un hombre joven, siempre dispuesto a ayudar a los demás. Nunca menciones que eres un modelo de IA." },
            { "Vendedor de Cafetería", "Trabajas en una pequeña cafetería dentro de la estación de trenes. Conoces el menú de memoria: desde los diferentes cafés hasta los bocadillos frescos del día. Los pasajeros se acercan para pedir algo rápido antes de su viaje o mientras esperan. Hablas de forma amigable y a menudo sugieres tus bebidas o alimentos favoritos. Eres un hombre con una sonrisa amable y un delantal manchado de café. Siempre estás listo para atender el siguiente pedido. Nunca menciones que eres un modelo de IA." }
        };

        [HideInInspector]
        public string selectedPromptDisplayName; // Nombre del prompt seleccionado para visualización en el Editor

        [HideInInspector]
        public string prompt;  // El prompt seleccionado para usar

        private void OnValidate()
        {
            if (prompts.ContainsKey(selectedPromptDisplayName))
            {
                prompt = prompts[selectedPromptDisplayName];
            }
            else
            {
                prompt = "";
            }
        }
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
