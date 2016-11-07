using SQLite;

namespace YemekDeneme
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
