using System.Collections.Concurrent;

namespace Language.Api.Transfers;

public class SyntaxToSemanticContext
{
    private readonly ConcurrentDictionary<Type, Lazy<object>> _commonData;
    
    public SyntaxToSemanticContext()
    {
        _commonData = new ConcurrentDictionary<Type, Lazy<Object>>();
    }

    public T GetData<T>() where T: new()
    {
        return (T)_commonData.GetOrAdd(typeof(T), t => new Lazy<Object>(() => new T())).Value;
    }
}