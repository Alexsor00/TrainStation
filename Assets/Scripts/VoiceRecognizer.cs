using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognizer : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer;

	public static VoiceRecognizer instance;
	public bool startedAnalysis;
	float timer;
	// Use this for initialization
	void Start()
	{
		timer = 15;
		if (VoiceRecognizer.instance) Destroy(this);
		VoiceRecognizer.instance = this;

		dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.AutoSilenceTimeoutSeconds = 50;
		dictationRecognizer.InitialSilenceTimeoutSeconds = 50;
		dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
		dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
		
		On();
	}
    private void Update()
    {
		if (dictationRecognizer.Status.ToString() == "Stopped")
		{
			timer -= Time.deltaTime;
            if (timer <= 0)
            {
				On();
				timer = 15;
            }
		}
		else timer = 15;
    }
    public void On()
    {
		Debug.Log("Recongnize on");
		print("Recognizer on");
		dictationRecognizer.Start();
    }

	public void Off()
    {
		dictationRecognizer.Stop();
    }

	private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
	{
		Debug.Log("DictationRecognizer_DictationResult");
		dictationRecognizer.Start();

		startedAnalysis = true;
		SentenceAnalyzer.instance.Analyze(text);
        
		print(text + " " + confidence);
	}

    
	private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
	{
		dictationRecognizer.Stop();
		print("Recognizer stopped");
		print(cause);
		dictationRecognizer.Start();
	}


	void OnDestroy()
	{
		dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
		dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
		dictationRecognizer.Dispose();
	}
}
