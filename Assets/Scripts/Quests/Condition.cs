using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "HamsterGame/Quest/Condition", fileName = "new Condition")]
public class Condition : ScriptableObject
{
    public bool pickUpAllGrains;
    public bool dropAllGrains;
    public bool needSpecificAmountOfGrains;
    [ConditionalHide("needSpecificAmountOfGrains")] public int specificAmountOfGrains;
    public bool hasLimitedEndurance;
    [ConditionalHide("hasLimitedEndurance")] public int maxEndurancePoints;
    [ConditionalHide("hasLimitedEndurance")] public bool displayEndurance;
    public bool findExit;
    public bool needToFindExit;
    [ConditionalHide("needToFindExit")] public Transform exitTransform;

    public UnityEvent onAddGrain;
    public UnityEvent onRemoveGrain;
    public UnityEvent onMove;

    public void OnAddGrain()
    {
        onAddGrain?.Invoke();
    }

    public void OnRemoveGrain()
    {
        onRemoveGrain?.Invoke();
    }

    public void OnMove()
    {
        onMove?.Invoke();
    }
}
