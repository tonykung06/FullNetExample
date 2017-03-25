using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullNetExample.Domain
{
    public class SecretIdentity
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        public Samurai Smaurai { get; set; }
        public int SamuraiId { get; set; }
    }
}
