using Npgsql;

namespace DataLayer;

public abstract class BaseSQL<T> where T : new() {
    /// <summary>
    /// Stores data in the database
    /// Set CurrentQuery to the SQL query you want to run
    /// </summary>
    protected void Store(NpgsqlCommand cmd) {
        var db = new Library.Database();
        cmd.Connection = db.Conn;
        db.Store(cmd); // this finalises as well
    }

    /// <summary>
    /// Fetches data from the database
    /// This is to get only one result
    /// Set CurrentQuery to the SQL query you want to run
    /// </summary>
    protected async Task<T> QueryOne(NpgsqlCommand cmd) {
        var db = new Library.Database();
        cmd.Connection = db.Conn;
        var result = await db.Query(cmd); // this does not finalise

        result.Read();

        var model = new T();
        ReadTables(result);

        await result.CloseAsync();

        // upon done reading the reader
        db.Finalise();

        // finally return the result as a class
        return model;
    }

    /// <summary>
    /// Fetches data from the database
    /// This is to get multiple results as a <see cref="List{T}"/>
    /// Set CurrentQuery to the SQL query you want to run
    /// </summary>
    protected async Task<List<T>> QueryMultiple(NpgsqlCommand cmd) {
        var db = new Library.Database();

        cmd.Connection = db.Conn;
        var result = await db.Query(cmd); // this does not finalise

        List<T> listModels = new List<T>();

        while (result.Read()) {
            var model = ReadTables(result);
            listModels.Add(model);
        }

        await result.CloseAsync();

        // upon done reading the reader
        db.Finalise();

        // finally return the result as a class
        return listModels;
    }

    /// <summary>
    /// Use this class to define how to convert a reader row into the model class
    /// </summary>
    /// <param name="reader"><see cref="NpgsqlDataReader"/></param>
    /// <returns><see cref="T"/></returns>
    protected abstract T ReadTables(NpgsqlDataReader reader);
}