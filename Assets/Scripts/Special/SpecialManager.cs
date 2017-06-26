using System.Collections.Generic;
using UnityEngine;

/*
 * The SpecialManager is an ability that needs to be charged to operate some special action.
 */
[CreateAssetMenu(fileName = "SpecialManager", menuName = "ScriptableObjects/Specials/SpecialManager")]
public class SpecialManager : ScriptableObject
{
    private static readonly float MAX_VALUE = 100;

    public Player player;
    public List<SpecialUsedListener> specialUsedListeners;
    public List<SpecialFullListener> specialFullListeners;
    public SpecialAction SpecialAction
    {
        private get {return _specialAction; }
        set
        {
            if (SpecialAction != null)
                SpecialAction.SpecialRemoved(this);
            _specialAction = value;
            SpecialAction.SpecialEquiped(this);
        }
    }

    private SpecialAction _specialAction;
    private float currentCharge;

    public void OnEnable()
    {
        specialUsedListeners = new List<SpecialUsedListener>(30);
        specialFullListeners = new List<SpecialFullListener>(30);
        currentCharge = MAX_VALUE;
        ActionManager actionManager = Singleton<ActionManager>.Instance;
        actionManager.RegisterStartButtonListener(InputButton.SECONDARY_ATTACK, ActivateSpecial);
    }

    /*
     * Increases the value by increase
     */ 
    public void IncreaseCharge(float increase)
    {
        if (increase <= 0)
            return;

        SetCurrentCharge(currentCharge + increase);
    }

    /*
     * Triggers the special action.
     */
    public void ActivateSpecial(InputButton inputButton)
    {
        if (currentCharge != MAX_VALUE)
            return;

        if (SpecialAction != null)
        {
            SpecialAction.DoAction(this);
            NotifySpecialUsedListeners();
        }
    }

    private void SetCurrentCharge(float newValue)
    {
        if (newValue < 0)
        {
            newValue = 0;
        }
        else if (newValue >= MAX_VALUE)
        {
            newValue = MAX_VALUE;
            if(currentCharge != MAX_VALUE)
                NotifySpecialFullListeners();
        }

        currentCharge = newValue;
    }

    private void NotifySpecialUsedListeners()
    {
        foreach (SpecialUsedListener usedListener in specialUsedListeners)
        {
            usedListener.SpecialUsed(this);
        }
    }

    private void NotifySpecialFullListeners()
    {
        foreach (SpecialFullListener fullListener in specialFullListeners)
        {
            fullListener.SpecialBarFull(this);
        }
    }
}