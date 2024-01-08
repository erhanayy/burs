using MyAdmin;
using MyAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YourNamespace.ViewModels
{
    public class RegisterViewModel
    {
        public Contact Contact { get; set; }

        public List<ContactType> ContactTypes { get; set; }

        public int SelectedContactTypeId { get; set; }
    }
}
