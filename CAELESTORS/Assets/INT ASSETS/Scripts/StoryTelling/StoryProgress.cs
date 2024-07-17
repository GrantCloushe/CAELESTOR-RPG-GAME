using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgress : MonoBehaviour
{
    public static int storyChapter = 0;
    public static int chapterEvent = 5;

    public int StoryChapter()
    {
        return storyChapter;
    }

    public int ChapterEvent()
    {
        return chapterEvent;
    }

    public void SetChapter(int _chapter, int _event = 0)
    {
        storyChapter = _chapter;
        chapterEvent = _event;
    }

}
