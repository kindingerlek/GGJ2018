using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour {

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
            other.GetComponent<Player>().Die();
        else
            Destroy(other.gameObject);
    }
}
