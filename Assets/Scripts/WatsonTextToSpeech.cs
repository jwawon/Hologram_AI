/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;

public class WatsonTextToSpeech : MonoBehaviour
{

    private string _username = "6f7a3ceb-3e2e-4fac-914d-0b9a87fe57d4";
    private string _password = "qVnImcuHjYWd";
    private string _url = "https://stream.watsonplatform.net/text-to-speech/api";

    TextToSpeech _textToSpeech;

    string _createdCustomizationId;

    private bool _synthesizeTested = false;
    private bool _getVoicesTested = false;
    private bool _getVoiceTested = false;
    private bool _getPronuciationTested = false;
    private bool _getCustomizationsTested = false;
    private bool _createCustomizationTested = false;
    private bool _deleteCustomizationTested = false;
    private bool _getCustomizationTested = false;
    private bool _updateCustomizationTested = false;
    private bool _getCustomizationWordsTested = false;
    private bool _addCustomizationWordsTested = false;
    private bool _deleteCustomizationWordTested = false;
    private bool _getCustomizationWordTested = false;

	public ExampleStreaming SpeechToTextManager;
    private bool isWatsonPlay=false;
    private AudioSource source;
    GameObject audioObject;
    private void OnEnable() {
        LogSystem.InstallDefaultReactors();

        //  Create credential and instantiate service
        Credentials credentials = new Credentials(_username, _password, _url);

        _textToSpeech = new TextToSpeech(credentials);
        audioObject = new GameObject("AudioObject");
        source = audioObject.AddComponent<AudioSource>();
        if(RestfulMainScript.isNlpComplete){
            Debug.Log("TTS enabled: " +RestfulMainScript.nlpAnswer);    
            RunnableTTS.Run(Examples(RestfulMainScript.nlpAnswer));
            RestfulMainScript.isNlpComplete = false;
        }
    }
    private void Update() {
        if(isWatsonPlay){
            if(!source.isPlaying){
                Debug.Log("audio is stopped");
                isWatsonPlay=false;
                _synthesizeTested = false;
                SpeechToTextManager.StartRecording();
                Destroy(audioObject);
                this.gameObject.SetActive(false);
            }
        }
        
    }

    private IEnumerator Examples(string answerString)
    {
        //  Synthesize
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting synthesize.");
        _textToSpeech.Voice = VoiceType.en_US_Allison;
        _textToSpeech.ToSpeech(HandleToSpeechCallback, OnFail, answerString, true);
        while (!_synthesizeTested)
            yield return null;
        Log.Debug("ExampleTextToSpeech.Examples()", "Text to Speech examples complete.");
    }

    void HandleToSpeechCallback(AudioClip clip, Dictionary<string, object> customData = null)
    {
        PlayClip(clip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            if(source.isPlaying){
                isWatsonPlay = true;
            }
            // Destroy(audioObject, clip.length);

            _synthesizeTested = true;
        }
    }

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
    }
}
