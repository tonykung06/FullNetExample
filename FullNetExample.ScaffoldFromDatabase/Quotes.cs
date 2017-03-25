using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullNetExample.ScaffoldFromDatabase
{
    public partial class Quotes
    {
        public int Id { get; set; }
        public int SamuraiId { get; set; }
        public string Text { get; set; }

        [ForeignKey("SamuraiId")]
        [InverseProperty("Quotes")]
        public virtual Samurais Samurai { get; set; }
    }
}
