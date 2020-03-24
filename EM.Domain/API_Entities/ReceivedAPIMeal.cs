using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Domain.API_Entities
{
    public class ReceivedAPIMeal
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StarterId { get; set; }
        public int MainId { get; set; }
        public int DessertId { get; set; }
    }
}
