using System;
using System.Collections.Generic;
using Videohosting.Models;

namespace Videohosting.Controllers.Comparers
{
    public class LikeComparer : IComparer<Video>
    {
        public int Compare(Video lhs, Video rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentNullException();
            }

            return lhs.LikesCount > rhs.LikesCount ? -1 : lhs.LikesCount == rhs.LikesCount ? 0 : 1;
        }
    }
}