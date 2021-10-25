using UnityEngine;

public class GoalManager : MonoBehaviour {

    private int currentGoalIndex;

    private int lap;

    public Vector2 NextGoal {
        get => transform.GetChild(currentGoalIndex).position;
    }

    private void Start() {
        currentGoalIndex = -1;
        lap = 0;
        Next();
    }

    public void Next() {
        currentGoalIndex++;
        if(currentGoalIndex == transform.childCount) {
            currentGoalIndex = 0;
            lap++;
        }
        transform.GetChild(currentGoalIndex).GetComponent<Goal>().SetGM(this);
        Debug.Log(currentGoalIndex);
    }

    public float GetScore(Transform t) {
        return (lap * transform.childCount + currentGoalIndex) * 100f + Mathf.Min(1f / Vector2.Distance(t.position, NextGoal), 80f);
    }

    public void Reset() {
        Start();
    }

}
