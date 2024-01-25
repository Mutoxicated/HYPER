using System;

[Serializable]
public class Shield 
{
    public float maxHealth;
    private float health;

    public Shield(float health, bool isPerma){
        this.health = health;
        maxHealth = this.health;
    }

    public float TakeDamage(float intake){
        health -= intake;
        return health;
    }
}
