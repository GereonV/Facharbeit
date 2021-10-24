using System;
using UnityEngine;

public class Goal : MonoBehaviour {

    public event EventHandler GoalReached;

    private void OnTriggerEnter2D() {
        GoalReached?.Invoke(this, EventArgs.Empty);
        GoalReached = null;
    }

}
