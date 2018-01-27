using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public bool storePlayerPref = true;
    public KeyInput[] CustomKeys;
    public AxisInput[] CustomAxis;
    private bool hasInitilized;

    void Start()
    {
        InitInput();
    }

    private void InitInput()
    {
        if (hasInitilized)
            return;

        if (CustomKeys == null || CustomAxis == null)
            return;

        cInput.usePlayerPrefs = storePlayerPref;

        // Initializing Keys        
        for (int i = 0; i < CustomKeys.Length; i++)
        {
            KeyInput key = CustomKeys[i];

            if (key.action.Length == 0 || key.defPrimary.Length == 0)
                Debug.LogError("Please define a name or primary key to key: " + i);
            else
                cInput.SetKey
                (
                    key.action,
                    GetStringFromKeys(key.defPrimary),
                    GetStringFromKeys(key.defSecondary)
                );
        }

        // Initializing Axis
        for (int i = 0; i < CustomAxis.Length; i++)
        {
            AxisInput axis = CustomAxis[i];

            if (axis.name.Length == 0 || axis.negative.Length == 0 || axis.positive.Length == 0)
                Debug.LogError("Please define a name, negative key or positive key to axis: " + i);
            else
                cInput.SetAxis
                (
                    axis.name,
                    axis.negative,
                    axis.positive,
                    axis.sensitivity,
                    axis.gravity,
                    axis.deadzone
                );
        }

        hasInitilized = true;
    }
    

    public string GetStringFromKeys(string keyName)
    {
        FieldInfo field = typeof(Keys).GetField(keyName);
        PropertyInfo property = typeof(Keys).GetProperty(keyName);

        if (field != null)
            return (string)field.GetValue(field);
        if (property != null)
            return (string)property.GetValue(property, null);
        return null;
    }

    [System.Serializable]
    public class KeyInput
    {
        public string action;
        public string defPrimary;
        public string defSecondary;

        public void SetDefault()
        {
            cInput.ChangeKey(action, defPrimary, defSecondary);
        }
    }

    [System.Serializable]
    public class AxisInput
    {
        public string name;             //The description of what the axis is used for.
        public string positive;         //The input which provides the positive value of the axis.
        public string negative;         //The input which provides the negative value of the axis.
        [Range(0, 10)]
        public float sensitivity = 3f;      //Optional. The sensitivity for this axis. Default is 3.
        [Range(0, 10)]
        public float gravity = 3f;          //Optional. How fast the axis returns to 0 when input stops. Default is 3.
        [Range(0, 01)]
        public float deadzone = 0.001f; //Optional. How fast the axis returns to 0 when input stops. Default is 0.001.
    }





    // Inputs Variables
    [HideInInspector] public float axisHorizontal;
    [HideInInspector] public float axisVertical;
    [HideInInspector] public bool action1;
    [HideInInspector] public bool action1Down;
    [HideInInspector] public bool isAxisOn { get { return axisDirectionClamped.sqrMagnitude > 0.05f; } }

    [HideInInspector] public Vector2 axisDirection = new Vector2();
    [HideInInspector] public Vector2 axisDirectionClamped = new Vector2();

    [HideInInspector] public Vector3 worldAxisDirection = new Vector3();
    [HideInInspector] public Vector3 worldAxisDirectionClamped = new Vector3();



    public void Update()
    {
        action1Down = cInput.GetButtonDown("Action 1");
        action1 = cInput.GetButton("Action 1");

        axisHorizontal = cInput.GetAxis("Horizontal");
        axisVertical = cInput.GetAxis("Vertical");
        
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
