using UnityEngine;


public class Weapon : MonoBehaviour
{
    
    
    public GameObject bullet;
    public Transform gunTransform;

    public float shootTime = 1;
    public float timeElapsed = 0;

    void Start()
    {
        timeElapsed = Time.time + shootTime;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(timeElapsed < Time.time)
            {
                GameObject.Instantiate(bullet, gunTransform.position, gunTransform.rotation);
                timeElapsed = Time.time + shootTime;
            }
        }
    }
}
