using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public int myPlayerIndex;

    [Space(15)]

    [SerializeField] private string upKey        = "Xbox{0}LStickUp";
    [SerializeField] private string downKey      = "Xbox{0}LStickDown";
    [SerializeField] private string leftKey      = "Xbox{0}LStickLeft";
    [SerializeField] private string rightKey     = "Xbox{0}LStickRight";
    [SerializeField] private string lookUpKey    = "Xbox{0}RStickUp";
    [SerializeField] private string lookDownKey  = "Xbox{0}RStickDown";
    [SerializeField] private string lookLeftKey  = "Xbox{0}RStickLeft";
    [SerializeField] private string lookRightKey = "Xbox{0}RStickRight";
    [SerializeField] private string attackKey    = "Xbox{0}A";
    [SerializeField] private string defenceKey   = "Xbox{0}B";

    // Inputs Variables
    [HideInInspector] public float axisHorizontal;
    [HideInInspector] public float axisVertical;
    [HideInInspector] public float axisLookHorizontal;
    [HideInInspector] public float axisLookVertical;

    [HideInInspector] public bool attack;
    [HideInInspector] public bool attackDown;

    [HideInInspector] public bool defence;
    [HideInInspector] public bool defenceDown;

    [HideInInspector] public bool isAxisOn { get { return axisDirectionClamped.sqrMagnitude > 0.0001f; } }

    [HideInInspector] public Vector2 axisDirection = new Vector2();
    [HideInInspector] public Vector2 axisDirectionClamped = new Vector2();

    [HideInInspector] public Vector3 worldAxisDirection = new Vector3();
    [HideInInspector] public Vector3 worldAxisDirectionClamped = new Vector3();

    public void Awake()
    {
        if (myPlayerIndex == 0)
            myPlayerIndex = GameManager.Instance.AddPlayer(this.GetComponent<Player>()) + 1;
        else
            GameManager.Instance.players[myPlayerIndex-1] = this.GetComponent<Player>();

        transform.name = "Player " + myPlayerIndex;

        InitInput();
    }

    public void InitInput()
    {
        int i = myPlayerIndex;

        cInput.SetKey(string.Format("P{0} Up",         i), GetKeyFromString(string.Format(upKey, i)));
        cInput.SetKey(string.Format("P{0} Down",       i), GetKeyFromString(string.Format(downKey, i)));
        cInput.SetKey(string.Format("P{0} Left",       i), GetKeyFromString(string.Format(leftKey, i)));
        cInput.SetKey(string.Format("P{0} Right",      i), GetKeyFromString(string.Format(rightKey, i)));
        cInput.SetKey(string.Format("P{0} Look Up",    i), GetKeyFromString(string.Format(lookUpKey, i)));
        cInput.SetKey(string.Format("P{0} Look Down",  i), GetKeyFromString(string.Format(lookDownKey, i)));
        cInput.SetKey(string.Format("P{0} Look Left",  i), GetKeyFromString(string.Format(lookLeftKey, i)));
        cInput.SetKey(string.Format("P{0} Look Right", i), GetKeyFromString(string.Format(lookRightKey, i)));
        cInput.SetKey(string.Format("P{0} Attack",     i), GetKeyFromString(string.Format(attackKey, i)));
        cInput.SetKey(string.Format("P{0} Defence",    i), GetKeyFromString(string.Format(defenceKey, i)));


        cInput.SetAxis(string.Format("P{0} Horizontal",      i),
            string.Format("P{0} Left",   i),
            string.Format("P{0} Right", i),
            3);

        cInput.SetAxis(string.Format("P{0} Vertical",        i),
            string.Format("P{0} Down", i),
            string.Format("P{0} Up",  i),
            3);

        cInput.SetAxis(string.Format("P{0} Look Horizontal", i),
            string.Format("P{0} Look Left",   i),
            string.Format("P{0} Look Right", i),
            3);

        cInput.SetAxis(string.Format("P{0} Look Vertical",   i),
            string.Format("P{0} Look Down", i),
            string.Format("P{0} Look Up",  i),
            3);
    }

    public static string GetKeyFromString(string keyName)
    {
        FieldInfo field = typeof(Keys).GetField(keyName);
        PropertyInfo property = typeof(Keys).GetProperty(keyName);

        if (field != null)
            return (string)field.GetValue(field);
        if (property != null)
            return (string)property.GetValue(property, null);
        return null;
    }


    public void Update()
    {
        attackDown = cInput.GetButtonDown(string.Format("P{0} Attack", myPlayerIndex));
        attack     = cInput.GetButton(    string.Format("P{0} Attack", myPlayerIndex));

        defenceDown = cInput.GetButtonDown(string.Format("P{0} Defence", myPlayerIndex));
        defence     = cInput.GetButton(    string.Format("P{0} Defence", myPlayerIndex));

        axisHorizontal = cInput.GetAxis(string.Format("P{0} Horizontal", myPlayerIndex));
        axisVertical   = cInput.GetAxis(string.Format("P{0} Vertical",   myPlayerIndex));
        
        axisLookHorizontal = cInput.GetAxis(string.Format("P{0} Look Horizontal", myPlayerIndex));
        axisLookVertical   = cInput.GetAxis(string.Format("P{0} Look Vertical",   myPlayerIndex));

        axisDirection.Set(axisHorizontal, axisVertical);
        axisDirectionClamped = axisDirection.SquareToCircleClamp();

        worldAxisDirection.Set(axisHorizontal, 0, axisVertical);
        worldAxisDirectionClamped = worldAxisDirection.CubeToSphereClamp();
    }

    public Vector3 JoystickRelativeToCamera()
    {
        Vector3 tempTarget =
            axisDirectionClamped.y * Camera.main.transform.forward +
            axisDirectionClamped.x * Camera.main.transform.right;

        tempTarget.y = 0f;

        return tempTarget;
    }
}
