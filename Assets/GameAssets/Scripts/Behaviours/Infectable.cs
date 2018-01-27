using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infectable : MonoBehaviour {

    public GameObject infectedBy = null;

    private new Renderer renderer;
    private Material originalMaterial;


    public void Awake()
    {
        renderer = GetComponent<Renderer>();

        originalMaterial = renderer.material;
    }

    public void Infect(GameObject infector)
    {
        if (infectedBy.GetInstanceID() == infector.GetInstanceID())
            return;
        
        Debug.Log(this.name + ": I was infected by " + infector.name);
        infectedBy = infector;

        renderer.material = infector.GetComponent<Renderer>().material;

    }
}
