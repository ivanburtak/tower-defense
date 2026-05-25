````markdown name=DEVELOPMENT_GUIDE.md
# Tower Defense - Development Guide 📚

Complete technical documentation for developers and contributors.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                      GameManager (Core)                      │
│  - Health tracking    - Money management    - Win/Lose       │
└──────────┬────────────────────────────────────────┬──────────┘
           │                                        │
    ┌──────▼──────┐                         ┌──────▼──────┐
    │   Towers    │                         │   Enemies   │
    │  - Attack   │                         │  - Movement │
    │  - Target   │                         │  - Damage   │
    │  - Cost     │                         │  - Health   │
    └──────┬──────┘                         └──────┬──────┘
           │                                        │
    ┌──────▼──────┐                         ┌──────▼──────┐
    │ Projectiles │                         │ Waypoints   │
    │  - Movement │                         │  - Paths    │
    │  - Collision│                         │  - Progress │
    └─────────────┘                         └─────────────┘
           │
    ┌──────▼──────┐
    │   Combat    │
    │  - Damage   │
    │  - Effects  │
    └─────────────┘
```

## Core Systems

### 1. GameManager (Core/GameManager.cs)

**Purpose**: Central game state management and event system

**Key Methods**:
```csharp
// Money Management
bool TrySpendMoney(int amount)      // Spend money, returns success
void AddMoney(int amount)           // Add money to player
int GetCurrentMoney()               // Get current money

// Health Management
void TakeDamage(int damage)         // Reduce health
void Heal(int amount)               // Restore health
int GetCurrentHealth()              // Get current health

// Game Flow
void EndGame(bool playerWon)        // End the game
void ResetGame()                    // Reset for new playthrough
```

**Events**:
```csharp
event Action<int> OnHealthChanged   // Fire when health changes
event Action<int> OnMoneyChanged    // Fire when money changes
event Action OnGameOver             // Fire on game over
event Action OnGameWon              // Fire on victory
```

**Usage Pattern**:
```csharp
// Subscribe to events
GameManager.Instance.OnMoneyChanged += HandleMoneyChange;

// Use methods
if (GameManager.Instance.TrySpendMoney(100))
{
    // Tower purchased successfully
}
```

### 2. Tower System (Towers/)

**Class Hierarchy**:
```
Tower (Abstract Base)
├── BasicTower
├── RapidFireTower
└── SlowTower
```

**Tower Base Class Methods**:
```csharp
void FindTarget()           // Locate closest enemy
void RotateTowardsTarget()  // Aim at target
void Attack()               // Fire projectile
string GetTowerName()       // Get tower display name
int GetCost()              // Get tower cost
```

**Tower Comparison**:
| Tower | Cost | Damage | Range | Cooldown | Special |
|-------|------|--------|-------|----------|---------|
| Basic | $100 | 10 | 5 | 1.0s | None |
| RapidFire | $150 | 5 | 4 | 0.3s | 3.3x attack speed |
| Slow | $200 | 3 | 6 | 1.5s | Applies slow effect |

**Creating Custom Tower**:
```csharp
public class IceTower : Tower
{
    public override string GetTowerName() => "Ice Tower";
    public override int GetCost() => 175;

    protected override void Start()
    {
        attackRange = 5.5f;
        attackCooldown = 1.2f;
        damage = 8;
        base.Start();
    }

    protected override void OnAttack()
    {
        // Custom attack behavior
    }
}
```

### 3. Combat System (Combat/)

**Projectile Base Class**:
```csharp
void Initialize(Enemy target, int damage, float speed)
void MoveTowardsTarget()
void OnHit()                // Override for effects
```

**SlowProjectile Extends Projectile**:
```csharp
// Applies crowd control effect
void Initialize(Enemy target, int damage, float speed, 
                float slowMultiplier, float duration)
```

**Combat Flow**:
```
Tower Detects Enemy
    ↓
Tower Aims
    ↓
Attack Cooldown Ready
    ↓
