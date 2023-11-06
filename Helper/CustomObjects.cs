using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAdmin.Helper
{
    public abstract class BaseDropSkelaton
    {
        public int? Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class CustomCity : BaseDropSkelaton
    {
    }
    public class CustomCounty : BaseDropSkelaton
    {
    }
}