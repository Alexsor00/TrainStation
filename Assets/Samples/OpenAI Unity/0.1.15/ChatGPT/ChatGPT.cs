using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI
{
    public class ChatGPTService 
{
    private OpenAIApi openai = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    public async Task<string> ProcessExternalText(string externalText, string prompt)
    {
        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = externalText
        };
        
        if (messages.Count == 0) newMessage.Content = prompt + "\n" + externalText; 
        messages.Add(newMessage);
        // Complete the instruction 
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo-0613",
            Temperature = 0.3f,
            MaxTokens = 15,
            Messages = messages,
        });
            
        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
        {
            var message = completionResponse.Choices[0].Message;
            return message.Content.Trim();
        }
        else
        {
            return "No response from ChatGPT.";
        }
    }
}
}
