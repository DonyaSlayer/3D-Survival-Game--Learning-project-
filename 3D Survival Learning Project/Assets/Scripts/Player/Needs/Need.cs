using Unity.VisualScripting;
using UnityEngine;

public abstract class Need
{
    public float Current { get; protected set; }
    public float Max { get; protected set; }
    protected float tickRate;
    protected float minValue = 0;
    public Need(float max, float newTickRate)
    {
        Max = max;
        Current = Max;
        tickRate = newTickRate;
    }

    public virtual void PermanentMinus(float deltaTime)
    {
        Current -= tickRate * deltaTime;
        Current = Mathf.Clamp(Current, minValue, Max);
    }

    public virtual void CustomPermanentMinus(float amount, float deltaTime)
    {
        Current -= amount * deltaTime;
        Current = Mathf.Clamp(Current, minValue, Max);
    }

    public virtual void Minus(float amount)
    {
        Current -= amount;
        Current = Mathf.Clamp(Current, minValue, Max);
    }

    public virtual void PermanentRestore(float amount, float deltaTime)
    {
        Current += amount * deltaTime;
        Current = Mathf.Clamp(Current, minValue, Max);
    }

    public virtual void Restore(float amount)
    {
        Current += amount;
        Current = Mathf.Clamp(Current, minValue, Max);
    }

    public virtual float GetPercentage()
    {
        return Current / Max;
    }

    public virtual bool IsEmpty()
    {
        return Current <= minValue;
    }
}

