using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteep : MonoBehaviour
{
    private CharacterController cc;
    [SerializeField] AudioSource footstepSource;
    public float stepRate = 0.5f;
    public LayerMask groundMask;
    public float stepTimer = 0f;
    public float velocity;
    private FirstPersonController fpc;

    [SerializeField] AudioClip[] woodClips;
    [SerializeField] AudioClip[] metalClips;
    [SerializeField] AudioClip[] grassClips;
    [SerializeField] AudioClip[] defaultClips;
    // Start is called before the first frame update
    void Start()
    {
        fpc = GetComponent<FirstPersonController>();
        cc = GetComponent<CharacterController>();
        footstepSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (cc.isGrounded && fpc.velocityMagnitude > 0.2f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepRate;
            }
        }
        else
        {
            stepTimer = 0f; // reset si está en el aire
        }
    }


    AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }

    void PlayFootstep()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down, color: Color.red, 5f);
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundMask))
        {
            string tag = hit.collider.tag;

            switch (tag)
            {
                case "Metal":
                    footstepSource.PlayOneShot(GetRandomClip(metalClips));
                    break;
                case "Wood":
                    footstepSource.PlayOneShot(GetRandomClip(woodClips));
                    break;
                case "Grass":
                    footstepSource.PlayOneShot(GetRandomClip(grassClips));
                    break;
                default:
                    footstepSource.PlayOneShot(GetRandomClip(defaultClips));
                    break;
            }
        }
    }
}
