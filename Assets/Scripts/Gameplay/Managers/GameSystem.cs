using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour, ISystem
{
    [SerializeField]
    private PlanningSequence planningSequence;

    public void Init()
    {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        planningSequence.Init();
    }

    public void Terminate()
    {
        if (gameObject.activeInHierarchy) gameObject.SetActive(false);
        planningSequence.Terminate();
    }
}
