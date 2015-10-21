
namespace BirdNest.Rendering
{
	public interface IMeshSlotCollator
	{
		void Collate (SceneNode s1);

		MeshSlot[] Slots ();

		void Clear ();
	}
}

