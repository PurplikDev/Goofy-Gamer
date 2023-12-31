namespace goofygame.inventory.gun {
    public class Weapon {
        string _name;
        int _damage;
        float _range;
        float _attackSpeed;

        public string Name {
            get {
                return _name;
            }
        }
        public int Damage {
            get {
                return _damage;
            }
        }
        public float Range {
            get {
                return _range;
            }
        }
        public float AttackSpeed {
            get {
                return _attackSpeed;
            }
        }

        public Weapon(string name, int damage, float range, float attackSpeed) {
            _name = name;
            _damage = damage;
            _range = range;
            _attackSpeed = attackSpeed;
        }
    }

    public class WeaponItem : Item {
        private Weapon _weapon;
        public Weapon GetWeapon {
            get {
                return _weapon;
            }
        }

        public WeaponItem(string name, int damage, float range, float attackSpeed, float recoil) : this(name, 2.5f, damage, range, attackSpeed, recoil) {}

        public WeaponItem(string name, float itemCooldown, int damage, float range, float attackSpeed, float recoil) : base(name, itemCooldown) {
            _weapon = new Weapon(name, damage, range, attackSpeed);
        }
    }
}