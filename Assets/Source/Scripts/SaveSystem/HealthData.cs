namespace AdventureWorld.Prueba
{
    [System.Serializable]
    internal struct HealthData
    {
        public float hp;
        public float BaseHP;
        public float additiveHp;
        public float multiplicativeHp;

        public HealthData(HealthUnit hp)
        {
            this.hp = hp.HP;
            this.BaseHP = hp.BaseHP;
            this.additiveHp = hp.AddictiveHP;
            this.multiplicativeHp = hp.MultiplierHP;
        }
        
        public void toHealthUnit(HealthUnit hp)
        {
            hp.HP = this.hp;
            {
                //Todos estos valores son actualizados por el equipamiento, buffs y levels, por lo que al asignar esos valores, esto no es necesario

                // hp.BaseHP = this.BaseHP;
                // hp.AddictiveHP = this.additiveHp;
                // hp.MultiplierHP = this.multiplicativeHp;
            }

        }
    }
}