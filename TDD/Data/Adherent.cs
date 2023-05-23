using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDD.Data
{
    [Table("Adherent")]
    public class Adherent
	{
        [Key]
        [Column("code")]
        public string Code { get; set; }
		public string Nom	 { get; set; }
		public string Prenom { get; set; }
		public DateTime DateNaissance { get; set; }
		public string Civilite { get; set; }
		public virtual IList<Livre> LivresReserves { get; set; }
	}
}

