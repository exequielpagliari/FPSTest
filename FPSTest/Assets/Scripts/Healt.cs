using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Healt : MonoBehaviour
{
    public int Life;
    public DestroySystem destroySystem;

    public Healt(int amount)
        { Life = amount; }

    private void Awake()
    {
        destroySystem = GetComponent<DestroySystem>();
    }

    public void AddDamage(int amount)
    {
        Life = Mathf.Max(0, Life - amount);
        if(Life == 0)
        {
            GameObject.Instantiate(destroySystem.destroyParticles, this.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void AddHealt(int amount)
    {
        Life = Mathf.Min(100, Life + amount);
    }
}
