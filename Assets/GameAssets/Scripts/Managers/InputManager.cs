using UnityEngine;
using System.Collections;
using System.Reflection;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public bool storePlayerPref = true;
    public KeyInput[] CustomKeys;
    public AxisInput[] CustomAxis;
    private bool hasInitilized;

    static InputManager()
    {
        Lazy = false;
        FindInactive = true;
        DestroyOthers = DestroyOptions.DestroyBehaviour;
        Persist = true;
    }

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

    public static void ChangePrimary(int i)
    {
        cInput.ChangeKey(Instance.CustomKeys[i].action, 1);
    }

    public static void ChangeSecondary(int i)
    {
        cInput.ChangeKey(Instance.CustomKeys[i].action, 2);
    }

    public static void ResetAll()
    {
        cInput.Clear();
        foreach (KeyInput key in Instance.CustomKeys)
        {
            key.SetDefault();
        }
    }

    public static string GetStringFromKeys(string keyName)
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
}