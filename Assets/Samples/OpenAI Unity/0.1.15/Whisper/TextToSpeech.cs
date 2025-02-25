﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeech
{


    // Start is called before the first frame update
   public async void ConvertTextToSpeech(string textToConvert, AudioSource audioSource, bool isMale)
    {
        var credentials = new BasicAWSCredentials("AKIAZHRI4TMTGGZAGYO6", "XcM7S2z6oawJi/7LrwzDhdAg+fPFQzL7U9DEl8Sf");
        var client = new AmazonPollyClient(credentials, RegionEndpoint.EUWest3);
      VoiceId voiceSelection = isMale ? VoiceId.Sergio : VoiceId.Lucia; // Esto selecciona Juan si isMale es true, y Lucia si es false

        var request = new SynthesizeSpeechRequest(){
         Text = textToConvert,
         LanguageCode = LanguageCode.EsES,
         Engine = Engine.Neural,
         VoiceId = voiceSelection,
            OutputFormat = OutputFormat.Ogg_vorbis // Cambiamos a OGG
        };

        var response = await client.SynthesizeSpeechAsync(request);
         WriteIntoFile(response.AudioStream);

         using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.OGGVORBIS)){
               var op = www.SendWebRequest();

               while(!op.isDone) await Task.Yield();

               var clip = DownloadHandlerAudioClip.GetContent(www);
               audioSource.clip = clip;
               audioSource.Play();
         }
    }

  private void WriteIntoFile(Stream stream){
    using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create)){
      byte[] buffer = new byte[8 * 1024];
      int bytesRead;
      while((bytesRead = stream.Read(buffer, 0 , buffer.Length)) > 0){
         fileStream.Write(buffer, 0, bytesRead);
      }
    }
  }

}
