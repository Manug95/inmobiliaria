namespace InmobiliariaGutierrezManuel.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string connectionString;

        public BaseRepository()
        {
            this.connectionString = "server=127.0.0.1;uid=root;pwd=root;database=inmobiliaria";
        }
    }
}