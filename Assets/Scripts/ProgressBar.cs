using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject fill;
    public GameObject goal;
    public GameObject activateWhenGoalHit;
    public bool goalActive = true;
    public float goalPadding = 0.3f;
    public float maxFillScale = 7;

    private float goalInitY;

    // 0 to 1
    private float currentValue;
    private float currentGoal;

    // Start is called before the first frame update
    void Start()
    {
        goal.SetActive(goalActive);
        goalInitY = goal.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ls = fill.transform.localScale;
        ls.x = currentValue * maxFillScale;
        fill.transform.localScale = ls;

        if (goalActive)
        {
            float goalPosRange = maxFillScale - (goalPadding * 2);
            float goalPosOffset = goalPadding;
            Vector3 goalPos = goal.transform.localPosition;
            goalPos.y = (currentGoal * goalPosRange) + goalPosOffset + goalInitY;
            goal.transform.localPosition = goalPos;
        }
    }

    public void SetCurrentValue(float value)
    {
        currentValue = value;
    }

    public void SetCurrentGoal(float value)
    {
        currentGoal = value;
    }

    public void TriggerGoalAnimation()
    {
        ParticleSystem ps = goal.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        activateWhenGoalHit.SetActive(true);
    }
}
