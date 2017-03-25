using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullNetExample.ScaffoldFromDatabase
{
    public partial class Samurais
    {
        public Samurais()
        {
            Quotes = new HashSet<Quotes>();
            SamuraiBattle = new HashSet<SamuraiBattle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Samurai")]
        public virtual ICollection<Quotes> Quotes { get; set; }
        [InverseProperty("Samurai")]
        public virtual ICollection<SamuraiBattle> SamuraiBattle { get; set; }
        [InverseProperty("Samurai")]
        public virtual SecretIdentity SecretIdentity { get; set; }
    }
}
