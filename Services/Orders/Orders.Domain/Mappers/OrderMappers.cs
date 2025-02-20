using Orders.Domain.DTOs.Orders;
using Orders.Domain.Entities;
using Shared.Contracts.Events.OrderEvents;

namespace Orders.Domain.Mappers;

public static class OrderMappers
{
    public static OrderResponse ToOrderResponse(this Order order)
    {
        return new OrderResponse
        {
            OrderId = order.OrderId,
            UserId = order.UserId.ToString(),
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            ShippingId = order.ShippingId,
            PaymentId = order.PaymentId,
            OrderItems = order.OrderItems.Select(oi => oi.ToOrderItemResponse()).ToList()
        };
    }

    private static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
    {
        return new OrderItemResponse
        {
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            Subtotal = orderItem.Subtotal
        };
    }

    public static Order ToOrder(this CreateOrderRequest request)
    {
        return new Order
        {
            UserId = request.UserId,
            OrderDate = request.OrderDate,
            Status = request.Status,
            TotalAmount = request.TotalAmount,
            ShippingId = request.ShippingId,
            PaymentId = request.PaymentId,
            OrderItems = request.OrderItems.Select(oi => oi.ToOrderItem()).ToList()
        };
    }

    public static OrderItem ToOrderItem(this CreateOrderItemRequest request)
    {
        return new OrderItem
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Subtotal = request.Quantity * request.UnitPrice
        };
    }
    
    public static OrderCreatedEvent ToOrderCreatedEvent(this Order order)
    {
        return new OrderCreatedEvent
        {
            OrderId = order.OrderId.ToString(),
            OrderDate = order.OrderDate,
            PaymentId = order.PaymentId,
            ShippingId = order.ShippingId,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => oi.ToOrderItemEvent()).ToList()
        };
    }

    public static OrderCreatedEvent.OrderItemEvent ToOrderItemEvent(this OrderItem orderItem)
    {
        return new OrderCreatedEvent.OrderItemEvent
        {
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            Subtotal = orderItem.Subtotal
        };
    }
}