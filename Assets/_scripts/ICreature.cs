namespace goofygame.creature {
    public interface ICreature {
        public void Heal(int amount = 1);
        public void Damage(int amount = 1);
    }
}