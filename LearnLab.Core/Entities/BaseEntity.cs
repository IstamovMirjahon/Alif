using System.Diagnostics.CodeAnalysis;

namespace LearnLab.Core.Entities
{
    class BaseEntity : IHasisDeletedProporty
    {
        public Guid Id { get; set; }
        [AllowNull]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [AllowNull]
        public DateTime UpdateDate { get; set; }
        [AllowNull]
        public DateTime DeleteDate { get; set; }

    }
}
