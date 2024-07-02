public interface IEatable: IPickable
{
    public float FeedAmount { get; set; }
    public void FeedMe(IEntity entityToFeed);
}
