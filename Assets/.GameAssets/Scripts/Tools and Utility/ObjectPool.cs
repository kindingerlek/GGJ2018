using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : Component
{

    public ObjectPool(T obj) : this(obj, 0, null, false) { }

    public ObjectPool(T obj, int amount) : this(obj, amount, null, false) { }

    public ObjectPool(T obj, int amount, Transform parent) : this(obj, amount, parent, false) { }

    public ObjectPool(T obj, Transform parent) : this(obj, 0, parent, false) { }

    public ObjectPool(T obj, Transform parent, bool hidden) : this(obj, 0, parent, hidden) { }

    public ObjectPool(T obj, bool hidden) : this(obj, 0, null, hidden) { }

    public ObjectPool(T obj, int amount, bool hidden) : this(obj, amount, null, hidden) { }

    public ObjectPool(T obj, int amount, Transform parent, bool hidden)
    {
        if (!obj)
            throw new ArgumentException("Object can't be null");

        if (amount < 0)
            throw new ArgumentException("Amount can't be less than 0");

        objects = new List<T>();
        objReference = obj;

        this.hidden = hidden;
        this.parent = parent;

        CreateNew(amount);
        pools.Add(this);
    }

    public bool hidden { get; private set; }
    public T objReference { get; private set; }
    public List<T> objects { get; private set; }
    public Transform parent { get; private set; }
    private static List<ObjectPool<T>> pools = new List<ObjectPool<T>>();

    public T CreateNew()
    {
        var newObject = Object.Instantiate(objReference);
        newObject.name = newObject.name.Replace("(Clone)", "");
        newObject.gameObject.SetActive(false);

        if (hidden)
            newObject.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        if (parent)
            newObject.transform.SetParent(parent);

        objects.Add(newObject);
        return newObject;
    }

    public T[] CreateNew(int amount)
    {
        var objs = new T[amount];

        for (int i = 0; i < amount; i++)
            objs[i] = CreateNew();

        return objs;
    }

    public T GetFree()
    {
        var freeObject = (from item in objects
                          where !item.gameObject.activeSelf
                          select item).FirstOrDefault();

        if (!freeObject)
            freeObject = CreateNew();

        freeObject.gameObject.SetActive(true);
        return freeObject;
    }

    public T GetFree(Vector3 position, Quaternion rotation)
    {
        var freeObject = GetFree();

        freeObject.transform.position = position;
        freeObject.transform.rotation = rotation;

        return freeObject;
    }

    public void SetFree(T obj)
    {
        if (!objects.Contains(obj))
            throw new ArgumentException("Object is not in objects list");

        obj.gameObject.SetActive(false);
    }

    public bool TrySetFree(T obj)
    {
        if (!objects.Contains(obj))
        {
            obj.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    public static void SetFreeToAll(T obj)
    {
        for (int i = 0; i < pools.Count; i++)
            pools[i].TrySetFree(obj);
    }

    public static implicit operator bool(ObjectPool<T> exists)
    {
        return exists != null;
    }
}