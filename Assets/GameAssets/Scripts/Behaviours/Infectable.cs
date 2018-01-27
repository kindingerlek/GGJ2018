using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infectable : MonoBehaviour {

    public Player infectedBy = null;

    private new Renderer renderer;
    private Material originalMaterial;


    public void Awake()
    {
        renderer = GetComponent<Renderer>();

        originalMaterial = renderer.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Infectable infectable = collision.gameObject.GetComponent<Infectable>();

        if (infectable == null)
            return;

        infectable.Infect(this.gameObject);
    }
    

    public void Infect(GameObject other)
    {
        if (infectedBy != null)
        {
            if (infectedBy.GetInstanceID() == other.GetInstanceID())
                return;
        }

        int i1 = this.infectedBy ? this.infectedBy.playerIndex : 0 ;
        int i2 = 0;


        if (other.GetComponent<Player>())
            i2 = other.GetComponent<Player>().playerIndex;

        else if (other.GetComponent<Infectable>())
            i2 = other.GetComponent<Infectable>().infectedBy == null ? 0 : other.GetComponent<Infectable>().infectedBy.playerIndex;



        RuleManager.collisionType infectRes = RuleManager.Instance.CompareInfectation(i1, i2);



        if (infectRes == RuleManager.collisionType.MeInfectOther){
            Debug.Log(this.name + ": I infect " + other.name);

            if (other.GetComponent<Infectable>())
                other.GetComponent<Infectable>().infectedBy = this.infectedBy;
            
            other.GetComponent<Renderer>().material = renderer.material;

        } else if (infectRes == RuleManager.collisionType.OtherInfectMe) {
            Debug.Log(this.name + ": I was infected by " + other.name);

            if (other.GetComponent<Infectable>())
                this.infectedBy = other.GetComponent<Infectable>().infectedBy; 

            renderer.material = other.GetComponent<Renderer>().material;
        }

        GameManager.Instance.CountPoints();
    }
}
