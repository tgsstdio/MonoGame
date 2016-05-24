using System;
using System.Collections.Generic;

namespace MonoGame.Graphics
{
	public class DrawItemCompiler : IDrawItemCompiler
	{
		#region IDrawItemCompiler implementation

		private readonly IEffectCache mEffects;
		public DrawItemCompiler(IEffectCache effects)
		{
			mEffects = effects;
		}	

		static StateGroup CollateValues (StateGroup[] stack)
		{
			var dest = new StateGroup ();			
			for (int i = 0; i < stack.Length; ++i)
			{
				var src = stack [i];
				if (src != null)
				{
					if (!dest.State.HasValue && src.State.HasValue)
					{
						dest.State = src.State;
					}

					if (!dest.SlotIndex.HasValue && src.SlotIndex.HasValue)
					{
						dest.SlotIndex = src.SlotIndex;
					}
					if (!dest.TargetIndex.HasValue && src.TargetIndex.HasValue)
					{
						dest.TargetIndex = src.TargetIndex;
					}
					if (!dest.EffectIndex.HasValue && src.EffectIndex.HasValue)
					{
						dest.EffectIndex = src.EffectIndex;
					}
					if (!dest.Pass.HasValue && src.Pass.HasValue)
					{
						dest.Pass = src.Pass;
					}
//					if (!dest.ProgramIndex.HasValue && src.ProgramIndex.HasValue)
//					{
//						dest.ProgramIndex = src.ProgramIndex;
//					}
					if (!dest.ShaderOptions.HasValue && src.ShaderOptions.HasValue)
					{
						dest.ShaderOptions = src.ShaderOptions;
					}
					if (!dest.UniformsIndex.HasValue && src.UniformsIndex.HasValue)
					{
						dest.UniformsIndex = src.UniformsIndex;
					}
					if (!dest.MarkerIndex.HasValue && src.MarkerIndex.HasValue)
					{
						dest.MarkerIndex = src.MarkerIndex;
					}
					if (!dest.ResourceIndex.HasValue && src.ResourceIndex.HasValue)
					{
						dest.ResourceIndex = src.ResourceIndex;
					}
					if (!dest.MeshOffset.HasValue && src.MeshOffset.HasValue)
					{
						dest.MeshOffset = src.MeshOffset;
					}
					if (!dest.BufferMask.HasValue && src.BufferMask.HasValue)
					{
						dest.BufferMask = src.BufferMask;
					}
					if (!dest.Command.HasValue && src.Command.HasValue)
					{
						dest.Command = src.Command;
					}
					if (!dest.RasterizerValues.HasValue && src.RasterizerValues.HasValue)
					{
						dest.RasterizerValues = src.RasterizerValues;
					}
					if (!dest.StencilValues.HasValue && src.StencilValues.HasValue)
					{
						dest.StencilValues = src.StencilValues;
					}
					if (!dest.BlendValues.HasValue && src.BlendValues.HasValue)
					{
						dest.BlendValues = src.BlendValues;
					}
					if (!dest.Flags.HasValue && src.Flags.HasValue)
					{
						dest.Flags = src.Flags;
					}
				}
			}
			return dest;
		}

		int GetBitPosition (int value)
		{
			bool found = false;
			int i = 0;

			while (i < 32)
			{
				int mask = (1 << i);

				if ((value & mask) == mask)
				{
					found = true;
					break;
				}

				++i;
			}

			if (found)
				return i;
			else
				throw new InvalidOperationException ("Bit value not found");
				
		}

		private DrawItemCompilerOutput DeriveProgramIndex(StateGroup dest)
		{
			var output = new DrawItemCompilerOutput ();

			if (!dest.EffectIndex.HasValue)
			{
				throw new InvalidOperationException("No effect have been specified.");
			}

			if (dest.EffectIndex.HasValue)
			{
				Effect technique;
				if (!mEffects.TryGetValue (dest.EffectIndex.Value, out technique))
				{
					throw new ArgumentOutOfRangeException(paramName : "EffectIndex", message : "EffectIndex out of existing range");
				}

				if (!dest.Pass.HasValue)
				{
					throw new ArgumentNullException(paramName : "Pass", message : "Pass must be specified with EffectIndex");
				}

				int passIndex = GetBitPosition (dest.Pass.Value);
				EffectPass expectedPass = technique.Passes [passIndex];

				if (!dest.ShaderOptions.HasValue)
				{
					throw new ArgumentNullException(paramName : "ShaderOptions", message : "Value in DrawItem not supplied");
				}

				EffectPipeline variant;
				// TODO : filter based on vertex format AND options
				var permutations = expectedPass.Variants;

				if (!permutations.TryGetValue (dest.ShaderOptions.Value, out variant))
				{
					throw new ArgumentException(paramName : "ShaderOptions", message : "Variant not found for effect");
				}

				output.Pass = expectedPass;
				output.Variant = variant;
			}

			var item = new DrawItem ();

			output.Item = item;
			return output;
		}

