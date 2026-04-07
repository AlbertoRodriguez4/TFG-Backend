using System.ComponentModel.DataAnnotations.Schema;

namespace AA2_CS.Model
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string passwordhash { get; set; } = string.Empty;
        public int level { get; set; } = 1;
        public int strength { get; set; } = 0;
        public int endurance { get; set; } = 0;
        public int consistencystreak { get; set; } = 0;
        public int gold { get; set; } = 0;
        public string role { get; set; } = string.Empty;
        public int experience { get; set; } = 0;
        public int? equippedStrengthId { get; set; }
        public int? equippedEnduranceId { get; set; }

        // --- NUEVO CAMPO PARA LA FOTO DE PERFIL ---
        public string? avatarUrl { get; set; }

        // --- CAMPO PARA VERIFICACIÓN DE EMAIL ---
        public bool isEmailVerified { get; set; } = false; 

        [ForeignKey("equippedStrengthId")]
        public virtual Item? EquippedStrengthItem { get; set; }

        [ForeignKey("equippedEnduranceId")]
        public virtual Item? EquippedEnduranceItem { get; set; }

        public User() { }

        // Opcional: Actualizar el constructor si lo usas
        public User(string name, string email, string passwordHash, int level, int strength, int endurance, int consistencyStreak, int gold, string role, int experience, int? equippedStrengthId = null, int? equippedEnduranceId = null, string? avatarUrl = null, bool isEmailVerified = false)
        {
            this.name = name;
            this.email = email;
            this.passwordhash = passwordHash;
            this.level = level;
            this.strength = strength;
            this.endurance = endurance;
            this.consistencystreak = consistencyStreak;
            this.gold = gold;
            this.role = role;
            this.experience = experience;
            this.equippedStrengthId = equippedStrengthId;
            this.equippedEnduranceId = equippedEnduranceId;
            this.avatarUrl = avatarUrl;
            this.isEmailVerified = isEmailVerified;
        }
    }
}