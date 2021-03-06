﻿/******************************************************************************
 * Author: Michael Morris
 * Course: NEFMA
 * File: Conversation.cs
 * 
 * Description: Handles storage and processing of dialogue nodes.  Handling is
 * not provided for seperate responses per node (it is not needed).  Will need
 * additional handling for voice over.
 * 
 * Credits: 
 * Rough structure based on the one suggested by Tony Li here:
 * https://forum.unity3d.com/threads/dialogue-system-tutorial.315618/
 * 
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation
{
    public List<Node> dialogue;         // TODO: Needs to become private.
    
    // Move to next dialogue entry, or if moving past end of conversation, close dialogue
    int nextIndex(int oldIndex)
    {
        if (isEOF(oldIndex)) {
            return -1;
        }
        
        return (oldIndex + 1);
    }

    public string getSpeaker(int index) {
        return dialogue[index].Speaker;
    }

    public string getText(int index) {
        return dialogue[index].Text;
    }

    public string getVOFile(int index) {
        return dialogue[index].voFile;
    }

    public int getDisplayTime(int index)
    {
        return dialogue[index].displayTime;
    }

    public string getType(int index)
    {
        return dialogue[index].type;
    }

    public int getSpecial(int index)
    {
        return dialogue[index].special;
    }

    public bool isEOF(int index) {
        if (index >= dialogue.Count) {
            return true;
        }

        return false;
    }
}


[System.Serializable]
public class Node
{
    public string Speaker;
    public string Text;
    public string voFile;
    public int displayTime;
    public string type = "Ellipsis";
    public int special = 0;
}