Fire Projectile
    ↓
Projectile Moves
    ↓
Collision with Enemy
    ↓
Enemy Takes Damage
    ↓
Effects Applied (Slow, etc.)
```

### 4. Enemy System (Enemies/)

**Enemy AI**:
```csharp
// Movement
void FollowPath()           // Move along waypoints
void ReachedEnd()           // Enemy reached goal

// Health
void TakeDamage(int damage)
void Die()                  // Remove enemy, reward player

// Status Effects
void ApplySlow(float slowAmount, float duration)
```

**Enemy Lifecycle**:
```
Spawn at Waypoint 0
    ↓
Follow Waypoint Path (0 → 1 → 2 → 3 → 4 → 5)
    ↓
Take Damage from Towers
    ↓
Either:
  A) Reach End → Take Damage to Player
  B) Health Reaches 0 → Die & Reward Player
```

### 5. Wave System (Waves/WaveManager.cs)

**Wave Progression**:
```
Wave 1: 5 enemies
Wave 2: 8 enemies
Wave 3: 12 enemies
Wave 4: 15 enemies
Wave 5: 20 enemies
```

**Wave Flow**:
```csharp
// Each wave:
1. Wait timeBetweenWaves seconds
2. Start wave
3. Spawn enemies with timeBetweenSpawns spacing
4. Wait for all enemies to be defeated
5. Go to step 1

// After Wave 5:
Victory!
```

### 6. Tower Placement (Towers/TowerPlacement.cs)

**Placement System**:
```csharp
// User selects tower type
void StartPlacingTower(GameObject towerPrefab)

// Show preview following mouse
void UpdatePreviewPosition()

// Validate placement (not blocking other towers)
void ValidatePlacement()

// Confirm and deduct money
void PlaceTower(GameObject towerPrefab)
```

**Validation**:
- Green preview = Valid placement
- Red preview = Invalid placement (blocked)
- Placement only succeeds if player has enough money

## Event System

All major events are centralized in GameManager:

```csharp
// Subscribe
GameManager.Instance.OnHealthChanged += (health) =>
{
    Debug.Log($"Health now: {health}");
};

// Fire event (handled by GameManager internally)
GameManager.Instance.TakeDamage(10);  // Fires OnHealthChanged
```

**Best Practices**:
- Always unsubscribe in OnDestroy()
- Use null-coalescing operator (?.) for safety
- Subscribe in Start(), unsubscribe in OnDestroy()

## UI System

### GameUIManager

Displays real-time game information:
```csharp
void UpdateHealthDisplay(int health)
void UpdateMoneyDisplay(int money)
void ShowGameOverScreen()
void ShowGameWonScreen()
```

### TowerUIManager

Handles tower selection:
```csharp
void SelectBasicTower()
void SelectRapidFireTower()
void SelectSlowTower()
```

## Extension Points

### Adding New Tower Type

```csharp
// 1. Create new class extending Tower
public class FireTower : Tower
{
    public override string GetTowerName() => "Fire Tower";
    public override int GetCost() => 180;

    protected override void Start()
    {
        attackRange = 5.5f;
        attackCooldown = 0.8f;
        damage = 15;
        projectileSpeed = 12f;
        base.Start();
    }

    protected override void OnAttack()
    {
        // Fire-specific effects
        VisualEffects.CreateExplosionEffect(firePoint.position);
    }
}

// 2. Create FireProjectile if needed
public class FireProjectile : Projectile
{
    protected override void OnHit()
    {
        target.ApplyBurn(5, 3f);  // Custom effect
        VisualEffects.ScreenShake();
    }
}

// 3. Add to TowerUIManager buttons
```

### Adding Enemy Types

```csharp
// 1. Extend Enemy class
public class TankEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        maxHealth = 60;  // Twice the health
        speed = 1f;      // Half the speed
        moneyReward = 100; // More reward
    }
}

