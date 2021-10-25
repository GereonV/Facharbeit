using System;
using UnityEngine;

public class Goal : MonoBehaviour {

    private GoalManager gm;

    private void OnTriggerEnter2D() {
        gm?.Next();
        gm = null;
    }

    public void SetGM(GoalManager gm) {
        this.gm = gm;
    }

}
