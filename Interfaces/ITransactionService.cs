using Tp_ProgramacionIII.DTOs;
using Tp_ProgramacionIII.Models;


namespace Tp_ProgramacionIII.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> Get();
        Task<TransactionDTO?> Gett(int id);
        Task<TransactionDTO> AddTransaction(TransactionDTO transactionDTO);
        Task<bool> UpdateTransaction(int id, TransactionDTO transactionDTO);
        Task<bool> DeleteTransaction(int id);


    }
}