// 2. Modify WaveManager to spawn mix:
private void SpawnWaveCoroutine(int enemiesToSpawn)
{
    for (int i = 0; i < enemiesToSpawn; i++)
    {
        // 20% tank enemies, 80% normal
        GameObject prefab = Random.value < 0.2f 
            ? tankEnemyPrefab 
            : normalEnemyPrefab;
        
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenSpawns);
    }
}
```

### Adding Status Effects

```csharp
// 1. Add effect to Enemy
public void ApplyBurn(int damagePerSecond, float duration)
{
    StartCoroutine(BurnCoroutine(damagePerSecond, duration));
}

private IEnumerator BurnCoroutine(int dps, float duration)
{
    float elapsed = 0;
    while (elapsed < duration)
    {
        TakeDamage(dps);
        elapsed += 1f;
        yield return new WaitForSeconds(1f);
    }
}

// 2. Use from projectile
public class FireProjectile : Projectile
{
    protected override void OnHit()
    {
        if (target != null)
        {
            target.TakeDamage(damage);
            target.ApplyBurn(3, 5f);
        }
    }
}
```

## Performance Considerations

### Object Pooling

Currently implemented for enemies (EnemySpawner):
```csharp
// Pre-allocate 50 enemies
private int poolSize = 50;

// Reuse instead of instantiate/destroy
public Enemy SpawnEnemy(Vector3 position)
{
    GameObject enemy = enemyPool[poolIndex];
    poolIndex = (poolIndex + 1) % poolSize;
    enemy.SetActive(true);
    return enemy.GetComponent<Enemy>();
}
```

### Optimization Tips

1. **Use Object Pooling** for frequently spawned objects
2. **Reduce Collider Checks** with layer masking
3. **Limit Projectiles** with max projectile count
4. **Use Physics2D** queries instead of FindObjectOfType()
5. **Cache Component References** in Start()

## Debugging

### Debug Output

All systems use Debug.Log with emoji prefixes:

```
🎉 - Victory events
💀 - Game over events
🌊 - Wave events
🎯 - Tower events
💨 - Combat events
❄️ - Slow/status effects
💰 - Economy events
✓ - System initialization
⏱️ - Settings events
🎨 - Animation events
📺 - Visual effect events
❌ - Error conditions
```

### Testing Checklist

- [ ] Towers attack enemies correctly
- [ ] Money system works (earn/spend)
- [ ] Health system works (damage/death)
- [ ] Waves progress correctly
- [ ] 5 waves complete = victory
- [ ] Health reaches 0 = defeat
- [ ] Slow towers slow enemies
- [ ] Camera panning works
- [ ] Game speed control works
- [ ] UI displays correctly

## Common Issues

### Enemies Not Attacking Player

**Check**:
1. Waypoints are set up correctly
2. Waypoint indices are sequential (0, 1, 2, ...)
3. Final waypoint position is configured

### Towers Not Targeting

**Check**:
1. Tower has Circle Collider 2D as trigger
2. Enemy prefab has "Enemy" tag
3. Fire point is positioned correctly
4. Projectile prefab is assigned

### Money Not Updating

**Check**:
1. GameManager is in scene
2. Enemy is calling AddMoney() on death
3. UIManager is subscribed to OnMoneyChanged
4. TextMeshPro component exists

## Version History

**Version 1.0 (Month 2)**
- ✅ Core game manager
- ✅ 3 tower types
- ✅ Combat system
- ✅ Enemy AI
- ✅ Wave progression
- ✅ Economy system
- ✅ UI framework

**Version 1.1 (Month 3 - Planned)**
- [ ] Tower upgrades
- [ ] Additional enemy types
- [ ] Sound effects
- [ ] Animations
- [ ] Visual polish
- [ ] Main menu

## Contributing

To add new features:

1. Create feature branch from `development`
2. Implement changes with full documentation
3. Test thoroughly
4. Create pull request with description
5. Code review and merge

## License

MIT License - Educational use permitted

---

**Last Updated**: Month 2 Complete
**Next Phase**: Month 3 - Polish & Progression
````
