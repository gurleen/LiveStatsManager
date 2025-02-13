using Optional;

namespace LiveStatsManager.Services;

public class DatabaseRepository(IServiceProvider services)
{
    private readonly Database _database = services.GetRequiredService<Database>();
    
    public void InsertSingleton<T>(T obj)
    {
        using var db = _database.Session();
        var collection = db.GetCollection<T>(typeof(T).Name);
        collection.DeleteAll();
        collection.Insert(obj);
    }
    
    public Option<T> GetSingleton<T>()
    {
        using var db = _database.Session();
        var collection = db.GetCollection<T>(typeof(T).Name);
        return collection.Count() < 1 ? 
            Option.None<T>() : 
            Option.Some(collection.FindOne(x => true));
    }
}