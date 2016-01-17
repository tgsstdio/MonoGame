using System;
using System.Collections.Generic;

namespace MonoGame.Graphics
{
	public class DrawItemCompiler : IDrawItemCompiler
	{
		#region IDrawItemCompiler implementation

		private IEffectCache mEffects;
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
				if (!dest.ProgramIndex.HasValue && src.ProgramIndex.HasValue)
				{
					dest.ProgramIndex = src.ProgramIndex;
				}
				if (!dest.ShaderOptions.HasValue && src.ShaderOptions.HasValue)
				{
					dest.ShaderOptions = src.ShaderOptions;
				}
				if (!dest.UniformsIndex.HasValue && src.UniformsIndex.HasValue)
				{
					dest.UniformsIndex = src.UniformsIndex;
				}
				if (!dest.ResourceListIndex.HasValue && src.ResourceListIndex.HasValue)
				{
					dest.ResourceListIndex = src.ResourceListIndex;
				}
				if (!dest.ResourceItemIndex.HasValue && src.ResourceItemIndex.HasValue)
				{
					dest.ResourceItemIndex = src.ResourceItemIndex;
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
			return dest;
		}

		private byte DeriveProgramIndex(StateGroup dest)
		{
			if (!dest.EffectIndex.HasValue && !dest.ProgramIndex.HasValue)
			{
				throw new InvalidOperationException("Neither effect or specific program have been specified.");
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

				// for all passes
				if (dest.Pass.Value != ~0 && dest.Pass.Value >= technique.Passes.Length)
				{
					throw new ArgumentOutOfRangeException(paramName : "Pass", message : "Pass out of existing range");
				}

				EffectPass expectedPass = technique.Passes [dest.Pass.Value];
				if (!dest.ShaderOptions.HasValue)
				{
					throw new ArgumentNullException(paramName : "ShaderOptions", message : "Value in DrawItem not supplied");
				}

				EffectShaderVariant variant;
				// TODO : filter based on vertex format AND options
				var permutations = expectedPass.Variants;

				if (!permutations.TryGetValue (dest.ShaderOptions.Value, out variant))
				{
					throw new ArgumentException(paramName : "ShaderOptions", message : "Variant not found for effect");
				}

				return variant.Program.ProgramIndex;
			}
			else
			{
				return dest.ProgramIndex.Value;
			}
		}

		private DrawItem ExtractItem(StateGroup item)
		{
			var dest = new DrawItem ();
			dest.State = item.State.Value;
			dest.SlotIndex = item.SlotIndex.Value;
			dest.TargetIndex = item.TargetIndex.Value;

			dest.ProgramIndex = DeriveProgramIndex (item);
			dest.UniformsIndex = item.UniformsIndex.Value;

			//dest.Pair = new ResourceListKey{ ListIndex = item.ResourceListIndex.Value, ItemIndex = item.ResourceItemIndex.Value };
			dest.ResourceListIndex = item.ResourceListIndex.Value;			
			dest.ResourceItemIndex = item.ResourceItemIndex.Value;
			dest.MeshIndex = item.MeshOffset.Value;
			dest.BufferMask = item.BufferMask.Value;

//			dest.Command = item.Command.Value;
			dest.DrawCount = item.Command.Value.Count;
			dest.Primitive = item.Command.Value.Primitive;


			dest.RasterizerValues = item.RasterizerValues.Value;
			//dest.DepthBias = item.RasterizerValues.Value.DepthBias;
		//	dest.SlopeScaleDepthBias = item.RasterizerValues.Value.SlopeScaleDepthBias;


			dest.StencilValues = item.StencilValues.Value;
//			dest.DepthCompareFunction = item.DepthStencilValues.Value.DepthCompareFunction;
//			dest.StencilAndDepthTestFailed = item.DepthStencilValues.Value.StencilAndDepthTestFailed;
//			dest.StencilTestPassed = item.DepthStencilValues.Value.StencilTestPassed; 
//			dest.StencilTestFailed = item.DepthStencilValues.Value.StencilTestFailed; 
//			dest.StencilAndDepthTestFailed= item.DepthStencilValues.Value.StencilAndDepthTestFailed; 
//			dest.StencilMask = item.DepthStencilValues.Value.StencilMask;
//			dest.StencilWriteMask = item.DepthStencilValues.Value.StencilWriteMask;		
//
			dest.BlendValues = item.BlendValues.Value;
			dest.DepthValues = item.DepthValues.Value;

			dest.Flags = item.Flags.Value;
			return dest;
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
			if (!dest.ResourceListIndex.HasValue)
			{
				throw new ArgumentNullException(paramName : "ResourceListIndex", message : "Value in DrawItem not supplied");
			}
			if (!dest.ResourceItemIndex.HasValue)
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

		public DrawItem[] Compile (StateGroup[] stack)
		{
			var summary = CollateValues (stack);
			Validate (summary);
			// all ones

			var output = new List<StateGroup> ();
			if (summary.EffectIndex.HasValue && summary.Pass.HasValue && summary.Pass == ~0)
			{
				Effect technique;
				if (!mEffects.TryGetValue (summary.EffectIndex.Value, out technique))
				{
					throw new ArgumentOutOfRangeException(paramName : "EffectIndex", message : "EffectIndex out of existing range");
				}

				foreach (var p in technique.Passes)
				{
					var alternative = CollateValues (new StateGroup[] { new StateGroup{Pass=p.Pass}, summary});
					output.Add (alternative);
				}
			}
			else
			{
				output.Add (summary);
			}

			var result = new List<DrawItem> ();
			foreach (var item in output)
			{
				result.Add (ExtractItem (item));
			}

			return result.ToArray();
		}

		#endregion
	}
}

