public interface IDamageable: ITarget
{
    public delegate void Death();
    public event Death OnDeath;

    public float HP { get; set; }
    public void TakeDamage(float damage);
}
