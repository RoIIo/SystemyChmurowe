namespace Chmura.Dto
{
	public interface IToEntity<T> where T : class
	{
		T ToEntity();
	}
}
