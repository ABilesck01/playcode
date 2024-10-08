using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOnEnable : MonoBehaviour
{
    private DOTweenAnimation DOTweenAnimation;

    private void Awake()
    {
        DOTweenAnimation = GetComponent<DOTweenAnimation>();
    }

    private void OnEnable()
    {
        DOTweenAnimation.DOPlay();
    }

    private void OnDisable()
    {
        DOTweenAnimation.DORewind();
    }
}
