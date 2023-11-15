using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI
{
    public class ChatGPTService 
    {
        private OpenAIApi openai = new OpenAIApi();
        private string conversationState;

        public ChatGPTService(string initialPrompt)
        {
            this.conversationState = initialPrompt;
        }

        public async Task<string> ProcessExternalText(string externalText)
        {
            var messages = new List<ChatMessage>
            {
                new ChatMessage()
                {
                    Role = "system",
                    Content = conversationState
                },
                new ChatMessage()
                {
                    Role = "user",
                    Content = externalText
                }
            };
        Debug.Log("Generando el texto");
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-4",
                Temperature = 0.3f,
                MaxTokens = 15,
                Messages = messages,
            });
                  Debug.Log("Texto generado");
      
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var response = completionResponse.Choices[0].Message.Content.Trim();
                // Actualiza el estado de la conversación
                conversationState += "\n" + externalText + "\n" + response;
                return response;
            }
            else
            {
                return "No response from ChatGPT.";
            }
        }
    }
}
