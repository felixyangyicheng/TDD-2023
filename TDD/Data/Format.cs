using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace TDD.Data
{
	public enum Format
	{
        Poche,
        [Display(Name = "Broché")]
        Broche,
        [Display(Name = "Grand Format")]
        GrandFormat
    }
}

