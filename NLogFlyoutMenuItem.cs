using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    public class NLogFlyoutMenuItem
    {
        public NLogFlyoutMenuItem()
        {
            TargetType = typeof(NLogFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}