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

        int infectRes = 1;

        if (infectRes == 1){
            Debug.Log(this.name + ": I was infected by " + infector.name);
            infectedBy = infector;
        } else if (infectRes == -1) {
            Debug.Log(infector.name + ": I was infected by " + this.name);
            infector.GetComponent<Infectable>().infectedBy = this.GetComponent<GameObject>();
        }
        
        Debug.Log(this.name + ": I was infected by " + infector.name);
        infectedBy = infector;

        renderer.material = infector.GetComponent<Renderer>().material;

    }
}
