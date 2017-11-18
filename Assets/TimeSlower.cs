using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TimeSlower : MonoBehaviour
{

    private class TimeSlowerState
    {
        public float timeScale;
        public float lerpRate;
        public float target;
    }

    private static TimeSlowerState state = new TimeSlowerState();

    public static float TimeScale
    {
        get
        {
            return state.timeScale;
        }
    }

    public void Start()
    {
        state.timeScale = .01f;
        state.target = 1f;
        state.lerpRate = .01f;
    }

    public void FixedUpdate()
    {
        state.timeScale = (state.timeScale * (1f - state.lerpRate)) + (state.target * state.lerpRate);
    }

    public static void StartInput()
    {
        state.timeScale = .15f;
        state.target = 1f;
        state.lerpRate = .007f;
    }
    public static void EndInput()
    {
        state.target = 1f;
        state.lerpRate = .05f;
    }
}
