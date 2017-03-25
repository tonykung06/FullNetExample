using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullNetExample.ScaffoldFromDatabase
{
    public partial class SamuraiBattle
    {
        public int SamuraiId { get; set; }
        public int BattleId { get; set; }

        [ForeignKey("BattleId")]
        [InverseProperty("SamuraiBattle")]
        public virtual Battles Battle { get; set; }
        [ForeignKey("SamuraiId")]
        [InverseProperty("SamuraiBattle")]
        public virtual Samurais Samurai { get; set; }
    }
}
