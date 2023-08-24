namespace CostsSettler.Tests.Helpers;

public abstract class RandomFactoryBase<TModel, TModelAttributes> 
    where TModel : class 
    where TModelAttributes : class
{
    protected abstract TModel CreateModel(Guid id, TModelAttributes? attributes = null);

    public TModel Create(Guid? id = null, TModelAttributes? attributes = null)
    {
        id = id ?? Guid.NewGuid();
        return CreateModel((Guid) id, attributes);
    }

    public ICollection<TModel> CreateCollection(int count)
    {
        var list = new List<TModel>();

        for (int i = 0; i < count; i++)
        {
            list.Add(Create());
        }

        return list;
    }

    public ICollection<TModel> CreateCollection(ICollection<Guid> guids)
    {
        var list = new List<TModel>();

        foreach (var guid in guids)
        {
            list.Add(Create(guid));
        }

        return list;
    }
}