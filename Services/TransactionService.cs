using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tp_ProgramacionIII.DTOs;
using Tp_ProgramacionIII.Interfaces;
using Tp_ProgramacionIII.Models;

namespace Tp_ProgramacionIII.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        public TransactionService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }                       

        public async Task<List<TransactionDTO>> Get()
        {
            var transaction = await _context.Transacciones
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();
            var transactionDTO = transaction.Select(t => new TransactionDTO
            {
                Id = t.Id,
                CryptoCode = t.CryptoCode,
                Action = t.Action,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                DateTime = t.DateTime,
                ClientId = t.ClientId,
            }).ToList();
            return transactionDTO;

        }

        public async Task<TransactionDTO?> Gett(int id)
        {
            var transacciones = await _context.Transacciones.FirstOrDefaultAsync(g => g.Id == id);
            if (transacciones == null)
            {
                return null;
            }
            return new TransactionDTO
            {
                Id = transacciones.Id,
                CryptoCode = transacciones.CryptoCode,
                Action = transacciones.Action,
                CryptoAmount = transacciones.CryptoAmount,
                Money = transacciones.Money,
                DateTime = transacciones.DateTime,
            };
        }

        public async Task<TransactionDTO> AddTransaction(TransactionDTO transactionDTO)
        {
            string cryptoCodeLower = transactionDTO.CryptoCode.ToLower();

            string urlCriptoya = $"https://criptoya.com/api/fiwind/{cryptoCodeLower}/ars";

            decimal precio = 0;
            try
            {
                var response = await _httpClient.GetAsync(urlCriptoya);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(jsonString))
                    {
                        precio = doc.RootElement.GetProperty("totalAsk").GetDecimal();
                    }
                }
                else
                {
                    throw new Exception("No se pudo obtener el precio desde CryptoYa");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar CryptoYa" + ex.Message);
            }

            decimal totalGastado = precio * transactionDTO.CryptoAmount;

            var nuevaTransaccion = new Transaction
            {
                CryptoCode = transactionDTO.CryptoCode,
                Action = transactionDTO.Action,
                CryptoAmount = transactionDTO.CryptoAmount,
                Money = totalGastado,
                DateTime = transactionDTO.DateTime,
                ClientId = transactionDTO.ClientId,
            };

            _context.Transacciones.Add(nuevaTransaccion);
            await _context.SaveChangesAsync();

            var nuevaTransactionDTO = new TransactionDTO
            {
                Id = nuevaTransaccion.Id,
                CryptoCode = nuevaTransaccion.CryptoCode,
                Action = nuevaTransaccion.Action,
                CryptoAmount = nuevaTransaccion.CryptoAmount,
                Money = nuevaTransaccion.Money,
                DateTime = nuevaTransaccion.DateTime,
            };

            return nuevaTransactionDTO;
        }

        public async Task<bool> UpdateTransaction(int id, TransactionDTO transactionDTO)
        {
            var transaccionExistente = await _context.Transacciones.FindAsync(id);

            if (transaccionExistente == null)
            {
                return false;
            }


            transaccionExistente.CryptoCode = transactionDTO.CryptoCode;
            transaccionExistente.Action = transactionDTO.Action;
            transaccionExistente.CryptoAmount = transactionDTO.CryptoAmount;
            transaccionExistente.Money = transactionDTO.Money;
            transaccionExistente.DateTime = transactionDTO.DateTime;

            _context.Transacciones.Update(transaccionExistente);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteTransaction(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);

            if (transaccion == null)
            {
                return false;

            }

            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
