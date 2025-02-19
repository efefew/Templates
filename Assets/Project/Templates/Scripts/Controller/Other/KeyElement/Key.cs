public abstract class Key
{
    public enum TypeClick
    {
        Down,
        Hold,
        Up
    }

    protected float _scale;

    public abstract float Get();
}