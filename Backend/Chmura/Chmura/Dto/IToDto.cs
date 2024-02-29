namespace Chmura.Dto
{
	public interface IToDto<T> where T : class
	{
		T ToDto();
	}
}
