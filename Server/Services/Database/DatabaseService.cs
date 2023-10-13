using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CalculatorApp.Server.Services.Database
{
	/// <summary>
	/// Služba pro práci s SQL databázi
	/// </summary>
	public class DatabaseService : IDatabaseService
    {
        private readonly ILogger<DatabaseService> _logger;
        private readonly string? _connectionString;
        private readonly IConfiguration _config;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="logger">Služba pro logování</param>
        /// <param name="configuration">Služba pro konfiguraci</param>
        public DatabaseService(ILogger<DatabaseService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _connectionString = GetConnectionString();            
        }

        /// <summary>
        /// Metoda pro uložení příladu do SQL databáze
        /// </summary>
        /// <param name="example">Příklad</param>		
        public async Task SaveExampleAsync(string example)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                DynamicParameters parameters = new();
                parameters.Add("Example", example);

                await connection.ExecuteAsync("dbo.AddExample", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {               
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

            _logger.LogInformation($"Příklad {example} uložen do databáze");
        }

        /// <summary>
        /// Metoda pro získání seznamu výpočtů
        /// </summary>
        /// <returns>Vrací posledních 14 výpočtů</returns>
        public async Task<List<string>> GetExamplesAsync()
        {            
            List<string> examples = new();

            try
            {
                using var connection = new SqlConnection(_connectionString);   
                examples = (await connection.QueryAsync<string>("dbo.GetExamples", commandType: CommandType.StoredProcedure)).ToList();
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex.Message);
				throw new Exception(ex.Message);
			}

            _logger.LogInformation("Z databáze načten seznam příkladů");
            return examples;
        }

        /// <summary>
        /// Získá connection string pro SQL databázi
        /// </summary>
        /// <returns>Connection string</returns>
        private string GetConnectionString()
        {
            return _config.GetConnectionString("LocalDB") ?? throw new Exception($"Nelze načíst připojení k databázi");
        }
    }
}