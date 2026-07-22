using UnityEngine;
using DG.Tweening;
using System;

public class MissileAnimate : MonoBehaviour
{

    [SerializeField] private GameObject missilePrefab;


    public void Launch(Vector3 from, Vector3 to, Action OnLanded)
    {
        transform.position = from;

        transform
            .DOJump(to, 2f, 1, 1.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(() => {
                                OnLanded();
                                Destroy(gameObject);
                                });
    }

}