		private DrawItemCompilerOutput ExtractItem(StateGroup group)
		{
			var output = DeriveProgramIndex (group);
			var result = new DrawItem ();
			result.ProgramIndex = output.Variant.Program.ProgramIndex;

			result.State = group.State.Value;
			result.SlotIndex = group.SlotIndex.Value;
			result.TargetIndex = group.TargetIndex.Value;


			result.UniformsIndex = group.UniformsIndex.Value;

			//dest.Pair = new ResourceListKey{ ListIndex = item.ResourceListIndex.Value, ItemIndex = item.ResourceItemIndex.Value };
		
			result.ResourceIndex = group.ResourceIndex.Value;
			result.BindingSet = group.MeshOffset.Value;
			result.MarkerIndex = group.MarkerIndex.Value;	
			result.BufferMask = group.BufferMask.Value;

//			dest.Command = item.Command.Value;
		//	dest.DrawCount = item.Command.Value.Count;
			result.Primitive = group.Command.Value.Primitive;


			result.RasterizerValues = group.RasterizerValues.Value;
			//dest.DepthBias = item.RasterizerValues.Value.DepthBias;
		//	dest.SlopeScaleDepthBias = item.RasterizerValues.Value.SlopeScaleDepthBias;


			result.StencilValues = group.StencilValues.Value;
//			dest.DepthCompareFunction = item.DepthStencilValues.Value.DepthCompareFunction;
//			dest.StencilAndDepthTestFailed = item.DepthStencilValues.Value.StencilAndDepthTestFailed;
//			dest.StencilTestPassed = item.DepthStencilValues.Value.StencilTestPassed; 
//			dest.StencilTestFailed = item.DepthStencilValues.Value.StencilTestFailed; 
//			dest.StencilAndDepthTestFailed= item.DepthStencilValues.Value.StencilAndDepthTestFailed; 
//			dest.StencilMask = item.DepthStencilValues.Value.StencilMask;
//			dest.StencilWriteMask = item.DepthStencilValues.Value.StencilWriteMask;		
//
			result.BlendValues = group.BlendValues.Value;
			result.DepthValues = group.DepthValues.Value;

			result.Flags = group.Flags.Value;
			output.Item = result;

			return output;
		}

		public void Validate(StateGroup dest)
		{
			if (!dest.State.HasValue)
			{
				throw new ArgumentNullException(paramName : "State", message : "Value in DrawItem not supplied");
			}

			if (!dest.SlotIndex.HasValue)
			{
				throw new ArgumentNullException(paramName : "SlotIndex", message : "Value in DrawItem not supplied");
			}
			if (!dest.TargetIndex.HasValue)
			{
				throw new ArgumentNullException(paramName : "TargetIndex", message : "Value in DrawItem not supplied");
			}

			if (!dest.UniformsIndex.HasValue)
			{
				throw new ArgumentNullException(paramName : "UniformsIndex", message : "Value in DrawItem not supplied");
			}
			if (!dest.MarkerIndex.HasValue)
			{
				throw new ArgumentNullException(paramName : "ResourceListIndex", message : "Value in DrawItem not supplied");
			}
			if (!dest.ResourceIndex.HasValue)
			{
				throw new ArgumentNullException(paramName : "ResourceItemIndex", message : "Value in DrawItem not supplied");
			}
			if (!dest.MeshOffset.HasValue)
			{
				throw new ArgumentNullException(paramName : "MeshIndex", message : "Value in DrawItem not supplied");
			}
			if (!dest.BufferMask.HasValue)
			{
				throw new ArgumentNullException(paramName : "BufferMask", message : "Value in DrawItem not supplied");
			}
			if (!dest.Command.HasValue)
			{
				throw new ArgumentNullException(paramName : "Command", message : "Value in DrawItem not supplied");
			}
			if (!dest.RasterizerValues.HasValue)
			{
				throw new ArgumentNullException(paramName : "RasterizerValues", message : "Value in DrawItem not supplied");
			}
			if (!dest.StencilValues.HasValue)
			{
				throw new ArgumentNullException(paramName : "StencilValues", message : "Value in DrawItem not supplied");
			}
			if (!dest.BlendValues.HasValue)
			{
				throw new ArgumentNullException(paramName : "BlendValues", message : "Value in DrawItem not supplied");
			}
			if (!dest.DepthValues.HasValue)
			{
				throw new ArgumentNullException(paramName : "DepthValues", message : "Value in DrawItem not supplied");
			}
			if (!dest.Flags.HasValue)
			{
				throw new ArgumentNullException(paramName : "Flags", message : "Value in DrawItem not supplied");
			}
		}

		public DrawItemCompilerOutput[] Compile (StateGroup[] stack)
		{
			var summary = CollateValues (stack);
			Validate (summary);
			// all ones

			var output = new List<StateGroup> ();
			if (summary.EffectIndex.HasValue && summary.Pass.HasValue)
			{
				Effect technique;
				if (!mEffects.TryGetValue (summary.EffectIndex.Value, out technique))
				{
					throw new ArgumentOutOfRangeException(paramName : "EffectIndex", message : "EffectIndex out of existing range");
				}

				foreach (var p in technique.Passes)
				{
					if ((p.Pass & summary.Pass) == p.Pass)
					{
						var alternative = CollateValues (new [] { new StateGroup{Pass=p.Pass}, p.PassGroup, summary});
						Validate (alternative);
						output.Add (alternative);
					}
				}
			}
			else
			{
				output.Add (summary);
			}

			var result = new List<DrawItemCompilerOutput> ();
			foreach (var item in output)
			{
				result.Add (ExtractItem (item));
			}

			return result.ToArray();
		}

		#endregion
	}
}

