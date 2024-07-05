public interface IFeedable: IPickable
{
    public float FeedAmount { get; }
    public void FeedEntity(IEntity entityToFeed);
}
