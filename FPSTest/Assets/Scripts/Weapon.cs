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
    public int damage = 2;
    public float spreadAngle = 2f; // grados de dispersión

    [Header("EffectsWeapon")]
    public GameObject tracerPrefab;
    public GameObject goEffect;
    public ParticleSystem shootParticles;
    public GameObject shootLight;
    public WeaponRecoil weaponRecoil;
    private AudioSource audioSurce;
    public AudioClip shootClip;

    [Header("Config Raycast")]
    public Camera camGun;

    void Start()
    {
        audioSurce = GetComponent<AudioSource>();
        timeElapsed = Time.time + shootTime;
    }

    void Update()
    {
        
        Vector3 targetPoint; 


        if (Input.GetButton("Fire1"))
        {

            if (timeElapsed < Time.time && typeAttack == TypeAttack.Projectil)
            {
                if (!audioSurce.isPlaying)
                {
                    audioSurce.clip = shootClip;
                    audioSurce.Play();
                }
                audioSurce.Play();
                weaponRecoil.Fire();
                shootParticles.Play();
                shootLight.SetActive(true);
                GameObject.Instantiate(bullet, gunTransform.position, gunTransform.rotation);
                timeElapsed = Time.time + shootTime;
                
            }
           
            if (timeElapsed < Time.time && typeAttack == TypeAttack.Ray)
            {
                if (!audioSurce.isPlaying)
                {
                    audioSurce.clip = shootClip;
                    audioSurce.Play();
                }
                weaponRecoil.Fire();
                shootParticles.Play();
                shootLight.SetActive(true);
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = camGun.ScreenPointToRay(screenCenter);
                Vector3 direction = ray.direction;
                direction = ApplySpread(direction, spreadAngle);
                if (Physics.Raycast(ray.origin, direction, out RaycastHit hitInfo, 100f,layerMask))
                {                    
                    targetPoint = hitInfo.point;
                    if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
                    {
                        hitInfo.collider.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 10f;
                    }

                    if(hitInfo.collider.gameObject.GetComponent<Healt>())
                    {
                        hitInfo.collider.gameObject.GetComponent<Healt>().AddDamage(damage);
                    }
                    timeElapsed = Time.time + shootTime;
                }
                else
                {
                    targetPoint = ray.origin + ray.direction * 100f;
                }
                SpawnTracer(goEffect.transform.position, targetPoint);
            }
            
        }
        else
        {
            shootParticles.Stop();
            shootLight.SetActive(false);
        }
    }

    void SpawnTracer(Vector3 start, Vector3 end)
    {
        GameObject tracerGO = Instantiate(tracerPrefab);
        LineRenderer lr = tracerGO.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Destroy(tracerGO, 0.05f); // Desaparece rápido
    }

    Vector3 ApplySpread(Vector3 direction, float angleInDegrees)
    {
        // Convertimos el ángulo a radianes
        float spreadRadius = Mathf.Tan(angleInDegrees * Mathf.Deg2Rad);

        // Generamos un punto aleatorio dentro del círculo
        Vector2 randomPointInCircle = UnityEngine.Random.insideUnitCircle * spreadRadius;

        // Creamos una rotación desde la dirección base
        Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(right, direction).normalized;

        // Sumamos la desviación
        Vector3 spreadDirection = (direction + right * randomPointInCircle.x + up * randomPointInCircle.y).normalized;

        return spreadDirection;
    }
    private void ShootEffect()
    {
        
    }

}
