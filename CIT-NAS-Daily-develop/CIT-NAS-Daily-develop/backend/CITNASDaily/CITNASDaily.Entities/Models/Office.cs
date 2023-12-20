using System.ComponentModel.DataAnnotations;

namespace CITNASDaily.Entities.Models
{
	/// <summary>
	/// This is the class for the Office Model
	/// </summary>
	public class Office
	{
		[Key]
		public int Id { get; set; }
		public string? OfficeName { get; set; }	
		public string? SuperiorFirstName { get; set; }
		public string? SuperiorLastName { get; set; }
		public List<NAS>? NAS { get; set; }
	}
}
