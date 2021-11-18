using PetStore.Store.Api.Model;
using PetStore.Store.Api.Model.Commands;
using PetStore.Store.Api.Model.Queries;

namespace PetStore.Store.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    private readonly IGetInventory _getInventory;
    private readonly IGetOrder _getOrder;
    private readonly IDeleteOrder _deleteOrder;
    private readonly IInsertOrder _insertOrder;

    public ApiController(IGetInventory getInventory, IGetOrder getOrder, IDeleteOrder deleteOrder, IInsertOrder insertOrder)
    {
        _getInventory = getInventory;
        _getOrder = getOrder;
        _deleteOrder = deleteOrder;
        _insertOrder = insertOrder;
    }

    /// <summary>
    /// Returns pet inventories by status
    /// </summary>
    /// <returns>Returns a map of status codes to quantities</returns>
    [HttpGet("/v1/store/inventory")]
    public ActionResult<IList<InventoryLine>> Get()
    {
        var inventory = _getInventory.Execute();

        return Ok(inventory);
    }
        
    /// <summary>
    /// Find purchase order by ID
    /// </summary>
    /// <param name="orderId">ID of the purchase order</param>
    /// <response code="404">Purchase was not found</response>
    /// <returns>Returns the relevant purchase order</returns>
    [HttpGet("/v1/store/order/{orderId}")]
    public ActionResult<Order> Get(int orderId)
    {
        var order = _getOrder.Execute(orderId);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }
        
    /// <summary>
    /// Delete purchase order by ID
    /// </summary>
    /// <param name="orderId">ID of the purchase order</param>
    /// <response code="200">Purchase has been deleted</response>
    /// <response code="404">Purchase was not found</response>
    [HttpDelete("/v1/store/order/{orderId}")]
    public ActionResult Delete(int orderId)
    {
        var order = _getOrder.Execute(orderId);
        if (order == null)
        {
            return NotFound();
        }
            
        _deleteOrder.Execute(orderId);

        return Ok();
    }
        
    /// <summary>
    /// Place an order
    /// </summary>
    /// <param name="order">Order placed for purchasing the pet</param>
    /// <response code="400">Invalid order</response>
    [HttpPost("/v1/store/order")]
    public ActionResult<Order> Place([FromBody] Order order)
    {
        if (order == null)
        {
            return StatusCode(400);
        }

        var orderId = _insertOrder.Execute(order);

        var result = _getOrder.Execute(orderId);

        return Ok(result);
    }
}