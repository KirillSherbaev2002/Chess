using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour
{
    private GameObject[] NeedDrop;
    public GameObject TransitionEffect;
    public Material Golden;
    public Material Silver;
    [SerializeField] float Speed;
    [SerializeField] bool Controlled = true;
    public GameObject FallingPlatform;

    void Awake()
    {
        NeedDrop = GameObject.FindGameObjectsWithTag("NeedsToBeDropped");
        foreach (GameObject platform in NeedDrop)
        {
            platform.AddComponent<Rigidbody>();
            platform.transform.position = new Vector3(platform.transform.position.x + Random.Range(-0.1f, +0.1f), platform.transform.position.y + Random.Range(-0.3f, -0.6f), platform.transform.position.z + Random.Range(-0.1f, +0.1f));
            platform.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
        }
    }
    void Update()
    {
        //Чтобы при падении не управлялось
        if (gameObject.transform.position.y >= 0.9f && Controlled)
        {
            float inputH = Input.GetAxis("Horizontal");
            float inputV = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * -inputH + transform.right * inputV;
            gameObject.transform.position += moveDirection * Speed;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    IEnumerator Start()
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(10);
        foreach (GameObject platform in NeedDrop)
        {
            Destroy(platform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var Enemies = FindObjectsOfType<Enemy>();
        if (other.gameObject.tag == "DeadZone" && !Enemies[0].WasTouched && !Enemies[1].WasTouched)
        {
            gameObject.GetComponent<MeshRenderer>().material = Silver;
            Instantiate(TransitionEffect, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            Controlled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DropAfterTime")
        {
            FallingPlatform = collision.gameObject;
            StartCoroutine(AddRigit());
        }
    }
    IEnumerator AddRigit()
    {
        yield return new WaitForSeconds(1);
        FallingPlatform.AddComponent<Rigidbody>();
    }
}
