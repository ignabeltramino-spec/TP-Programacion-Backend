using Microsoft.AspNetCore.Mvc;
using Tp_ProgramacionIII.DTOs;
using Tp_ProgramacionIII.Interfaces;

namespace Tp_ProgramacionIII.Controllers
{
    [ApiController]
    [Route("Transaction")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _TransactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _TransactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transaccion = await _TransactionService.Get();
            return Ok(transaccion);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> Gett(int id)
        {
            var transaccion = await _TransactionService.Gett(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            return Ok(transaccion);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TransactionDTO transactionsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevaTransaccion = await _TransactionService.AddTransaction(transactionsDTO);

            return Ok(nuevaTransaccion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TransactionDTO transactionsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool exito = await _TransactionService.UpdateTransaction(id, transactionsDTO);

                if (!exito)
                {
                    return NotFound($"No se encontró la transacción con ID {id} para actualizar.");
                }

                return Ok(new { message = "Transacción actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la transacción: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool resultado = await _TransactionService.DeleteTransaction(id);

                if (!resultado)
                {
                    return NotFound($"No se encontró la transacción con ID {id} para eliminar.");
                }

                return Ok(new { message = "Transacción eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la transacción: {ex.Message}");
            }
        }
    }
}


