using UnityEngine;

public class GoalManager : MonoBehaviour {

    private int currentGoalIndex;

    public Vector2 NextGoal {
        get => transform.GetChild(currentGoalIndex).position;
    }

    private void Start() {
        currentGoalIndex = transform.childCount - 1;
        Next();
    }

    private void Next() {
        currentGoalIndex++;
        if(currentGoalIndex == transform.childCount)
            currentGoalIndex = 0;
        transform.GetChild(currentGoalIndex).GetComponent<Goal>().GoalReached += (o, e) => Next();
        Debug.Log(currentGoalIndex);
    }

}
