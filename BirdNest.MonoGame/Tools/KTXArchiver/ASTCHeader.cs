using System;

namespace KTXArchiver
{
	public class ASTCHeader
	{
		//private byte[] magic; // magic[4];
		private readonly byte blockdim_x;
		private readonly byte blockdim_y;
		private readonly byte blockdim_z;
		private readonly byte[] xsize;   /* x-size = xsize[0] + xsize[1] + xsize[2] */
		private readonly byte[] ysize;   /* x-size, y-size and z-size are given in texels */
		private readonly byte[] zsize;   /* block count is inferred */

		public ASTCHeader (byte[] buffer)
		{
			const int BLOCKDIM_X = 4;
			blockdim_x = buffer [BLOCKDIM_X];	// 4
			const int BLOCKDIM_Y = 5;
			blockdim_y = buffer [BLOCKDIM_Y];	// 5
			const int BLOCKDIM_Z = 6;
			blockdim_z = buffer [BLOCKDIM_Z];	// 6

			const int X_SIZE_LIMIT = 3;
			const int X_SIZE_OFFSET = 7;
			xsize = new byte[X_SIZE_LIMIT];
			xsize [0] = buffer [X_SIZE_OFFSET];	//  7
			xsize [1] = buffer [X_SIZE_OFFSET + 1];	// 8 
			xsize [2] = buffer [X_SIZE_OFFSET + 2];	// 9

			const int Y_SIZE_LIMIT = 3;
			const int Y_SIZE_OFFSET = 10;
			ysize = new byte[Y_SIZE_LIMIT];
			ysize [0] = buffer [Y_SIZE_OFFSET];	// 10 
			ysize [1] = buffer [Y_SIZE_OFFSET + 1];	// 11
			ysize [2] = buffer [Y_SIZE_OFFSET + 2];	// 12

			const int Z_SIZE_LIMIT = 3;
			const int Z_SIZE_OFFSET = 13;
			zsize = new byte[Z_SIZE_LIMIT];
			zsize [0] = buffer [Z_SIZE_OFFSET];	// 13 
			zsize [1] = buffer [Z_SIZE_OFFSET + 1];	// 14
			zsize [2] = buffer [Z_SIZE_OFFSET + 2]; // 15

			FileSize = GetBytesToRead ();
		}

		public int FileSize {
			get;
			private set;
		}

		public int XBlocks { get; private set;}
		public int YBlocks { get; private set;}
		public int ZBlocks { get; private set;}

		private int GetBytesToRead ()
		{
			/* Merge x,y,z-sizes from 3 chars into one integer value. */
			int sizeX = xsize [0] + (xsize [1] << 8) + (xsize [2] << 16);
			int sizeY = ysize [0] + (ysize [1] << 8) + (ysize [2] << 16);
			int sizeZ = zsize [0] + (zsize [1] << 8) + (zsize [2] << 16);
			/* Compute number of blocks in each direction. */
			XBlocks = (sizeX + blockdim_x - 1) / blockdim_x;
			YBlocks = (sizeY + blockdim_y - 1) / blockdim_y;
			ZBlocks = (sizeZ + blockdim_z - 1) / blockdim_z;
			/* Each block is encoded on 16 bytes, so calculate total compressed image data size. */
			return (int)(XBlocks * YBlocks * ZBlocks << 4);
		}
	};
}

