using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullNetExample.ScaffoldFromDatabase
{
    public partial class SecretIdentity
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        public int SamuraiId { get; set; }

        [ForeignKey("SamuraiId")]
        [InverseProperty("SecretIdentity")]
        public virtual Samurais Samurai { get; set; }
    }
}
