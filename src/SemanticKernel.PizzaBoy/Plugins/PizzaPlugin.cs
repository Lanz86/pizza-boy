using System;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SemanticKernel.PizzaBoy.Plugins;

public class PizzaPlugin
{
    private readonly string[]  _ingredients = [
        "pommodoro",
        "mozzarella",
        "funghi",
        "prosciutto cotto",
        "prosciutto crudo",
        "wuster",
        "patatine",
        "burrata",
        "ananas"
    ];

    private OrderModel order = new();

    [KernelFunction("get_order")]
    [Description("Ottiene l'ordine corrente")]
    public async Task<OrderModel> GetOrderAsync() 
    {
        await Task.Delay(1000);
        return order;
    }

    [KernelFunction("add_ingredients")]
    [Description("Aggiunge gli ingredienti alla pizza dell'ordine")]
    public async Task AddIngredientsAsync(string[] ingredients)
    {
        await Task.Delay(1000);
        foreach (var ingredient in ingredients)
        {
            if (!_ingredients.Contains(ingredient))
            {
                throw new ArgumentException($"Ingredient {ingredient} is not valid");
            }
        }
        order?.Pizza?.Ingredients.AddRange(ingredients);
    }

    [KernelFunction("change_order_status")]
    [Description("Cambia lo stato dell'ordine corrente")]
    public async Task <OrderModel> ChangeOrderStatus(OrderModel.OrderStatus status) {
        await Task.Delay(1000);
        order.Status = status;
        return order;
    }

    [KernelFunction("get_order_status")]
    [Description("Ottiene lo stato dell'ordine corrente")]
    public async Task<OrderModel.OrderStatus> GetOrderStatus()
    {
        await Task.Delay(1000);
        return order.Status;
    }


    [KernelFunction("create_order")]
    [Description("Crea un nuovo ordine")]
    public async Task<OrderModel> CreateNewOrderAsync(string[]? ingredients)
    {
        await Task.Delay(1000);
        order = new();
        if(ingredients.Length > 0)
        {
            order.Pizza.Ingredients.AddRange(ingredients);
        }
        return order;
    }

    public class OrderModel 
    {

        public PizzaModel? Pizza { get; set; } = new();
        public OrderStatus Status { get; set; }
        public enum OrderStatus {
            Pending,
            Delivery,
            Delivered
        }
    }
    
    public class PizzaModel 
    {
        public List<string> Ingredients { get; set; } = new();
    }
}
