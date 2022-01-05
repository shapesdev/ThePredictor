using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlanningSequence planningSequence;

    private void Awake()
    {
        planningSequence.Start();
    }
}
