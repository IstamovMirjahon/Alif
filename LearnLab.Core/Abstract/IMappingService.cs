
namespace LearnLab.Core.Abstract;

public interface IMappingService
{
    T Map<T, TSource>(TSource source);

    T Map<T, TSource>(TSource source, T destination);
}