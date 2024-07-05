public interface IDroppable
{
    public delegate void Drop(IPickable pickableDrop);
    public event Drop OnDrop;
}
