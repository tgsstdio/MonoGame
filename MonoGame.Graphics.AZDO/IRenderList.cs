namespace MonoGame.Graphics.AZDO
{
	public interface IRenderList
	{
		bool Push (DrawItem[] items, out DrawItemOffset output);
		DrawItem[] ToArray();
	}

}

