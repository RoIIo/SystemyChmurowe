namespace Chmura.Database.Entities
{
	public class Pollen
	{
		public virtual int Id { get; set; }
		public virtual string? Name { get; set; }
		public virtual IList<Honey>? HoneyList { get; set; }
	}
}
