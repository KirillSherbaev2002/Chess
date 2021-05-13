using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Singleton Pawn;
    public bool WasTouched = false;
    private void Awake()
    {
        Pawn = FindObjectOfType<Singleton>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "KillZone" && gameObject.GetComponent<MeshRenderer>().material != Pawn.Golden)
        {
            gameObject.GetComponent<MeshRenderer>().material = Pawn.Golden;
            Instantiate(Pawn.TransitionEffect, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
            WasTouched = true;
        }
    }
}
