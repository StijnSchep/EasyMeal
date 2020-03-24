using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EM.Domain;
using EM.Domain.Chef_Entities;

namespace EM.GUI_Chef.Models.ViewModels
{
    public class WeekdayViewModel
    {
        public WeekDay day { get; set; }
        public int dayIndex { get; set; }
    }
}
