﻿/******************************************************************************
 * Author: Michael Morris
 * Course: NEFMA
 * File: ConversationRunner.cs
 * 
 * Description: ConversationRunner actually runs a conversation, and tracks
 * the current position of the dialogue in the conversation (ie. node 8).
 * Voiceover progress needs to be tracked here as well.
 * 
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ConversationRunner : MonoBehaviour {
    public string convName;                  // Set a default conversation.
    public DialogueController dialogue;      // Handles speech bubble popups.

    protected Conversation conversation;     // Raw data for conversation.   
    protected float timeOfUpdate;            // Tracks expected time when the next dialogue should load.
    protected int currIndex;                 // Tracks which dialogue in the conversation we are on.
    protected bool conversationLoaded;       // True if a conversation has been successfully loaded from disk.

    float _skipTimer = 0.0f;

    // Use this for initialization
    void Start () {
        conversationLoaded = false;
        currIndex = 0;

        dialogue.initialize(convName);



        if (convName.Length > 0) {
            startConversation(convName);

            Globals.gamePaused = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!conversationLoaded) { return; }

        if (Time.time >= timeOfUpdate) {
            next();
        }

        // Skip Cutscene
        if (Input.GetButton("Jump_0") || Input.GetButton("Jump_1") || Input.GetButton("Jump_2") || Input.GetButton("Jump_3") || Input.GetButton("Jump_4"))
        {
            _skipTimer += Time.deltaTime;
        }
        else
        {
            _skipTimer = 0.0f;
        }

        if (_skipTimer > 2.0f)
        {
            end();
            // fade out
            Globals.gamePaused = false;
            Globals.fading = true;
            Globals.fadeDir = 1;
        }
    }

    public void startConversation(string conversationPath) {
        loadConversation(conversationPath);
        // Handle conversation load failure as gracefully as we can...
        if (conversationLoaded) {
            currIndex = 0;
            updateLine(0);
        }
    }

    // Move to next dialogue entry, or if moving past end of conversation, close dialogue
    public void next() {
        dialogue.stopVoiceOver();

        if (conversation.isEOF(currIndex)) {
            this.end();
            return;
        }

        currIndex = currIndex + 1;
        updateLine(currIndex);
    }

    // Reset tracking vars, empty out output strings.
    public void end() {
        if (!conversationLoaded) { return; }

        this.clear();

        conversationLoaded = false;
        currIndex = 0;
    }

    // Load a conversation from the provided file path.
    protected void loadConversation(string conversationPath)
    {
        // From Unity3D - Loading Game Data Json
        // Path.Combine combines strings into a file path.
        string filePath = Path.Combine(Application.streamingAssetsPath, conversationPath);
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            conversation = JsonUtility.FromJson<Conversation>(data);
            //Debug.Log(conversation.dialogue.Count);
            conversationLoaded = true;
            currIndex = 0;
        }
        else
        {
            Debug.Log(Application.streamingAssetsPath);
            Debug.LogError("ERROR: Failed to load conversation!");
            conversationLoaded = false;
        }

    }

    protected void playVOLine(int index) {
        dialogue.stopVoiceOver();

        // If a voice over line exists for this index {
        // TODO: Play this VO line now.

        string voFile = conversation.dialogue[index].voFile;
        if (voFile.Length > 0) {
            float duration = dialogue.playVoiceOver(voFile);

            // Override time of update to wait out VO, only if clip exists.
            if (duration > 0) {
                timeOfUpdate = Time.time + duration;
            }
        }
    }

    // Update display variables for the new line and start VO.
    protected void updateLine(int index) {
        if (conversation == null) { return; }

        if (conversationLoaded && !conversation.isEOF(index)) {
            timeOfUpdate = Time.time + conversation.getDisplayTime(index);
            playVOLine(index);

            dialogue.setNode(conversation.dialogue[index]);
        }
    }

    // Clears display variables to hide window.
    protected void clear() {
        dialogue.clearUI();
        timeOfUpdate = 0;
    }

}
