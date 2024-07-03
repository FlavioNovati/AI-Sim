public interface IEatable: IPickable
{
    public float FeedAmount { get; }
    public void FeedMe(IEntity entityToFeed);
}
