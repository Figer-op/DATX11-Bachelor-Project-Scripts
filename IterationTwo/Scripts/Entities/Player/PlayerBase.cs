using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerBase : CharacterBase, IStamina, IDataPersistence<GameData>
{
    [field: Min(1)]
    [field: SerializeField]
    public float MaxStamina { get; private set; } = 100f;
    public float CurrentStamina { get; private set; }
    [field: Min(1)]
    [field: SerializeField]
    public float StaminaRegenValue { get; private set; } = 2f;

    private SerializableDictionary<string, WeaponEquipTimes> equippedWeapons;
    // Don't log the first weapon.
    private bool logEquippedWeapon = false;

    private float staminaTimer;
    [field: SerializeField]
    private float regenCooldown = 2f;

    public event Action<float> OnStaminaLost; // To enable UI to update and start cooldown.

    public event Action<float> OnStaminaGained; // To enable UI to update.
    // some subsribers might need different passed data  about the damage amount so event might seperate to two: 1 for UI and 1 for classes
    // for UI and other classes that want to interupt certain actions 

    [field: SerializeField]
    public AssetReference WeaponBaseAddress { get; private set; }

    public IEnumerable<WeaponAbilityLogic> PlayerWeaponAbilities => WeaponBase == null ? null : WeaponBase.WeaponAvailableAbilities;

    [SerializeField]
    private float immunityDuration = 0.5f;
    private bool isImmune = false;

    public event Action<WeaponBase> OnNewWeaponEquipped;

    public Vector2 StartPosition { private get; set; }

    private int deathCounter;

    [field: SerializeField]
    public DungeonType CurrentDungeon { get; private set; }

    private TimeDataHandler timeDataHandler;

    private void OnEnable()
    {
        OnStaminaLost += HandleStaminaLost;
    }

    private void OnDisable()
    {
        OnStaminaLost -= HandleStaminaLost;
    }

    private void HandleStaminaLost(float amount)
    {
        if (OnStaminaLost != null)
        {
            staminaTimer = 0f;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        CurrentStamina = MaxStamina;
        StartPosition = transform.position;

        timeDataHandler = FindAnyObjectByType<TimeDataHandler>();
    }

    private void Update()
    {
        StaminaRegenCooldown();
    }

    private void StaminaRegenCooldown()
    {
        staminaTimer += Time.deltaTime;
        if (staminaTimer >= regenCooldown)
        {
            RegenerateStamina(StaminaRegenValue);
            staminaTimer = regenCooldown * 0.75f;
        }
    }

    public void LoseStamina(float amount)
    {
        if (CurrentStamina >= amount)
        {
            float oldStamina = CurrentStamina;
            CurrentStamina = Mathf.Max(CurrentStamina - amount, 0);
            float lostAmount = oldStamina - CurrentStamina;

            OnStaminaLost?.Invoke(lostAmount);
        }
    }

    public void RegenerateStamina(float amount)
    {
        if (CurrentStamina < MaxStamina)
        {
            float oldStamina = CurrentStamina;
            CurrentStamina = Mathf.Min(CurrentStamina + amount, MaxStamina);
            float recoveredAmount = CurrentStamina - oldStamina;

            OnStaminaGained?.Invoke(recoveredAmount);
        }
    }

    public override void TakeDamage(float amount)
    {
        if (!isImmune)
        {
            StartCoroutine(ImmunityRoutine());
            base.TakeDamage(amount);
            
        }
        else Debug.Log("Can't be damaged, currently immune"); // temp Log remove after PR
    }

    private IEnumerator ImmunityRoutine()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityDuration); 
        isImmune = false;
    }

    public override void EquipNewWeapon(GameObject newWeapon)
    {
        DiscardWeapon();
        if (!newWeapon.TryGetComponent<WeaponBase>(out var weapon))
        {
            Debug.LogError("Could not find weapon base in new weapon!");
            return;
        }
        WeaponBaseAddress = weapon.WeaponBaseAddress;
        base.EquipNewWeapon(newWeapon);
        OnNewWeaponEquipped?.Invoke(weapon);
        if (logEquippedWeapon)
        {
            LogEquippedWeapon(newWeapon);
        }
        logEquippedWeapon = true;
    }

    private void LogEquippedWeapon(GameObject newWeapon)
    {
        string weaponName = newWeapon.name;
        if (!equippedWeapons.ContainsKey(weaponName))
        {
            equippedWeapons[weaponName] = new();
        }
        float currentTime = timeDataHandler.GetCurrentTime();
        equippedWeapons[weaponName].WeaponTimes.Add(currentTime);
    }

    private void DiscardWeapon()
    {
        if (WeaponBase == null)
        {
            return;
        }
        WeaponBase.DroppingWeapon(transform.position);
        WeaponBase = null;
    }

    public override void Die()
    {
        transform.position = StartPosition;
        Heal(MaxHealth);
        deathCounter++;
    }

    private void OnDestroy()
    {
        // Now that there is a sibling relationship between sprite and logic
        // have to destroy the parent like how it is with the enemies.
        Destroy(transform.parent.gameObject);
    }

    public void LoadData(GameData data)
    {
        CurrentHealth = data.DungeonData[CurrentDungeon].CurrentHealth;
        Addressables.LoadAssetAsync<GameObject>(data.DungeonData[CurrentDungeon].WeaponBase).Completed += OnWeaponLoaded;
        deathCounter = data.DungeonData[CurrentDungeon].AmountOfDeaths;
        equippedWeapons = data.DungeonData[CurrentDungeon].EquippedWeapons;
    }

    private void OnWeaponLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject weaponPrefab = handle.Result;
            if (weaponPrefab != null)
            {
                // Instantiate the weapon and assign it to the player
                GameObject EquippedWeapon = Instantiate(weaponPrefab, transform);
                EquipNewWeapon(EquippedWeapon);
            }
        }
        else
        {
            Debug.LogError($"Failed to load weapon with key: {handle.DebugName}");
        }
    }

    public void SaveData(GameData data)
    {
        data.DungeonData[CurrentDungeon].CurrentHealth = CurrentHealth;
        data.DungeonData[CurrentDungeon].WeaponBase = WeaponBaseAddress;
        data.DungeonData[CurrentDungeon].HasVisited = true;
        data.DungeonData[CurrentDungeon].AmountOfDeaths = deathCounter;
        data.DungeonData[CurrentDungeon].EquippedWeapons = equippedWeapons;
    }
}
