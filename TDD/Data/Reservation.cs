using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TDD.Data
{
	
	public class Reservation
	{
        [Key]
        [Column("id")]
        public int Id  { get; set; }
        
        [Column("isbn")]
        public string Isbn { get; set; }
        [Column("livre")]

        public virtual Livre Livre { get; set; }
        [Column("dateDebut")]

        public DateTime DateDebut { get; set; }
        [ForeignKey("adherentCode")]
        public string AdherentCode { get; set; }

        public virtual Adherent Adherent { get; set; }
        [Column("dateFin")]

        public DateTime? DateFin { get; set; }

        public bool IsValidDuration4Months()
        {
            bool result = false;
            var timeSpan = DateFin - DateDebut;
            if (timeSpan.Value.TotalSeconds>0)
            {
                result = true;
            }
            return result;
        }

    }
}

