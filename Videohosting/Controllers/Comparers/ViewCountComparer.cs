using System;
using System.Collections.Generic;
using Videohosting.Models;

namespace Videohosting.Controllers.Comparers
{
    public class ViewCountComparer : IComparer<Video>
    {
        public int Compare(Video lhs, Video rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentNullException();
            }

            return lhs.ViewsCount > rhs.ViewsCount ? -1 : lhs.ViewsCount == rhs.ViewsCount ? 0 : 1;
        }
    }
}