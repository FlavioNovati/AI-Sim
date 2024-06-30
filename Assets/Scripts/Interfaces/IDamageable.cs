public interface IDamageable
{
    public delegate void Death();
    public event Death OnDeath;

    public float HP { get; set; }
    public void TakeDamage(float damage);
}
