﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOne : MonoBehaviour
{
    public Button button;
    public Gamemanager gamemanager;
    public Npcmanager npcmanager;

    public void EventHonme()
    {
        StartCoroutine(HomeCoroutine());
    }

    IEnumerator HomeCoroutine()
    {
        yield return new WaitUntil(() => button.cuthome);
        Debug.Log("asf");
        gamemanager.Talk(500, false);

        yield return new WaitForSeconds(3f);
    }
}