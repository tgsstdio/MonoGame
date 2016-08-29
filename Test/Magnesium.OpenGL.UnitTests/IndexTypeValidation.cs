using System;
using NUnit.Framework;

namespace Magnesium.OpenGL.UnitTests
{
	[TestFixture]
	public class IndexTypeValidation
	{
		[TestCase]
		public void NoIndexBufferPassedIn()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType(repo, new GLCmdDrawCommand { IndexBuffer = null });
			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void BufferInButNotIndexedDrawAnd32Bit()
		{	
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT32,
			};
			repo.IndexBuffers.Add(indexBuffer);

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType (repo, new GLCmdDrawCommand { IndexBuffer = 0 });
			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is16Bit()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();

			var indexBuffer = new GLCmdIndexBufferParameter {
				indexType = MgIndexType.UINT16,
			};
			repo.IndexBuffers.Add(indexBuffer);

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType (repo, new GLCmdDrawCommand { IndexBuffer = 0 });
			Assert.AreEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is16BitIndexedIndirectDraw()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();
			var indexBuffer = new GLCmdIndexBufferParameter
			{
				indexType = MgIndexType.UINT16,
			};
			repo.IndexBuffers.Add(indexBuffer);

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType (
				repo, 
				new GLCmdDrawCommand 
				{ 
					IndexBuffer = 0,
					DrawIndexedIndirect = new GLCmdInternalDrawIndexedIndirect
					{
					
					}
				}
			); 
			Assert.AreEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is16BitIndexedDraw()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();
			var indexBuffer = new GLCmdIndexBufferParameter
			{
				indexType = MgIndexType.UINT16,
			};
			repo.IndexBuffers.Add(indexBuffer);

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType(
				repo,
				new GLCmdDrawCommand
				{
					IndexBuffer = 0,
					DrawIndexed = new GLCmdInternalDrawIndexed
					{

					}
				});
			Assert.AreEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is32BitIndexedIndirectDraw()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();
			var indexBuffer = new GLCmdIndexBufferParameter
			{
				indexType = MgIndexType.UINT32,
			};
			repo.IndexBuffers.Add(indexBuffer);

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType(
				repo,
				new GLCmdDrawCommand
				{
					IndexBuffer = 0,
				DrawIndexedIndirect = new GLCmdInternalDrawIndexedIndirect
					{

					}
				});

			Assert.AreNotEqual (GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		[TestCase]
		public void Is32BitIndexedDraw()
		{
			IGLCmdBufferRepository repo = new GLCmdBufferRepository();
			var indexBuffer = new GLCmdIndexBufferParameter
			{
				indexType = MgIndexType.UINT32,
			};
			repo.IndexBuffers.Add(indexBuffer);

			var actual = CmdBufferInstructionSetTransformer.ExtractIndexType(
				repo,
				new GLCmdDrawCommand
				{
					IndexBuffer = 0,
					DrawIndexed = new GLCmdInternalDrawIndexed
					{

					}
				});

			Assert.AreNotEqual(GLCommandBufferFlagBits.Index16BitMode, actual);
		}

		class MockGLCmdBufferRepository : IGLCmdBufferRepository
		{
			public IGLCmdBufferStore<int> BackCompareMasks
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<int> BackReferences
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<int> BackWriteMasks
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<MgColor4f> BlendConstants
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdDepthBiasParameter> DepthBias
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdDepthBoundsParameter> DepthBounds
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdDescriptorSetParameter> DescriptorSets
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<int> FrontCompareMasks
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<int> FrontReferences
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<int> FrontWriteMasks
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<IGLGraphicsPipeline> GraphicsPipelines
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdIndexBufferParameter> IndexBuffers
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<float> LineWidths
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdScissorParameter> Scissors
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdVertexBufferParameter> VertexBuffers
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public IGLCmdBufferStore<GLCmdViewportParameter> Viewports
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool MapRepositoryFields(ref GLCmdDrawCommand command)
			{
				throw new NotImplementedException();
			}

			public void PushBlendConstants(MgColor4f blendConstants)
			{
				throw new NotImplementedException();
			}

			public void PushDepthBias(float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor)
			{
				throw new NotImplementedException();
			}

			public void PushDepthBounds(float minDepthBounds, float maxDepthBounds)
			{
				throw new NotImplementedException();
			}

			public void PushGraphicsPipeline(IGLGraphicsPipeline glPipeline)
			{
				throw new NotImplementedException();
			}

			public void PushLineWidth(float lineWidth)
			{
				throw new NotImplementedException();
			}

			public void PushScissors(uint firstScissor, MgRect2D[] pScissors)
			{
				throw new NotImplementedException();
			}

			public void PushViewports(uint firstViewport, MgViewport[] pViewports)
			{
				throw new NotImplementedException();
			}

			public void SetCompareMask(MgStencilFaceFlagBits face, uint mask)
			{
				throw new NotImplementedException();
			}

			public void SetStencilReference(MgStencilFaceFlagBits face, uint mask)
			{
				throw new NotImplementedException();
			}

			public void SetWriteMask(MgStencilFaceFlagBits face, uint mask)
			{
				throw new NotImplementedException();
			}
		}
	}
}

