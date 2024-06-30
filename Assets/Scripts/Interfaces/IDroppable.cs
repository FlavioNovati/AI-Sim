public interface IDroppable: ITarget
{
    public delegate void Drop(IPickable pickableDrop);
    public event Drop OnDrop;
}
