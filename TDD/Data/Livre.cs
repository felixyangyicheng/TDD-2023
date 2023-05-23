using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TDD.Data
{
	public class Livre
	{

        [Key]
        [Column("isbn")]
        public string Isbn { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Editeur { get; set; }
        public Format Format { get; set; }
        public bool Disponible { get; set; }


        public Livre(string isbn, string titre, string auteur, string editeur, Format format,bool dispo )
        {
            Isbn = isbn;
            Titre = titre;
            Auteur = auteur;
            Editeur = editeur;
            Format = format;
            Disponible = dispo;
        }



    }
}

