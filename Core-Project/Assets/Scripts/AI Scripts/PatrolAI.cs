using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : BaseAI
{
    public Transform[] points;

    void Start ()
    {
        PatrolAction action = new PatrolAction(agent, points);

        SetCurrentAction(action);
    }
}
