using UnityEngine;




public class Weapon : MonoBehaviour
{
    
    public enum TypeAttack 
    {
        Ray,
        Projectil,
        Mele
    };
    [Header("Type Of Weapon")]
    public TypeAttack typeAttack = TypeAttack.Ray;
    public LayerMask layerMask;

    public GameObject bullet;
    public Transform gunTransform;

    public float shootTime = 1;
    public float timeElapsed = 0;

    [Header("EffectsWeapon")]
    public GameObject goEffect;
    public ParticleSystem shootParticles;
    public GameObject shootLight;
    public WeaponRecoil weaponRecoil;

    [Header("Config Raycast")]
    public Camera camGun;

    void Start()
    {
        timeElapsed = Time.time + shootTime;
    }

    void Update()
    {

        if (Input.GetButton("Fire1"))
        {

            if (timeElapsed < Time.time && typeAttack == TypeAttack.Projectil)
            {
                shootParticles.Play();
                shootLight.SetActive(true);
                GameObject.Instantiate(bullet, gunTransform.position, gunTransform.rotation);
                timeElapsed = Time.time + shootTime;

            }
           
            if (timeElapsed < Time.time && typeAttack == TypeAttack.Ray)
            {
                shootParticles.Play();
                shootLight.SetActive(true);
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = camGun.ScreenPointToRay(screenCenter);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f,layerMask))
                {

                    if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
                    {
                        hitInfo.collider.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
                    }
                    timeElapsed = Time.time + shootTime;
                }

            }
            
        }
        else
        {
            shootParticles.Stop();
            shootLight.SetActive(false);
        }
    }

    private void ShootEffect()
    {
        
    }

}
