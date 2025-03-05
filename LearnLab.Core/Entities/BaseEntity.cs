using System.Diagnostics.CodeAnalysis;

namespace LearnLab.Core.Entities
{
    public class BaseEntity : IHasisDeletedProporty
    {
        public Guid Id { get; set; }
        [AllowNull]
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        [AllowNull]
        public DateTimeOffset UpdateDate { get; set; }
        [AllowNull]
        public DateTimeOffset DeleteDate { get; set; }

    }
}
