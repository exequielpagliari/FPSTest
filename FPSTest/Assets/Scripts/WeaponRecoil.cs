using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public GameObject gun;
    public AnimationCurve recoilCurve;
    public float recoilDuration = 0.1f;
    public float recoilAmount = 5f;

    private float timer = 0f;
    private bool isRecoiling = false;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = gun.transform.localRotation;
    }

    void Update()
    {
        if (isRecoiling)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / recoilDuration;

            if (normalizedTime >= 1f)
            {
                isRecoiling = false;
                gun.transform.localRotation = originalRotation;
                return;
            }

            float curveValue = recoilCurve.Evaluate(normalizedTime);
            float recoil = curveValue * recoilAmount;

            gun.transform.localRotation = originalRotation * Quaternion.Euler(-recoil, 0f, 0f); // e.g., cámara sube en Y
        }

        

    }

    public void Fire()
    {
        timer = 0f;
        isRecoiling = true;
    }
}
