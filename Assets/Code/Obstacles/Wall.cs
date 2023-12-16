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

    [SerializeField] float openingSpeed = 0.25f;
    [SerializeField] bool opened = false;
    [SerializeField] float wallLength = 2f;
    [SerializeField] ActionType actionType = ActionType.MOVING;
    [SerializeField] SpriteRenderer rend;

    GameObject wall;

    public override void ReceiveSignal()
    {
        ChangeOpeness();
    }

    private void Start()
    {
        wall = transform.GetChild(0).gameObject;

        rend.size = new Vector2(1, wallLength);

        if (opened)
        {
            //   wall.transform.localPosition = new Vector3(0, wallLength / 2,0);
            Open();
        }
        else
        {
            Close();
           // wall.transform.localPosition = new Vector3(0, -wallLength / 2, 0);
        }
    }

    void Open()
    {
        switch (actionType)
        {
            case ActionType.MOVING:
                wall.transform.DOLocalMoveY(wallLength / 2, openingSpeed) ;
                break;
            case ActionType.SCALING:
                wall.transform.DOLocalMoveY(wallLength / 2, openingSpeed);
                wall.transform.DOScaleY(1, openingSpeed);
                break;
        }
    }

    void Close()
    {
        switch (actionType)
        {
            case ActionType.MOVING:
                wall.transform.DOLocalMoveY(-wallLength / 2, openingSpeed);
                break;
            case ActionType.SCALING:
                wall.transform.DOLocalMoveY(0, openingSpeed);
                wall.transform.DOScaleY(0, openingSpeed);
                break;
        }
    }

    void ChangeOpeness()
    {
        opened = !opened;
        if (opened)
        {
            Open();
        }
        else
        {
            Close();
        }        

    }
}

