using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : Signalable
{
    enum ActionType
    {
        MOVING,
        SCALING
    }

    [SerializeField]
    float openingSpeed = 0.25f;

    bool opened = false;
    [SerializeField] float wallLength = 2f;
    GameObject wall;

    ActionType actionType = ActionType.MOVING;
    

    public override void ReceiveSignal()
    {
        ChangeOpeness();
    }

    private void Start()
    {
        wall = transform.GetChild(0).gameObject;

        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wallLength, wall.transform.localScale.z);

        if (opened)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    void Open()
    {
        switch (actionType)
        {
            case ActionType.MOVING:
                wall.transform.DOLocalMoveY(0, openingSpeed);
                break;
            case ActionType.SCALING:
                wall.transform.DOScaleY(wallLength, openingSpeed);
                break;
        }
    }

    void Close()
    {
        switch (actionType)
        {
            case ActionType.MOVING:
                wall.transform.DOLocalMoveY(-wallLength, openingSpeed);
                break;
            case ActionType.SCALING:
                wall.transform.DOScaleY(0, openingSpeed);
                break;
        }
    }

    void ChangeOpeness()
    {
        if (opened)
        {
            Open();
        }
        else
        {
            Close();
        }
        opened = !opened;

    }
}

