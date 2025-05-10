public interface IStamina
{
    float MaxStamina{ get; }
    float CurrentStamina { get; }
    void LoseStamina(float amount);
    void RegenerateStamina(float amount);
}