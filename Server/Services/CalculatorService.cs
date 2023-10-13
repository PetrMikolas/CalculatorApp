using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using CalculatorApp.Server.Services.Database;
using CalculatorApp.Shared;

namespace CalculatorApp.Server.Services
{
	/// <summary>
	/// Služba pro ukládání a načítání výpočtů
	/// </summary>
	public class CalculatorService : CalculatorApi.CalculatorApiBase
	{
		private readonly ILogger<CalculatorService> _logger;
		private readonly IDatabaseService _databaseService;

		/// <summary>
		/// Konstruktor
		/// </summary>
		/// <param name="logger">Služba pro logování</param>
		/// <param name="databaseService">Služba pro práci s SQL databázi</param>
		public CalculatorService(ILogger<CalculatorService> logger, IDatabaseService databaseService)
		{
			_logger = logger;
			_databaseService = databaseService;
		}

		/// <summary>
		/// Služba pro získání výsledku početní operace
		/// </summary>
		/// <param name="request">Požadavek na získání výsledku</param>
		/// <param name="context">Kontext ke kontrétnímu volání rozhraní</param>
		/// <returns>V případě úspěchu vrátí výsledek</returns>
		public override async Task<Empty> SaveExample(SaveExampleRequest request, ServerCallContext context)
		{
			_logger.LogInformation($"Požadavek na uložení {request}");

			try
			{
				string example = request.Example;

				await _databaseService.SaveExampleAsync(example);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw new Exception($"Chyba při ukládání příkladu do databáze: {ex.Message}");
			}

			return new Empty();
		}

		/// <summary>
		/// Služba pro získání historie výpočtů
		/// </summary>
		/// <param name="request">Požadavek - prázdná zpráva</param>
		/// <param name="context">Kontext ke kontrétnímu volání rozhraní</param>
		/// <returns>V případě úspěchu vrátí posledních 14 výpočtů</returns>
		public override async Task<GetExamplesResponse> GetExamples(Empty request, ServerCallContext context)
		{
			_logger.LogInformation($"Požadavek na získání seznamu příkladů");

			try
			{
				return new GetExamplesResponse()
				{
					Examples = { await _databaseService.GetExamplesAsync() },
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				throw new Exception($"Chyba při načítání dat z databáze: {ex.Message}");
			}
		}
	}
}
