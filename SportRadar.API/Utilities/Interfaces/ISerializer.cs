namespace SportRadar.API.Utilities.Interfaces
{
    public interface ISerializer
    {
        public Task<string> SerializeAsync<T>(T input);
    }
}
