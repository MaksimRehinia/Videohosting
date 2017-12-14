using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videohosting.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public virtual Channel Subscriber { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
    }
}