namespace Magnesium.OpenGL
{
	public interface IMergeable<TData>
	{
		TData Merge (TData delta);
	}

}

