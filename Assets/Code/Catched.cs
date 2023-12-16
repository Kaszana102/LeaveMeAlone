using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Catched : MonoBehaviour
{
    [SerializeField] Transform transition;
    [SerializeField] float time;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Aloner"))
            transition.DOScale(new Vector3(7, 1, 7), time).OnComplete(SceneChange);
    }

    void SceneChange()
    {
        SceneLoader.LoadMainMenu();
    }
}
