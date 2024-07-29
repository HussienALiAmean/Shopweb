namespace Shop_web_API.Models.enums
{
    public enum OrderStatus
    {
        ProductQuantityExceededCurruntQuantity,
        ProductQuantityOutOfStock,
        ProductQuantityAvailable,
        ProductIsNotVisible,
        OrderDone,
        OrderError,
    }

}
