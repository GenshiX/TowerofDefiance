# Script Overview

## Combat
- `Damageable.cs`: Base class for things that take damage.
- `AttackBehavior.cs`: Abstract attack style.
- `DirectAttackBehavior.cs`: Instant damage attack.
- `ProjectileAttackBehavior.cs`: Single projectile attack.
- `BeamAttackBehavior.cs`: Instant beam-style attack.
- `MultiShotAttackBehavior.cs`: Multiple projectile attack.
- `Projectile.cs`: Homing projectile.

## Units
- `Unit.cs`: Shared idle attack loop.
- `UnitData.cs`: Base unit stats and upgrade list.
- `UnitStats.cs`: Runtime calculated stats.
- `UnitUpgradeState.cs`: Current upgrade levels for one unit.

## Enemies
- `Enemy.cs`: Runtime health, damage, death event.
- `EnemyData.cs`: Enemy base stats, rewards, scaling.

## Levels
- `TowerLevel.cs`: Owns current enemy and spawns the next one.
- `TowerLevelData.cs`: Level theme, available units, enemy rotation.
- `TowerManager.cs`: Lookup point for multiple levels.

## Upgrades
- `UpgradeData.cs`: Upgrade effect and cost scaling.
- `UpgradeEffectType.cs`: Enum of supported effects.
- `UpgradeManager.cs`: Draft purchase/apply logic.

## Economy
- `PlayerWallet.cs`: Placeholder gold wallet.
