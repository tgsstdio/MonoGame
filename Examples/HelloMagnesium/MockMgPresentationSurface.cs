using System;
using Magnesium;

namespace HelloMagnesium
{
    internal class MockMgPresentationSurface : IMgPresentationSurface
    {
        public IMgSurfaceKHR Surface
        {
            get
            {
                return null;
            }
        }

        public void Dispose()
        {

        }

        public void Initialize()
        {

        }
    }
}