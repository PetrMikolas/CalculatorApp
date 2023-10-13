namespace CalculatorApp.Server.Services.Database
{
    /// <summary>
    /// Specifikace rozhraní pro výpočet a práci s SQL databázi
    /// </summary>
    public interface IDatabaseService
    {
        public Task SaveExampleAsync(string priklad);

        /// <summary>
        /// Metoda pro získání seznamu výpočtů
        /// </summary>
        /// <returns>Vrací posledních 14 výpočtů</returns>
        public Task<List<string>> GetExamplesAsync();        
    }
}