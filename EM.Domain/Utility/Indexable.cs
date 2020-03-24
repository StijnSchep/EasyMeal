using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Utility
{
    public abstract class Indexable
    {

        [Key]
        public int Id { get; set; }

    }
}
