namespace CostsSettler.Tests.Helpers;

/// <summary>
/// Factory for random domain model.
/// </summary>
/// <typeparam name="TModel">Domain model.</typeparam>
/// <typeparam name="TModelAttributes">TModel attributes that should be applied.</typeparam>
public abstract class RandomFactoryBase<TModel, TModelAttributes> 
    where TModel : class 
    where TModelAttributes : class
{
    /// <summary>
    /// Creates random TModel for given attributes.
    /// </summary>
    /// <param name="id">TModel id that will be set.</param>
    /// <param name="attributes">TModel attributes to apply.</param>
    /// <returns>New TModel object.</returns>
    protected abstract TModel CreateModel(Guid id, TModelAttributes? attributes = null);

    /// <summary>
    /// Creates random TModel for given attributes.
    /// </summary>
    /// <param name="id">TModel id to be set. If null, new Guid is generated.</param>
    /// <param name="attributes">TModel attributes to apply.</param>
    /// <returns>New TModel object.</returns>
    public TModel Create(Guid? id = null, TModelAttributes? attributes = null)
    {
        id = id ?? Guid.NewGuid();
        return CreateModel((Guid) id, attributes);
    }

    /// <summary>
    /// Creates collection of TModel objects.
    /// </summary>
    /// <param name="count">Number of objects to create.</param>
    /// <returns>'count' new TModel objects.</returns>
    public ICollection<TModel> CreateCollection(int count)
    {
        var list = new List<TModel>();

        for (int i = 0; i < count; i++)
        {
            list.Add(Create());
        }

        return list;
    }
}